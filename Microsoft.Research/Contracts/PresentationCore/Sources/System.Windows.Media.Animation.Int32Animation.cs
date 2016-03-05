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

// File System.Windows.Media.Animation.Int32Animation.cs
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


namespace System.Windows.Media.Animation
{
  public partial class Int32Animation : Int32AnimationBase
  {
    #region Methods and constructors
    public System.Windows.Media.Animation.Int32Animation Clone()
    {
      return default(System.Windows.Media.Animation.Int32Animation);
    }

    protected override System.Windows.Freezable CreateInstanceCore()
    {
      return default(System.Windows.Freezable);
    }

    protected override int GetCurrentValueCore(int defaultOriginValue, int defaultDestinationValue, AnimationClock animationClock)
    {
      return default(int);
    }

    public Int32Animation()
    {
    }

    public Int32Animation(int toValue, System.Windows.Duration duration)
    {
    }

    public Int32Animation(int toValue, System.Windows.Duration duration, FillBehavior fillBehavior)
    {
    }

    public Int32Animation(int fromValue, int toValue, System.Windows.Duration duration, FillBehavior fillBehavior)
    {
    }

    public Int32Animation(int fromValue, int toValue, System.Windows.Duration duration)
    {
    }
    #endregion

    #region Properties and indexers
    public Nullable<int> By
    {
      get
      {
        return default(Nullable<int>);
      }
      set
      {
      }
    }

    public IEasingFunction EasingFunction
    {
      get
      {
        return default(IEasingFunction);
      }
      set
      {
      }
    }

    public Nullable<int> From
    {
      get
      {
        return default(Nullable<int>);
      }
      set
      {
      }
    }

    public bool IsAdditive
    {
      get
      {
        return default(bool);
      }
      set
      {
      }
    }

    public bool IsCumulative
    {
      get
      {
        return default(bool);
      }
      set
      {
      }
    }

    public Nullable<int> To
    {
      get
      {
        return default(Nullable<int>);
      }
      set
      {
      }
    }
    #endregion

    #region Fields
    public readonly static System.Windows.DependencyProperty ByProperty;
    public readonly static System.Windows.DependencyProperty EasingFunctionProperty;
    public readonly static System.Windows.DependencyProperty FromProperty;
    public readonly static System.Windows.DependencyProperty ToProperty;
    #endregion
  }
}
