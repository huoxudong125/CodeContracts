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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using Microsoft.Research.ClousotRegression;

namespace ContractVerificationAttributeObservance
{
  [ContractVerification(false)]
  class GeneralTests
  {
    [ClousotRegressionTest] // ignored due to ContractVerification false
    string TestMeIgnored(string s)
    {
      return s.ToString(); // no warning, as it isn't analyzed
    }

    [ClousotRegressionTest("regular"), ContractVerification(true)]
    [RegressionOutcome(Outcome = ProofOutcome.Top, Message = "Possibly calling a method on a null reference \'s\'", PrimaryILOffset = 2, MethodILOffset = 0)]
    string TestMeNotIgnored(string s)
    {
      return s.ToString(); // warning
    }

    class NestedIgnored
    {
      [ClousotRegressionTest] // ignored due to ContractVerification false
      string TestMeIgnored(string s)
      {
        return s.ToString(); // no warning, as it isn't analyzed
      }

      [ClousotRegressionTest("regular"), ContractVerification(true)]
      [RegressionOutcome(Outcome = ProofOutcome.Top, Message = "Possibly calling a method on a null reference \'s\'", PrimaryILOffset = 2, MethodILOffset = 0)]
      string TestMeNotIgnored(string s)
      {
        return s.ToString(); // warning
      }
    }

    [ContractVerification(true)]
    class NestedNotIgnored
    {
      [ClousotRegressionTest("regular")]
      [RegressionOutcome(Outcome = ProofOutcome.Top, Message = "Possibly calling a method on a null reference \'s\'", PrimaryILOffset = 2, MethodILOffset = 0)]
      string TestMeNotIgnored(string s)
      {
        return s.ToString(); // warning
      }

      [ClousotRegressionTest, ContractVerification(false)] // ignored due to ContractVerification false
      string TestMeIgnored(string s)
      {
        return s.ToString(); // no warning, as it isn't analyzed
      }
    }

    [ContractVerification(true)]
    public int Prop {
        [ClousotRegressionTest("regular")]
        [ContractVerification(false)]
        get { 
          Contract.Ensures(Contract.Result<int>() > 0);
          return 0;
        }

        [ClousotRegressionTest("regular")]
        [RegressionOutcome(Outcome=ProofOutcome.True,Message="assert is valid",PrimaryILOffset=15,MethodILOffset=0)]
        set {
          Contract.Requires(value > 0);
          Contract.Assert(value > 0);
        }
    }


  }
}
