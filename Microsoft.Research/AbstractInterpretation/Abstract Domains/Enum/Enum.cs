// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Research.DataStructures;
using System.Diagnostics.Contracts;
using System;

namespace Microsoft.Research.AbstractDomains
{
    [ContractVerification(true)]
    public class EnumDefined<Variable, Type, Expression>
      : IAbstractDomainWithRenaming<EnumDefined<Variable, Type, Expression>, Variable>,
      IAbstractDomainForEnvironments<Variable, Expression>
    {
        #region Object invariant
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(conditions != null);
            Contract.Invariant(defined != null);
        }

        #endregion

        #region State

        private enum State { Bottom, Normal, Top }

        readonly private SimpleImmutableFunctional<Variable, SetOfConstraints<Pair<Type, Variable>>> conditions;     // map var1 <-> <type, var2> with the meaning that var1 is true iff var2 is defined for enum type
        readonly private SetOfConstraints<Pair<Type, Variable>> defined;            // sets of <var, type> with the meaning that var is defined for type 
        readonly private State state;

        #endregion

        #region Constructors

        private EnumDefined(
          SimpleImmutableFunctional<Variable, SetOfConstraints<Pair<Type, Variable>>> conditions,
          SetOfConstraints<Pair<Type, Variable>> defined)
        {
            Contract.Requires(conditions != null);
            Contract.Requires(defined != null);

            this.conditions = conditions;
            this.defined = defined;

            if (conditions.IsBottom || defined.IsBottom)
            {
                state = State.Bottom;
            }
            else if (conditions.IsTop && defined.IsTop)
            {
                state = State.Top;
            }
            else
            {
                state = State.Normal;
            }
        }

        private EnumDefined(Dictionary<Variable, SetOfConstraints<Pair<Type, Variable>>> conditions, Set<Pair<Type, Variable>> defined)
          : this(
          new SimpleImmutableFunctional<Variable, SetOfConstraints<Pair<Type, Variable>>>(conditions),
          new SetOfConstraints<Pair<Type, Variable>>(defined, false)
          )
        {
            Contract.Requires(conditions != null);
            Contract.Requires(defined != null);
        }

        private EnumDefined(State state)
          : this
          (
          SimpleImmutableFunctional<Variable, SetOfConstraints<Pair<Type, Variable>>>.Unknown,
          SetOfConstraints<Pair<Type, Variable>>.Unknown
          )
        {
            Contract.Requires(state != State.Normal);

            this.state = state;
        }
        #endregion

        #region Assumptions

        [Pure]
        public EnumDefined<Variable, Type, Expression> AssumeTypeIff(Variable condition, Type type, Variable variable)
        {
            Contract.Ensures(Contract.Result<EnumDefined<Variable, Type, Expression>>() != null);

            SetOfConstraints<Pair<Type, Variable>> constraints;
            if (conditions.TryGetValue(condition, out constraints))
            {
                constraints = constraints.Add(new Pair<Type, Variable>(type, variable));
            }
            else
            {
                constraints = new SetOfConstraints<Pair<Type, Variable>>(new Pair<Type, Variable>(type, variable));
            }

            var newConditions = conditions.Add(condition, constraints);

            return new EnumDefined<Variable, Type, Expression>(newConditions, defined);
        }

        [Pure]
        public EnumDefined<Variable, Type, Expression> AssumeCondition(Variable condition)
        {
            Contract.Ensures(Contract.Result<EnumDefined<Variable, Type, Expression>>() != null);

            SetOfConstraints<Pair<Type, Variable>> constraints;
            if (conditions.TryGetValue(condition, out constraints))
            {
                var newDefinitions = defined;
                foreach (var pair in constraints.Values)
                {
                    newDefinitions = newDefinitions.Add(pair);
                }

                return new EnumDefined<Variable, Type, Expression>(conditions, newDefinitions);
            }
            else
            {
                return this;
            }
        }

        [Pure]
        public EnumDefined<Variable, Type, Expression> AssumeCondition(Type type, Variable var)
        {
            Contract.Ensures(Contract.Result<EnumDefined<Variable, Type, Expression>>() != null);

            var newDefinitions = defined.Add(new Pair<Type, Variable>(type, var));

            return new EnumDefined<Variable, Type, Expression>(conditions, newDefinitions);
        }

        #endregion

        #region Statics
        public static EnumDefined<Variable, Type, Expression> Unknown
        {
            get
            {
                return new EnumDefined<Variable, Type, Expression>(State.Top);
            }
        }
        #endregion

