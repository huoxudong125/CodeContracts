// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.DataStructures;
using System.Diagnostics;
using System.IO;
using System.Diagnostics.Contracts;

namespace Microsoft.Research.CodeAnalysis.ILCodeProvider
{
    internal enum Instruction
    {
        Pop,
    }

    internal static class Predefined
    {
        public static Subroutine OldEvalPopSubroutine<Local, Parameter, Type, Method, Field, Property, Event, Attribute, Assembly>(
          MethodCache<Local, Parameter, Type, Method, Field, Property, Event, Attribute, Assembly> methodCache,
          Subroutine oldEval
        )
        {
            Contract.Requires(oldEval != null);// F: Added as of Clousot suggestion
            Contract.Requires(oldEval.StackDelta == 1); // F: Added as of Clousot suggestion
            Contract.Requires(methodCache != null);// F: As of Clousot suggestion
            Debug.Assert(oldEval.StackDelta == 1);
            var sub = new SimpleILCodeProvider<Local, Parameter, Type, Method, Field, Property, Event, Attribute, Assembly>(
              new Instruction[] { Instruction.Pop },
              0).GetSubroutine(methodCache);

            sub.AddEdgeSubroutine(sub.Entry, sub.EntryAfterRequires, oldEval, "oldmanifest");
            return sub;
        }
    }

    internal class SimpleILCodeProvider<Local, Parameter, Type, Method, Field, Property, Event, Attribute, Assembly>
      : ICodeProvider<int, Local, Parameter, Method, Field, Type>
      , IDecodeMSIL<int, Local, Parameter, Method, Field, Type, Unit, Unit, Unit, Unit>
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(instructions != null); // F: made an object invariant because of the many precondition suggestions
        }


        private Instruction[] instructions;
        private int stackDelta;

        /// <summary>
        /// Build a code provider with the given instruction sequence
        /// </summary>
        /// <param name="instructions"></param>
        /// <param name="stackDelta">The number of values on the stack left by this instruction sequence (can be negative)</param>
        public SimpleILCodeProvider(Instruction[] instructions, int stackDelta)
        {
            Contract.Requires(instructions != null); // F: Clousot suggestion

            this.instructions = instructions;
            this.stackDelta = stackDelta;
        }

        public Subroutine GetSubroutine(MethodCache<Local, Parameter, Type, Method, Field, Property, Event, Attribute, Assembly> methodCache)
        {
            Contract.Requires(methodCache != null); // F: Added as of Clousot suggestion
            return methodCache.BuildSubroutine(stackDelta, this, 0);
        }

        #region ICodeProvider<int,Method,Type> Members

        public R Decode<Visitor, T, R>(int label, Visitor query, T data)
          where Visitor : ICodeQuery<int, Local, Parameter, Method, Field, Type, T, R>
        {
            switch (instructions[label])
            {
                case Instruction.Pop:
                    return query.Pop(label, Unit.Value, data);

                default:
                    throw new NotImplementedException("unknown instruction");
            }
        }

        public bool Next(int current, out int nextLabel)
        {
            current++;
            if (current < instructions.Length)
            {
                nextLabel = current;
                return true;
            }
            nextLabel = 0;
            return false;
        }

        public void PrintCodeAt(int pc, string indent, System.IO.TextWriter tw)
        {
            Contract.Requires(tw != null);// F: Added as of Clousot suggestion
            tw.Write("{0}{1}", indent, instructions[pc].ToString());
        }

        public string SourceAssertionCondition(int pc)
        {
            return null;
        }

        public bool HasSourceContext(int pc)
        {
            return false;
        }

        public string SourceDocument(int pc)
        {
            return null;
        }

        public int SourceStartLine(int pc)
        {
            return 0;
        }

        public int SourceEndLine(int pc)
        {
            return 0;
        }

        public int SourceStartColumn(int pc)
        {
            return 0;
        }

        public int SourceEndColumn(int pc)
        {
            return 0;
        }

        public int SourceStartIndex(int pc)
        {
            return 0;
        }

        public int SourceLength(int pc)
        {
            return 0;
        }

        public int ILOffset(int pc)
        {
            return 0;
        }

        #endregion


        #region IDecodeMSIL<int,Local,Parameter,Method,Field,Type,Unit,Unit,Unit> Members

        public Result ForwardDecode<Data, Result, Visitor>(int lab, Visitor visitor, Data data) where Visitor : IVisitMSIL<int, Local, Parameter, Method, Field, Type, Unit, Unit, Data, Result>
        {
            switch (instructions[lab])
            {
                case Instruction.Pop:
                    return visitor.Pop(lab, Unit.Value, data);

                default:
                    throw new NotImplementedException("unknown instruction");
            }
        }

        public bool IsUnreachable(int lab)
        {
            return lab < 0 || lab > instructions.Length;
        }

        public Unit Context
        {
            get { return Unit.Value; }
        }

        #endregion

        #region IDecodeMSIL<int,Local,Parameter,Method,Field,Type,Unit,Unit,Unit,Unit> Members


        public Unit EdgeData(int from, int to)
        {
            return Unit.Value;
        }

        public void Display(TextWriter tw, string prefix, Unit data) { }

        #endregion
    }
}
