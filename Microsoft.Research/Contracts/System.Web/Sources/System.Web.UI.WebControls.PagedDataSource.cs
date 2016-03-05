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

// File System.Web.UI.WebControls.PagedDataSource.cs
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


namespace System.Web.UI.WebControls
{
  sealed public partial class PagedDataSource : System.Collections.ICollection, System.Collections.IEnumerable, System.ComponentModel.ITypedList
  {
    #region Methods and constructors
    public void CopyTo(Array array, int index)
    {
    }

    public System.Collections.IEnumerator GetEnumerator()
    {
      return default(System.Collections.IEnumerator);
    }

    public System.ComponentModel.PropertyDescriptorCollection GetItemProperties(System.ComponentModel.PropertyDescriptor[] listAccessors)
    {
      return default(System.ComponentModel.PropertyDescriptorCollection);
    }

    public string GetListName(System.ComponentModel.PropertyDescriptor[] listAccessors)
    {
      return default(string);
    }

    public PagedDataSource()
    {
    }
    #endregion

    #region Properties and indexers
    public bool AllowCustomPaging
    {
      get
      {
        return default(bool);
      }
      set
      {
      }
    }

    public bool AllowPaging
    {
      get
      {
        return default(bool);
      }
      set
      {
      }
    }

    public bool AllowServerPaging
    {
      get
      {
        return default(bool);
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

    public int CurrentPageIndex
    {
      get
      {
        return default(int);
      }
      set
      {
      }
    }

    public System.Collections.IEnumerable DataSource
    {
      get
      {
        return default(System.Collections.IEnumerable);
      }
      set
      {
      }
    }

    public int DataSourceCount
    {
      get
      {
        return default(int);
      }
    }

    public int FirstIndexInPage
    {
      get
      {
        return default(int);
      }
    }

    public bool IsCustomPagingEnabled
    {
      get
      {
        return default(bool);
      }
    }

    public bool IsFirstPage
    {
      get
      {
        return default(bool);
      }
    }

    public bool IsLastPage
    {
      get
      {
        return default(bool);
      }
    }

    public bool IsPagingEnabled
    {
      get
      {
        return default(bool);
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return default(bool);
      }
    }

    public bool IsServerPagingEnabled
    {
      get
      {
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

    public int PageCount
    {
      get
      {
        return default(int);
      }
    }

    public int PageSize
    {
      get
      {
        return default(int);
      }
      set
      {
      }
    }

    public Object SyncRoot
    {
      get
      {
        return default(Object);
      }
    }

    public int VirtualCount
    {
      get
      {
        return default(int);
      }
      set
      {
      }
    }
    #endregion
  }
}
