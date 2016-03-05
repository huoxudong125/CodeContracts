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

// File System.Web.UI.ExpressionBindingCollection.cs
// Automatically generated contract file.
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics.Contracts;
using System;

// Disable the "this variable is not used" warning as every field would imply it.
#pragma warning disable 0414
// Disable the "this variable is never assigned to".
#pragma warning disable 0649
// Disable the "this variable is never used".
#pragma warning disable 0169
// Disable the "new keyword not required" warning.
#pragma warning disable 0109
// Disable the "extern without DllImport" warning.
#pragma warning disable 0626
// Disable the "could hide other member" warning, can happen on certain properties.
#pragma warning disable 0108


namespace System.Web.UI
{
  sealed public partial class ExpressionBindingCollection : System.Collections.ICollection, System.Collections.IEnumerable
  {
    #region Methods and constructors
    public void Add (ExpressionBinding binding)
    {
      Contract.Requires (binding != null);
    }

    public void Clear ()
    {
    }

    public bool Contains (string propName)
    {
      return default(bool);
    }

    public void CopyTo (ExpressionBinding[] array, int index)
    {
      Contract.Requires (array != null);
    }

    public void CopyTo (Array array, int index)
    {
    }

    public ExpressionBindingCollection ()
    {
    }

    public System.Collections.IEnumerator GetEnumerator ()
    {
      return default(System.Collections.IEnumerator);
    }

    public void Remove (string propertyName)
    {
    }

    public void Remove (string propertyName, bool addToRemovedList)
    {
    }

    public void Remove (ExpressionBinding binding)
    {
      Contract.Requires (binding != null);
    }
    #endregion

    #region Properties and indexers
    public int Count
    {
      get
      {
        return default(int);
      }
    }

    public bool IsReadOnly
    {
      get
      {
        Contract.Ensures (Contract.Result<bool>() == false);

        return default(bool);
      }
    }

    public bool IsSynchronized
    {
      get
      {
        return default(bool);
      }
    }

    public ExpressionBinding this [string propertyName]
    {
      get
      {
        return default(ExpressionBinding);
      }
    }

    public System.Collections.ICollection RemovedBindings
    {
      get
      {
        Contract.Ensures (Contract.Result<System.Collections.ICollection>() != null);

        return default(System.Collections.ICollection);
      }
    }

    public Object SyncRoot
    {
      get
      {
        return default(Object);
      }
    }
    #endregion
  }
}
