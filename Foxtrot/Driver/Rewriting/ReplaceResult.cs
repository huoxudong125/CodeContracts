// CodeContracts
// 
// Copyright (c) Microsoft Corporation
// 
// All rights reserved. 
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Compiler;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Microsoft.Contracts.Foxtrot
{
    internal sealed class ReplaceResult : StandardVisitor
    {
        /// <summary>
        /// Result starts out as a Local that is created by the client of this class. But if during
        /// the replacement of Result&lt;T&gt;(), an occurrence is found within an anonymous delegate,
        /// then the local is replaced with a member binding.
        /// 
        /// If the delegate is within a closure class, then the member binding is "this.f", where
        /// f is a new field that is added to the top-level closure class and "this" will be the
        /// type of the top-level closure class.
        /// 
        /// If the delegate is a static method added to the class itself, then the member binding
        /// is "null.f" where f is a new static field that is added to the class containing the method
        /// that contained the contract in which this occurrence of Result was found.
        /// 
        /// When this happens, a client should use the value of ReplaceResult.Result
        /// to assign the return value of the method to.
        /// 
        /// Note: The client should assign the value that the method returns to *both* the local
        /// originally passed in to the constructor of this visitor *and* to the member binding
        /// (if Result is modified because there is an occurrence of Result&lt;T&gt()
        /// in a delegate) because if any occurrences are found before visiting the closure,
        /// those occurrences will be replaced by the local and not the member binding!
        /// 
        /// When Result is found *not* within a delegate, then it is always replaced with
        /// the original local that the client handed to the constructor.
        /// </summary>
        private readonly Local originalLocalForResult;

        /// <summary>
        /// if non-null, used to store result in declaring type.
        /// NOTE: the field type is the type of the
        /// result in the instantiated context. For generic closure classes (which are generated by the 
        /// rewriter during copying of OOB contracts), the closure potentially expects a generic form for
        /// the result value. We thus have to generate access to the result by casting (box InstanceType, unbox GenericType).
        /// </summary>
        private Field topLevelStaticResultField;

        /// <summary>
        /// if non-null, used to store result in top-level closure instance. 
        /// NOTE: the field type is the type of the
        /// result in the instantiated context. For generic closure classes (which are generated by the 
        /// rewriter during copying of OOB contracts), the closure potentially expects a generic form for
        /// the result value. We thus have to generate access to the result by casting (box InstanceType, unbox GenericType).
        /// </summary>
        private Field topLevelClosureResultField;

        public IEnumerable<MemberBinding> NecessaryResultInitialization(Dictionary<TypeNode, Local> closureLocals)
        {
            if (topLevelStaticResultField != null)
            {
                yield return new MemberBinding(null, topLevelStaticResultField);
            }

            if (topLevelClosureResultField != null)
            {
                // note: this field is the field in the generic context of the closure. For the method to initialize this
                // we need the instantiated form, which we remember in topLevelClosureClassInstance
                Local local;
                if (closureLocals.TryGetValue(topLevelClosureClassInstance, out local))
                {
                    yield return
                        new MemberBinding(local,
                            GetInstanceField(this.originalLocalForResult.Type, topLevelClosureResultField,
                                topLevelClosureClassInstance));
                }
                else
                {
                    var access = new This(topLevelClosureClassInstance);
                    yield return
                        new MemberBinding(access,
                            GetInstanceField(this.originalLocalForResult.Type, topLevelClosureResultField,
                                topLevelClosureClassInstance));
                }
            }
        }

        public IEnumerable<MemberBinding> NecessaryResultInitializationAsync(Dictionary<Local, MemberBinding> closureLocals)
        {
            if (topLevelStaticResultField != null)
            {
                yield return new MemberBinding(null, topLevelStaticResultField);
            }

            if (topLevelClosureResultField != null)
            {
                // note: this field is the field in the generic context of the closure. For the method to initialize this
                // we need the instantiated form, which we remember in topLevelClosureClassInstance
                MemberBinding mb = null;
                foreach (var pair in closureLocals)
                {
                    var keyType = HelperMethods.Unspecialize(pair.Key.Type);
                    if (keyType == topLevelClosureClassDefinition)
                    {
                        mb = pair.Value;
                        yield return
                            new MemberBinding(mb,
                                GetInstanceField(this.originalLocalForResult.Type, topLevelClosureResultField,
                                    topLevelClosureClassInstance));
                        break;
                    }
                }

                Debug.Assert(mb != null, "Should have found access");
            }
        }

        private readonly Module assemblyBeingRewritten;
        private Method currentClosureMethod; // definition
        private Expression currentAccessToTopLevelClosure;

        private TypeNode topLevelClosureClassDefinition;
        private TypeNode topLevelClosureClassInstance;
        private TypeNode currentClosureClassInstance;
        private readonly TypeNodeList topLevelMethodFormals;

        /// <summary>
        /// Used when the method with the closure is generic and the field ends up on the corresponding generic closure class
        /// </summary>
        private TypeNode properlyInstantiatedFieldType;

        private int delegateNestingLevel;
        private readonly TypeNode declaringType; // needed to copy anonymous delegates into

        public ReplaceResult(Method containingMethod, Local originalLocalForResult, Module assemblyBeingRewritten)
        {
            Contract.Requires(containingMethod != null);

            this.assemblyBeingRewritten = assemblyBeingRewritten;
            this.declaringType = containingMethod.DeclaringType;
            this.topLevelMethodFormals = containingMethod.TemplateParameters;
            this.originalLocalForResult = originalLocalForResult;
            this.delegateNestingLevel = 0;
        }

        /// <summary>
        /// This property would be true, if Contract.Result&lt;T&gt;() was captured in the static context.
        /// For instance, following code will lead to this situation: 
        ///         public string Method(params string[] strings)
        ///    {
        ///        Contract.Ensures(Contract.ForAll(strings, s => Contract.Result&lt;string&gt;() == s));
        ///        return "42";
        ///    }
        ///
        /// In this case, caller of this code can decide to emit a warning, because current behavior
        /// could lead to issues in multithreaded environment
        /// </summary>
        public bool ContractResultWasCapturedInStaticContext { get; private set; }

        public override Expression VisitReturnValue(ReturnValue returnValue)
        {
            if (this.delegateNestingLevel == 0)
            {
                // not inside a closure method
                return this.originalLocalForResult;
            }

            if (this.currentClosureMethod.IsStatic || IsRoslynBasedStaticClosure())
            {
                // This is a hack and we should notify about it!
                ContractResultWasCapturedInStaticContext = true;

                // static closure: no place to store result. Current hack is to use a static field of the 
                // declaring type. However, if we have a static closure inside a non-static closure, this
                // breaks down, as we support only one kind of storage for return values.
                var field = GetReturnValueClosureField(this.declaringType, this.originalLocalForResult.Type,
                    FieldFlags.CompilerControlled | FieldFlags.Private | FieldFlags.Static,
                    originalLocalForResult.UniqueKey);

                Contract.Assume(returnValue != null);

                return CreateProperResultAccess(returnValue, null, field);
            }

            Debug.Assert(this.currentAccessToTopLevelClosure != null);
            Debug.Assert(this.topLevelClosureClassDefinition != null);

            {
                // Return an expression that is the value of the field defined in the
                // top-level closure class to hold the method's return value.
                // This will be this.up.Result where "up" is the field C#
                // generated to point to the instance of the top-level closure class.
                // "Result" is the field defined in this visitor's VisitConstruct when
                // it finds a reference to a anonymous delegate.
                var field = GetReturnValueClosureField(this.topLevelClosureClassDefinition,
                    this.properlyInstantiatedFieldType, FieldFlags.CompilerControlled | FieldFlags.Assembly,
                    this.topLevelClosureClassDefinition.UniqueKey);

                Contract.Assume(returnValue != null);

                return CreateProperResultAccess(returnValue, this.currentAccessToTopLevelClosure, field);
            }
        }

        /// <summary>
        /// Roslyn-based compiler changed the pattern for caching static (i.e. non-capturing) lambdas.
        /// This method returns true if currentClosureClassInstance is a static closure class generated by the Roslyn-based compiler.
        /// </summary>
        private bool IsRoslynBasedStaticClosure()
        {
            return currentClosureClassInstance.IsRoslynBasedStaticClosure();
        }

        private static Expression CreateProperResultAccess(ReturnValue returnValue, Expression closureObject, Field resultField)
        {
            Contract.Requires(returnValue != null);
            Contract.Requires(resultField != null);

            var fieldAccess = new MemberBinding(closureObject, resultField);

            if (resultField.Type != returnValue.Type)
            {
                // must cast to generic type expected in this context (box instance unbox.any Generic)
                return
                    new BinaryExpression(
                        new BinaryExpression(fieldAccess, new Literal(resultField.Type), NodeType.Box),
                        new Literal(returnValue.Type), NodeType.UnboxAny);
            }
            
            return fieldAccess;
        }

        /// <summary>
        /// If there is an anonymous delegate within a postcondition, then there
        /// will be a call to a delegate constructor.
        /// That call looks like "d..ctor(o,m)" where d is the type of the delegate.
        /// There are two cases depending on whether the anonymous delegate captured
        /// anything. In both cases, m is the method implementing the anonymous delegate.
        /// (1) It does capture something. Then o is the instance of the closure class
        /// implementing the delegate, and m is an instance method in the closure
        /// class.
        /// (2) It does *not* capture anything. Then o is the literal for null and
        /// m is a static method that was added directly to the class.
        /// 
        /// This method will cause the method (i.e., m) to be visited to collect any
        /// Result&lt;T&gt;() expressions that occur in it.
        /// </summary>
        /// <param name="cons">The AST representing the call to the constructor
        /// of the delegate type.</param>
        /// <returns>Whatever the base visitor returns</returns>
        public override Expression VisitConstruct(Construct cons)
        {
            if (cons.Type is DelegateNode)
            {
                UnaryExpression ue = cons.Operands[1] as UnaryExpression;
                if (ue == null) goto JustVisit;

                MemberBinding mb = ue.Operand as MemberBinding;
                if (mb == null) goto JustVisit;

                Method m = mb.BoundMember as Method;
                if (!HelperMethods.IsCompilerGenerated(m)) goto JustVisit;

                Contract.Assume(m != null);

                m = Definition(m);
                this.delegateNestingLevel++;

                TypeNode savedClosureClass = this.currentClosureClassInstance;
                Method savedClosureMethod = this.currentClosureMethod;
                Expression savedCurrentAccessToTopLevelClosure = this.currentAccessToTopLevelClosure;

                try
                {
                    this.currentClosureMethod = m;

                    if (m.IsStatic)
                    {
                        this.currentClosureClassInstance = null; // no closure object
                    }
                    else
                    {
                        this.currentClosureClassInstance = cons.Operands[0].Type;
                        if (savedClosureClass == null)
                        {
                            // Then this is the top-level closure class.
                            this.topLevelClosureClassInstance = this.currentClosureClassInstance;
                            this.topLevelClosureClassDefinition = Definition(this.topLevelClosureClassInstance);

                            this.currentAccessToTopLevelClosure = new This(this.topLevelClosureClassDefinition);
                            this.properlyInstantiatedFieldType = this.originalLocalForResult.Type;

                            if (this.topLevelMethodFormals != null)
                            {
                                Contract.Assume(this.topLevelClosureClassDefinition.IsGeneric);
                                Contract.Assume(topLevelClosureClassDefinition.TemplateParameters.Count >=
                                                this.topLevelMethodFormals.Count);

                                // replace method type parameters in result properly with last n corresponding type parameters of closure class
                                TypeNodeList closureFormals = topLevelClosureClassDefinition.TemplateParameters;
                                if (closureFormals.Count > this.topLevelMethodFormals.Count)
                                {
                                    int offset = closureFormals.Count - this.topLevelMethodFormals.Count;
                                    closureFormals = new TypeNodeList(this.topLevelMethodFormals.Count);
                                    for (int i = 0; i < this.topLevelMethodFormals.Count; i++)
                                    {
                                        closureFormals.Add(topLevelClosureClassDefinition.TemplateParameters[i + offset]);
                                    }
                                }

                                Duplicator dup = new Duplicator(this.declaringType.DeclaringModule, this.declaringType);

                                Specializer spec = new Specializer(this.declaringType.DeclaringModule,
                                    topLevelMethodFormals, closureFormals);

                                var type = dup.VisitTypeReference(this.originalLocalForResult.Type);
                                type = spec.VisitTypeReference(type);
                                this.properlyInstantiatedFieldType = type;
                            }
                        }
                        else
                        {
                            while (currentClosureClassInstance.Template != null)
                                currentClosureClassInstance = currentClosureClassInstance.Template;

                            // Find the field in this.closureClass that the C# compiler generated
                            // to point to the top-level closure
                            foreach (Member mem in this.currentClosureClassInstance.Members)
                            {
                                Field f = mem as Field;
                                if (f == null) continue;
                                if (f.Type == this.topLevelClosureClassDefinition)
                                {
                                    var consolidatedTemplateParams = this.currentClosureClassInstance.ConsolidatedTemplateParameters;

                                    TypeNode thisType;
                                    if (consolidatedTemplateParams != null && consolidatedTemplateParams.Count > 0)
                                    {
                                        thisType =
                                            this.currentClosureClassInstance.GetGenericTemplateInstance(
                                                this.assemblyBeingRewritten, consolidatedTemplateParams);
                                    }
                                    else
                                    {
                                        thisType = this.currentClosureClassInstance;
                                    }

                                    this.currentAccessToTopLevelClosure = new MemberBinding(new This(thisType), f);

                                    break;
                                }
                            }
                        }
                    }

                    this.VisitBlock(m.Body);
                }
                finally
                {
                    this.delegateNestingLevel--;
                    this.currentClosureMethod = savedClosureMethod;
                    this.currentClosureClassInstance = savedClosureClass;
                    this.currentAccessToTopLevelClosure = savedCurrentAccessToTopLevelClosure;
                }
            }

            JustVisit:
            return base.VisitConstruct(cons);
        }

        private static Method Definition(Method m)
        {
            Contract.Requires(m != null);

            while (m.Template != null) m = m.Template;
            return m;
        }

        private static TypeNode Definition(TypeNode t)
        {
            Contract.Requires(t != null);

            while (t.Template != null) t = t.Template;
            return t;
        }

        private Field GetReturnValueClosureField(TypeNode declaringType, TypeNode resultType, FieldFlags flags, int uniqueKey)
        {
            Contract.Requires(declaringType != null);

            Contract.Assume(declaringType.Template == null);
            Identifier name = Identifier.For("_result" + uniqueKey.ToString()); // unique name for this field

            Field f = declaringType.GetField(name);
            if (f != null) return f;

            f = new Field(declaringType,
                null,
                flags,
                name,
                resultType,
                null);

            declaringType.Members.Add(f);
            // remember we added it so we can make it part of initializations
            if (f.IsStatic)
            {
                topLevelStaticResultField = f;
            }
            else
            {
                topLevelClosureResultField = f;
            }

            return f;
        }

        private static Field GetInstanceField(TypeNode originalReturnType, Field possiblyGenericField, TypeNode instanceDeclaringType)
        {
            Contract.Requires(instanceDeclaringType != null);

            if (instanceDeclaringType.Template == null) return possiblyGenericField;

            var declaringTemplate = instanceDeclaringType;

            while (declaringTemplate.Template != null)
            {
                declaringTemplate = declaringTemplate.Template;
            }

            Contract.Assume(declaringTemplate == possiblyGenericField.DeclaringType);

            return Rewriter.GetFieldInstanceReference(possiblyGenericField, instanceDeclaringType);
#if false
      Field f = instanceDeclaringType.GetField(possiblyGenericField.Name);
      if (f != null)
      {
        // already instantiated
        return f;
      }
      // pseudo instance
      Field instance = new Field(instanceDeclaringType, possiblyGenericField.Attributes, possiblyGenericField.Flags, possiblyGenericField.Name, originalReturnType, null);
      instanceDeclaringType.Members.Add(instance);
      return instance;
#endif
        }
    }
}