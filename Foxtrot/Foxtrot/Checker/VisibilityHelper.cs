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

using System;
using System.Compiler;
using System.Diagnostics.Contracts;

namespace Microsoft.Contracts.Foxtrot
{
    /// <summary>
    /// Checks for a minimum level of visibility in a code tree.
    /// </summary>
    [ContractVerification(true)]
    internal class VisibilityHelper : InspectorIncludingClosures
    {
        private static readonly Lazy<TypeNode> systemAttributeType = new Lazy<TypeNode>(() =>
                HelperMethods.FindType(SystemTypes.SystemAssembly, StandardIds.System, Identifier.For("Attribute")));

        private Member memberInErrorFound;

        /// <summary>
        /// The visibility of things is checked to be as visible as this member.
        /// </summary>
        private Member AsThisMember;

        private void ReInitialize(Method asThisMember)
        {
            this.CurrentMethod = asThisMember;
            memberInErrorFound = null;
            this.AsThisMember = asThisMember;
        }

        private static bool IsAttribute(TypeNode declaringType)
        {
            var currentType = declaringType;
            while (currentType != null)
            {
                if (currentType == systemAttributeType.Value)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Checks for less-than-visible member references in an expression.
        /// </summary>
        public override void VisitMemberBinding(MemberBinding binding)
        {
            if (binding == null) return;

            Member mem = binding.BoundMember;
            if (mem != null)
            {
                // Member visiting includes also all attributes for the method.
                // But from Code Contracts perspective public method can have less visible attributes
                // because they're not part of the precondition.
                if (IsAttribute(mem.DeclaringType)) return;

                Field f = mem as Field;
                bool specPublic = false;

                if (f != null)
                {
                    specPublic = IsSpecPublic(f);
                }
                
                if (!specPublic && !HelperMethods.IsCompilerGenerated(mem))
                {
                    // F: It seems there is some type-state like invariant here justifying why this.AsThisMemeber != null
                    Contract.Assume(this.AsThisMember != null);
                    
                    if (!HelperMethods.IsReferenceAsVisibleAs(mem, this.AsThisMember))
                    {
                        this.memberInErrorFound = mem;
                        return;
                    }
                }
            }

            base.VisitMemberBinding(binding);
        }

        private static bool IsSpecPublic(Field f)
        {
            // F: 
            Contract.Requires(f != null);

            return f.Attributes.HasAttribute(ContractNodes.SpecPublicAttributeName);
        }

        /// <summary>
        /// Returns true if expr is as visible as method
        /// </summary>
        public bool IsAsVisibleAs(Expression expr, Method method)
        {
            return AsVisibleAs(expr, method) == null;
        }

        /// <summary>
        /// Returns null if okay, otherwise a member not as visible as method
        /// </summary>
        public Member AsVisibleAs(Expression expr, Method method)
        {
            this.ReInitialize(method);
            this.VisitExpression(expr);
            return this.memberInErrorFound;
        }
    }
}