        #region IAbstractDomainWithRenaming<Enum<Variable, Expression>,Variable> Members

        [ContractVerification(false)]
        public EnumDefined<Variable, Type, Expression> Rename(Dictionary<Variable, FList<Variable>> renaming)
        {
            if (!this.IsNormal())
            {
                return this;
            }
            var newConditions = new Dictionary<Variable, SetOfConstraints<Pair<Type, Variable>>>();

            foreach (var pair in conditions.Elements)
            {
                FList<Variable> newNames;
                if (renaming.TryGetValue(pair.Key, out newNames))
                {
                    var newPairs = new Set<Pair<Type, Variable>>();
                    Contract.Assume(pair.Value != null);

                    foreach (var equalities in pair.Value.Values)
                    {
                        FList<Variable> newRight;
                        if (renaming.TryGetValue(equalities.Two, out newRight))
                        {
                            foreach (var y in newRight.GetEnumerable())
                            {
                                newPairs.Add(new Pair<Type, Variable>(equalities.One, y));
                            }
                        }
                    }

                    foreach (var newName in newNames.GetEnumerable())
                    {
                        newConditions[newName] = new SetOfConstraints<Pair<Type, Variable>>(newPairs);
                    }
                }
            }

            var newDefinitions = new Set<Pair<Type, Variable>>();

            foreach (var equalities in defined.Values)
            {
                FList<Variable> newRight;
                if (renaming.TryGetValue(equalities.Two, out newRight))
                {
                    foreach (var y in newRight.GetEnumerable())
                    {
                        newDefinitions.Add(new Pair<Type, Variable>(equalities.One, y));
                    }
                }
            }

            return new EnumDefined<Variable, Type, Expression>(newConditions, newDefinitions);
        }

        #endregion

        #region Abstract Domain operations

        public bool IsBottom
        {
            get { return state == State.Bottom; }
        }

        public bool IsTop
        {
            get { return state == State.Top; }
        }

        public EnumDefined<Variable, Type, Expression> Bottom
        {
            get
            {
                Contract.Ensures(Contract.Result<EnumDefined<Variable, Type, Expression>>() != null);

                return new EnumDefined<Variable, Type, Expression>(State.Bottom);
            }
        }

        public EnumDefined<Variable, Type, Expression> Top
        {
            get
            {
                Contract.Ensures(Contract.Result<EnumDefined<Variable, Type, Expression>>() != null);

                return new EnumDefined<Variable, Type, Expression>(State.Top);
            }
        }

        public bool LessEqual(EnumDefined<Variable, Type, Expression> other)
        {
            Contract.Requires(other != null);

            bool direct;
            if (AbstractDomainsHelper.TryTrivialLessEqual(this, other, out direct))
            {
                return direct;
            }

            Contract.Assume(other.conditions != null);
            Contract.Assume(other.defined != null);

            return conditions.LessEqual(other.conditions) && defined.LessEqual(other.defined);
        }

        public EnumDefined<Variable, Type, Expression> Join(EnumDefined<Variable, Type, Expression> other)
        {
            Contract.Requires(other != null);
            Contract.Ensures(Contract.Result<EnumDefined<Variable, Type, Expression>>() != null);

            EnumDefined<Variable, Type, Expression> result;
            if (AbstractDomainsHelper.TryTrivialJoin(this, other, out result))
            {
                return result;
            }

            Contract.Assume(other.conditions != null);
            Contract.Assume(other.defined != null);

            return new EnumDefined<Variable, Type, Expression>(conditions.Join(other.conditions), defined.Join(other.defined));
        }

        public EnumDefined<Variable, Type, Expression> Meet(EnumDefined<Variable, Type, Expression> other)
        {
            Contract.Requires(other != null);
            Contract.Ensures(Contract.Result<EnumDefined<Variable, Type, Expression>>() != null);

            EnumDefined<Variable, Type, Expression> result;
            if (AbstractDomainsHelper.TryTrivialMeet(this, other, out result))
            {
                return result;
            }

            Contract.Assume(other.conditions != null);
            Contract.Assume(other.defined != null);

            return new EnumDefined<Variable, Type, Expression>(conditions.Meet(other.conditions), defined.Meet(other.defined));
        }


        public EnumDefined<Variable, Type, Expression> Widening(EnumDefined<Variable, Type, Expression> other)
        {
            Contract.Requires(other != null);
            Contract.Ensures(Contract.Result<EnumDefined<Variable, Type, Expression>>() != null);

            return this.Join(other);
        }
        #endregion

