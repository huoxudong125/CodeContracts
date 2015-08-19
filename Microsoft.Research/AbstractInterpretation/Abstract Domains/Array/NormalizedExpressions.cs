// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Research.AbstractDomains.Expressions;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Research.AbstractDomains
{
    /// <summary>
    /// NormalizedExpressions are mathematical expressions to be used as array limits.
    /// They have a limited form *on purpose*
    /// 
    /// NormalizedExpression ::= Constant | Var | Var + Constant
    /// </summary>
    [ContractVerification(true)]
    [ContractClass(typeof(NormalizedExpressionContracts<>))]
    abstract public class NormalizedExpression<Variable>
    {
        abstract public bool IsConstant(out int value);

        abstract public bool IsVariable(out Variable var);

        abstract public bool IsAddition(out Variable var, out int value);

        abstract public NormalizedExpression<Variable> PlusOne();

        abstract public NormalizedExpression<Variable> MinusOne();

        abstract public Expression Convert<Expression>(IExpressionEncoder<Variable, Expression> encoder);

        virtual public bool TryPrettyPrint<T>(IFactory<T> factory, out T result)
        {
            Contract.Requires(factory != null);

            result = default(T);
            return false;
        }

        [Pure]
        static public NormalizedExpression<Variable> For(int value)
        {
            Contract.Ensures(Contract.Result<NormalizedExpression<Variable>>() != null);

            return new ConstantExpression(value);
        }

        [Pure]
        static public NormalizedExpression<Variable> For(Variable var)
        {
            Contract.Ensures(Contract.Result<NormalizedExpression<Variable>>() != null);

            return new VariableExpression(var);
        }

        [Pure]
        static public NormalizedExpression<Variable> For(Variable var, int value)
        {
            Contract.Ensures(Contract.Result<NormalizedExpression<Variable>>() != null);

            return value == 0 ? For(var) : new Addition(var, value);
        }

        [Pure]
        static public bool TryConvertFrom<Expression>(
          Expression exp, IExpressionDecoder<Variable, Expression> decoder,
          out NormalizedExpression<Variable> result)
        {
            Contract.Requires(exp != null);

            if (decoder == null)
            {
                result = default(NormalizedExpression<Variable>);
                return false;
            }

            var reader = new TryConvertRead<Expression>(decoder);

            return reader.TryConvert(exp, out result);
        }

        [Pure]
        static public bool TryConvertFrom<Expression>(
          Expression exp, ExpressionManagerWithEncoder<Variable, Expression> expManager,
          out NormalizedExpression<Variable> result)
        {
            Contract.Requires(exp != null);
            Contract.Requires(expManager != null);

            if (TryConvertFrom(exp, expManager.Decoder, out result))
            {
                return true;
            }

            //if (expManager.Encoder != null)
            {
                Polynomial<Variable, Expression> normalized;
                if (Polynomial<Variable, Expression>.TryToPolynomialForm(exp, expManager.Decoder, out normalized))
                {
                    return TryConvertFrom(normalized.ToPureExpression(expManager.Encoder), expManager.Decoder, out result);
                }
            }

            result = default(NormalizedExpression<Variable>);
            return false;
        }

        internal class ConstantExpression
          : NormalizedExpression<Variable>
        {
            private readonly int value;

            public ConstantExpression(int value)
            {
                this.value = value;
            }

            public override bool IsAddition(out Variable var, out int value)
            {
                var = default(Variable);
                value = default(int);

                return false;
            }

            public override bool IsConstant(out int value)
            {
                value = this.value;

                return true;
            }

            public override bool IsVariable(out Variable var)
            {
                var = default(Variable);

                return false;
            }

            public override NormalizedExpression<Variable> PlusOne()
            {
                return new ConstantExpression(value + 1);
            }

            public override NormalizedExpression<Variable> MinusOne()
            {
                return new ConstantExpression(value - 1);
            }

            override public bool TryPrettyPrint<T>(IFactory<T> factory, out T result)
            {
                result = factory.Constant(value);
                return true;
            }

            public override Expression Convert<Expression>(IExpressionEncoder<Variable, Expression> encoder)
            {
                return encoder.ConstantFor(value);
            }

            public override string ToString()
            {
                return value.ToString();
            }

            public override int GetHashCode()
            {
                return value;
            }

            public override bool Equals(object obj)
            {
                var that = obj as ConstantExpression;

                if (that == null)
                    return false;

                return value == that.value;
            }
        }

        internal class VariableExpression
          : NormalizedExpression<Variable>
        {
            private readonly Variable var;

            public VariableExpression(Variable var)
            {
                this.var = var;
            }

            public override bool IsAddition(out Variable var, out int value)
            {
                var = default(Variable);
                value = default(int);

                return false;
            }

            public override bool IsVariable(out Variable var)
            {
                var = this.var;

                return true;
            }

            public override bool IsConstant(out int value)
            {
                value = default(int);

                return false;
            }

            public override NormalizedExpression<Variable> PlusOne()
            {
                return new Addition(var, 1);
            }

            public override NormalizedExpression<Variable> MinusOne()
            {
                return new Addition(var, -1);
            }

            override public bool TryPrettyPrint<T>(IFactory<T> factory, out T result)
            {
                result = factory.Variable(var);
                return result != null;
            }

            public override Expression Convert<Expression>(IExpressionEncoder<Variable, Expression> encoder)
            {
                return encoder.VariableFor(var);
            }

            public override string ToString()
            {
                return var.ToString();
            }

            public override int GetHashCode()
            {
                return var.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var that = obj as VariableExpression;

                if (that == null)
                    return false;

                return var.Equals(that.var);
            }
        }

        internal class Addition
          : NormalizedExpression<Variable>
        {
            private readonly Variable var;
            private readonly int value;

            public Addition(Variable var, int value)
            {
                this.var = var;
                this.value = value;
            }

            public override bool IsAddition(out Variable var, out int value)
            {
                var = this.var;
                value = this.value;

                return true;
            }

            public override bool IsConstant(out int value)
            {
                value = default(int);

                return false;
            }

            public override bool IsVariable(out Variable var)
            {
                var = default(Variable);

                return false;
            }

            public override NormalizedExpression<Variable> PlusOne()
            {
                return For(var, value + 1);
            }

            public override NormalizedExpression<Variable> MinusOne()
            {
                return For(var, value - 1);
            }

            override public bool TryPrettyPrint<T>(IFactory<T> factory, out T result)
            {
                result = factory.Add(factory.Variable(var), factory.Constant(value));

                return true;
            }


            public override Expression Convert<Expression>(IExpressionEncoder<Variable, Expression> encoder)
            {
                return encoder.CompoundExpressionFor(ExpressionType.Int32,
                  ExpressionOperator.Addition,
                  encoder.VariableFor(var),
                  encoder.ConstantFor(value));
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", var.ToString(), value > 0 ? "+" : "", value.ToString());
            }

            public override int GetHashCode()
            {
                return value + var.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var that = obj as Addition;

                if (that == null)
                    return false;

                return value == that.value && that.var.Equals(var);
            }
        }

        private class TryConvertRead<Expression>
          : GenericExpressionVisitor<Void, Boolean, Variable, Expression>
        {
            private NormalizedExpression<Variable> result;

            public TryConvertRead(IExpressionDecoder<Variable, Expression> decoder)
              : base(decoder)
            {
                Contract.Requires(decoder != null);

                result = default(NormalizedExpression<Variable>);
            }

            //[SuppressMessage("Microsoft.Contracts", "Ensures-22-42", Justification="Depends on overriding")]
            public sealed override bool Visit(Expression exp, Void data)
            {
                Contract.Ensures(!Contract.Result<bool>() || result != null);

                result = default(NormalizedExpression<Variable>);
                return base.Visit(exp, data);
            }

            public bool TryConvert(Expression exp, out NormalizedExpression<Variable> result)
            {
                Contract.Requires(exp != null);
                Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out result) != null);
                Contract.Ensures(this.result == Contract.ValueAtReturn(out result));

                if (this.Visit(exp, Void.Value))
                {
                    Contract.Assert(this.result != null);
                    result = this.result;
                    return true;
                }

                this.result = result = default(NormalizedExpression<Variable>);
                return false;
            }

            public override bool VisitAddition(Expression left, Expression right, Expression original, Void data)
            {
                NormalizedExpression<Variable> leftNorm, rightNorm;
                if (this.TryConvert(left, out leftNorm))
                {
                    Contract.Assert(leftNorm != null);

                    if (this.TryConvert(right, out rightNorm))
                    {
                        Contract.Assert(rightNorm != null);

                        // All the cases. We declare all the variables here, 
                        // so to leverage the definite assignment analysis of 
                        // the compiler to check we are doing everything right
                        int leftConst, rightCont;
                        Variable leftVar, rightVar;
                        if (leftNorm.IsConstant(out leftConst))
                        {
                            // K + K
                            if (rightNorm.IsConstant(out rightCont))
                            {
                                result = NormalizedExpression<Variable>.For(leftConst + rightCont);  // Can overflow, we do not care has it may be the concrete semantics
                                return true;
                            }
                            // K + V
                            if (rightNorm.IsVariable(out rightVar))
                            {
                                result = NormalizedExpression<Variable>.For(rightVar, leftConst);
                                return true;
                            }
                        }
                        else if (leftNorm.IsVariable(out leftVar))
                        {
                            // V + K
                            if (rightNorm.IsConstant(out rightCont))
                            {
                                result = NormalizedExpression<Variable>.For(leftVar, rightCont);
                                return true;
                            }
                        }
                    }
                }

                return Default(data);
            }

            public override bool VisitConstant(Expression left, Void data)
            {
                Int32 value;
                if (this.Decoder.TryValueOf<Int32>(left, ExpressionType.Int32, out value))
                {
                    result = NormalizedExpression<Variable>.For(value);
                    return true;
                }

                return false;
            }

            public override bool VisitConvertToInt32(Expression left, Expression original, Void data)
            {
                return this.Visit(left, data);
            }

            public override bool VisitSubtraction(Expression left, Expression right, Expression original, Void data)
            {
                NormalizedExpression<Variable> leftNorm, rightNorm;
                if (this.TryConvert(left, out leftNorm))
                {
                    Contract.Assert(leftNorm != null);

                    if (this.TryConvert(right, out rightNorm))
                    {
                        Contract.Assert(rightNorm != null);

                        // All the cases. We declare all the variables here, 
                        // so to leverage the definite assignment analysis of 
                        // the compiler to check we are doing everything right
                        int leftConst, rightConst;
                        Variable leftVar;
                        if (rightNorm.IsConstant(out rightConst) && rightConst != Int32.MinValue)
                        {
                            // K - K
                            if (leftNorm.IsConstant(out leftConst))
                            {
                                result = NormalizedExpression<Variable>.For(leftConst - rightConst);  // Can overflow, we do not care has it may be the concrete semantics
                                return true;
                            }
                            // V - K
                            if (leftNorm.IsVariable(out leftVar))
                            {
                                result = NormalizedExpression<Variable>.For(leftVar, -rightConst);
                                return true;
                            }
                        }
                    }
                }

                return Default(data);
            }

            public override bool VisitVariable(Variable variable, Expression original, Void data)
            {
                result = NormalizedExpression<Variable>.For(variable);
                return true;
            }

            protected override bool Default(Void data)
            {
                result = default(NormalizedExpression<Variable>);
                return false;
            }

            public override string ToString()
            {
                return result == null ? "null" : result.ToString();
            }
        }
    }

    [ContractClassFor(typeof(NormalizedExpression<>))]
    internal abstract class NormalizedExpressionContracts<Variable>
      : NormalizedExpression<Variable>
    {
        public override NormalizedExpression<Variable> PlusOne()
        {
            Contract.Ensures(Contract.Result<NormalizedExpression<Variable>>() != null);

            return default(NormalizedExpression<Variable>);
        }

        public override NormalizedExpression<Variable> MinusOne()
        {
            Contract.Ensures(Contract.Result<NormalizedExpression<Variable>>() != null);

            return default(NormalizedExpression<Variable>);
        }

        public override Expression Convert<Expression>(IExpressionEncoder<Variable, Expression> encoder)
        {
            Contract.Requires(encoder != null);

            return default(Expression);
        }

        abstract public override bool IsConstant(out int value);

        abstract public override bool IsVariable(out Variable var);

        abstract public override bool IsAddition(out Variable var, out int value);
    }
}
