// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Microsoft.Research.CodeAnalysis.Expressions;

namespace Microsoft.Research.CodeAnalysis
{
    [ContractVerification(true)]
    public class ContractInferenceManager
    {
        #region Object Invariant

        [ContractInvariantMethod]
        private void ObjectInvariantMethod()
        {
            Contract.Invariant(this.PreCondition != null);
            Contract.Invariant(this.Assumptions != null);
            Contract.Invariant(this.OverriddenMethodPreconditionDispatcher != null);
            Contract.Invariant(this.Output != null);
        }

        #endregion

        #region State

        readonly public bool CanAddPreconditions;
        readonly public PreconditionInferenceManager PreCondition;
        readonly public IObjectInvariantDispatcher ObjectInvariant;
        readonly public IPostconditionDispatcher PostCondition;
        readonly public IAssumeDispatcher Assumptions;
        readonly public ICodeFixesManager CodeFixesManager;
        readonly public IOverriddenMethodPreconditionsDispatcher OverriddenMethodPreconditionDispatcher;

        readonly public IOutput Output;

        #endregion

        #region Constructor

        public ContractInferenceManager(bool CanAddPreconditions, IOverriddenMethodPreconditionsDispatcher overriddenMethodPreconditionsDispatcher, PreconditionInferenceManager precondition, IObjectInvariantDispatcher invariants, IPostconditionDispatcher postcondition, IAssumeDispatcher assumptions, ICodeFixesManager codefixesManager, IOutput output)
        {
            Contract.Requires(overriddenMethodPreconditionsDispatcher != null);
            Contract.Requires(precondition != null);
            Contract.Requires(invariants != null);
            Contract.Requires(postcondition != null);
            Contract.Requires(assumptions != null);
            Contract.Requires(codefixesManager != null);
            Contract.Requires(output != null);

            this.CanAddPreconditions = CanAddPreconditions;
            this.OverriddenMethodPreconditionDispatcher = overriddenMethodPreconditionsDispatcher;
            this.PreCondition = precondition;
            this.ObjectInvariant = invariants;
            this.PostCondition = postcondition;
            this.Assumptions = assumptions;
            this.CodeFixesManager = codefixesManager;
            this.Output = output;
        }

        #endregion

        #region AddPreconditionOrAssume

        public ProofOutcome AddPreconditionOrAssume(ProofObligation obl, IEnumerable<BoxedExpression> conditions, ProofOutcome outcome = ProofOutcome.Top)
        {
            Contract.Requires(conditions != null);
            Contract.Requires(obl != null);

            if (this.CanAddPreconditions)
            {
                Contract.Assume(this.PreCondition.Dispatch != null);
                return this.PreCondition.Dispatch.AddPreconditions(obl, conditions, outcome);
            }
            else
            {
                this.Assumptions.AddEntryAssumes(obl, conditions);
                this.OverriddenMethodPreconditionDispatcher.AddPotentialPreconditions(obl, conditions);

                return outcome;
            }
        }

        #endregion

        #region Questions

        public bool SuggestInferredRequires
        {
            get
            {
                return this.Output.LogOptions.SuggestRequires;
            }
        }

        public bool SuggestInferredEnsures(bool isProperty)
        {
            return this.Output.LogOptions.SuggestEnsures(isProperty);
        }

        public bool PropagateInferredRequires(bool isProperty)
        {
            return this.Output.LogOptions.PropagateInferredRequires(isProperty);
        }

        public bool PropagateInferredEnsures(bool isProperty)
        {
            return this.Output.LogOptions.PropagateInferredEnsures(isProperty);
        }

        public bool InferPreconditionsFromPostconditions
        {
            get
            {
                return this.Output.LogOptions.InferPreconditionsFromPostconditions;
            }
        }

        public bool CheckFalsePostconditions
        {
            get
            {
                return this.Output.LogOptions.CheckFalsePostconditions;
            }
        }

        #endregion
    }
}