        #region Implementation of the interfaces
        bool IAbstractDomain.LessEqual(IAbstractDomain a)
        {
            var other = a as EnumDefined<Variable, Type, Expression>;
            Contract.Assume(other != null);
            return this.LessEqual(other);
        }

        IAbstractDomain IAbstractDomain.Bottom
        {
            get { return this.Bottom; }
        }

        IAbstractDomain IAbstractDomain.Top
        {
            get { return this.Top; }
        }

        IAbstractDomain IAbstractDomain.Join(IAbstractDomain a)
        {
            var other = a as EnumDefined<Variable, Type, Expression>;
            Contract.Assume(other != null);
            return this.Join(other);
        }

        IAbstractDomain IAbstractDomain.Meet(IAbstractDomain a)
        {
            var other = a as EnumDefined<Variable, Type, Expression>;
            Contract.Assume(other != null);
            return this.Meet(other);
        }

        IAbstractDomain IAbstractDomain.Widening(IAbstractDomain prev)
        {
            var other = prev as EnumDefined<Variable, Type, Expression>;
            Contract.Assume(other != null);
            return this.Widening(other);
        }

        T IAbstractDomain.To<T>(IFactory<T> factory)
        {
            return factory.IdentityForAnd;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this;
        }

        #endregion

        #region IAbstractDomainForEnvironments<Variable,Expression> Members

        virtual public void AssumeDomainSpecificFact(DomainSpecificFact fact)
        {
        }


        public string ToString(Expression exp)
        {
            return "not implemented";
        }

        #endregion

        #region IPureExpressionAssignments<Variable,Expression> Members

        public List<Variable> Variables
        {
            get
            {
                var result = new List<Variable>();

                foreach (var pair in conditions.Elements)
                {
                    result.Add(pair.Key);
                    Contract.Assume(pair.Value != null);
                    foreach (var p in pair.Value.Values)
                    {
                        result.Add(p.Two);
                    }
                }

                foreach (var pair in defined.Values)
                {
                    result.Add(pair.Two);
                }

                return result;
            }
        }

        public void AddVariable(Variable var)
        {
            // does nothing
        }

        public void Assign(Expression x, Expression exp)
        {
            // does nothing
        }

        public void ProjectVariable(Variable var)
        {
            // does nothing
        }

        public void RemoveVariable(Variable var)
        {
            // does nothing
        }

        public void RenameVariable(Variable OldName, Variable NewName)
        {
            // does nothing
        }

        #endregion

        #region IPureExpressionTest<Variable,Expression> Members

        public IAbstractDomainForEnvironments<Variable, Expression> TestTrue(Expression guard)
        {
            return this;
        }

        public IAbstractDomainForEnvironments<Variable, Expression> TestFalse(Expression guard)
        {
            return this;
        }

        public FlatAbstractDomain<bool> CheckIfHolds(Expression exp)
        {
            return CheckOutcome.Top;
        }

        #endregion

        #region IAssignInParallel<Variable,Expression> Members

        public void AssignInParallel(Dictionary<Variable, FList<Variable>> sourcesToTargets, Converter<Variable, Expression> convert)
        {
            // We should not invoke it
            throw new Exception();
        }

        #endregion

        #region ToString
        public override string ToString()
        {
            if (this.IsBottom)
                return "_|_";
            if (this.IsTop)
                return "Top";
            else
                return string.Format("({0}\n {1})", conditions.ToString(), defined.ToString());
        }
        #endregion

        #region Checking
        public FlatAbstractDomain<bool> CheckIfVariableIsDefined(Variable var, Type type)
        {
            if (this.IsBottom)
            {
                return CheckOutcome.Bottom;
            }

            if (this.IsTop)
            {
                return CheckOutcome.Top;
            }

            var goal = new Pair<Type, Variable>(type, var);

            foreach (var pair in defined.Values)
            {
                if (goal.Equals(pair))
                {
                    return CheckOutcome.True;
                }
            }

            return CheckOutcome.Top;
        }
        #endregion

        #region Inspect state

        public IEnumerable<Pair<Type, Variable>> DefinedVariables
        {
            get
            {
                Contract.Requires(this.IsNormal());
                Contract.Ensures(Contract.Result<IEnumerable<Pair<Type, Variable>>>() != null);

                return defined.Values;
            }
        }

        #endregion
    }
}
