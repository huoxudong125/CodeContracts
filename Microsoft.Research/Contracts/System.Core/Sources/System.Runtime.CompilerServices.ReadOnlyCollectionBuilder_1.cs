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

// File System.Runtime.CompilerServices.ReadOnlyCollectionBuilder_1.cs
// Automatically generated contract file.
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics.Contracts;
using System;

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


namespace System.Runtime.CompilerServices
{
  sealed public partial class ReadOnlyCollectionBuilder<T> : IList<T>, ICollection<T>, IEnumerable<T>, System.Collections.IList, System.Collections.ICollection, System.Collections.IEnumerable
  {
    #region Methods and constructors
    public void Add(T item)
    {
    }

    public void Clear()
    {
    }

    public bool Contains(T item)
    {
      return default(bool);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
    }

    public IEnumerator<T> GetEnumerator()
    {
      return default(IEnumerator<T>);
    }

    public int IndexOf(T item)
    {
      return default(int);
    }

    public void Insert(int index, T item)
    {
    }

    public ReadOnlyCollectionBuilder(int capacity)
    {
      Contract.Requires(0 <= capacity);
    }

    public ReadOnlyCollectionBuilder(IEnumerable<T> collection)
    {
    }

    public ReadOnlyCollectionBuilder()
    {
    }

    public bool Remove(T item)
    {
      return default(bool);
    }

    public void RemoveAt(int index)
    {
    }

    public void Reverse()
    {
    }

    public void Reverse(int index, int count)
    {
      Contract.Requires(count >= 0);
      Contract.Requires(index >= 0);
    }

    void System.Collections.ICollection.CopyTo(Array array, int index)
    {
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return default(System.Collections.IEnumerator);
    }

    int System.Collections.IList.Add(Object value)
    {
      return default(int);
    }

    bool System.Collections.IList.Contains(Object value)
    {
      return default(bool);
    }

    int System.Collections.IList.IndexOf(Object value)
    {
      return default(int);
    }

    void System.Collections.IList.Insert(int index, Object value)
    {
    }

    void System.Collections.IList.Remove(Object value)
    {
    }

    public T[] ToArray()
    {
      Contract.Ensures(Contract.Result<T[]>() != null);

      return default(T[]);
    }

    public System.Collections.ObjectModel.ReadOnlyCollection<T> ToReadOnlyCollection()
    {
      Contract.Ensures(Contract.Result<System.Collections.ObjectModel.ReadOnlyCollection<T>>() != null);

      return default(System.Collections.ObjectModel.ReadOnlyCollection<T>);
    }
    #endregion

    #region Properties and indexers
    public int Capacity
    {
      get
      {
        Contract.Ensures(0 <= Contract.Result<int>());
        Contract.Ensures(Contract.Result<int>() <= 2147483647);

        return default(int);
      }
      set
      {
      }
    }

    public int Count
    {
      get
      {
        return default(int);
      }
    }

    public T this [int index]
    {
      get
      {
        return default(T);
      }
      set
      {
      }
    }

    bool System.Collections.Generic.ICollection<T>.IsReadOnly
    {
      get
      {
        return default(bool);
      }
    }

    bool System.Collections.ICollection.IsSynchronized
    {
      get
      {
        return default(bool);
      }
    }

    Object System.Collections.ICollection.SyncRoot
    {
      get
      {
        return default(Object);
      }
    }

    bool System.Collections.IList.IsFixedSize
    {
      get
      {
        return default(bool);
      }
    }

    bool System.Collections.IList.IsReadOnly
    {
      get
      {
        return default(bool);
      }
    }

    Object System.Collections.IList.this [int index]
    {
      get
      {
        return default(Object);
      }
      set
      {
      }
    }
    #endregion
  }
}
