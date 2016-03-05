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

#if NETFRAMEWORK_4_0 || SILVERLIGHT_4_0 || SILVERLIGHT_5_0
// File System.Linq.Expressions.DynamicExpression.cs
// Automatically generated contract file.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

// Disable the "this variable is not used" warning as every field would imply it.
#pragma warning disable 0414
// Disable the "this variable is never assigned to".
#pragma warning disable 0067
// Disable the "this event is never assigned to".
#pragma warning disable 0649
// Disable the "this variable is never used".
#pragma warning disable 0169
// Disable the "new keyword not required" warning.
#pragma warning disable 0109
// Disable the "extern without DllImport" warning.
#pragma warning disable 0626
// Disable the "could hide other member" warning, can happen on certain properties.
#pragma warning disable 0108


namespace System.Linq.Expressions
{
  public partial class DynamicExpression : Expression
  {
    #region Methods and constructors
    private DynamicExpression() {}

    protected internal override Expression Accept (ExpressionVisitor visitor)
    {
      return default(Expression);
    }

    [Pure]
    public DynamicExpression Update (IEnumerable<Expression> arguments)
    {
      Contract.Ensures(Contract.Result<DynamicExpression>() != null);
      return default(DynamicExpression);
    }
    #endregion

    #region Properties and indexers
    public ReadOnlyCollection<Expression> Arguments
    {
      get
      {
        Contract.Ensures(Contract.Result<ReadOnlyCollection<Expression>>() != null);
        return default(ReadOnlyCollection<Expression>);
      }
    }

    public CallSiteBinder Binder
    {
      get
      {
        Contract.Ensures(Contract.Result<CallSiteBinder>() != null);
        return default(CallSiteBinder);
      }
    }

#if NETFRAMEWORK_4_6
    public virtual Type DelegateType
#else
    public Type DelegateType
#endif
    {
      get
      {
        Contract.Ensures(Contract.Result<Type>() != null);
        return default(Type);
      }
    }

    public override sealed ExpressionType NodeType
    {
      get
      {
        return default(ExpressionType);
      }
    }

    public override Type Type
    {
      get
      {
        return default(Type);
      }
    }
    #endregion
  }
}
#endif