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

// File System.Windows.Media.CombinedGeometry.cs
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


namespace System.Windows.Media
{
  sealed public partial class CombinedGeometry : Geometry
  {
    #region Methods and constructors
    public System.Windows.Media.CombinedGeometry Clone()
    {
      return default(System.Windows.Media.CombinedGeometry);
    }

    public System.Windows.Media.CombinedGeometry CloneCurrentValue()
    {
      return default(System.Windows.Media.CombinedGeometry);
    }

    public CombinedGeometry()
    {
    }

    public CombinedGeometry(GeometryCombineMode geometryCombineMode, Geometry geometry1, Geometry geometry2, Transform transform)
    {
    }

    public CombinedGeometry(GeometryCombineMode geometryCombineMode, Geometry geometry1, Geometry geometry2)
    {
    }

    public CombinedGeometry(Geometry geometry1, Geometry geometry2)
    {
    }

    protected override System.Windows.Freezable CreateInstanceCore()
    {
      return default(System.Windows.Freezable);
    }

    public override double GetArea(double tolerance, ToleranceType type)
    {
      return default(double);
    }

    public override bool IsEmpty()
    {
      return default(bool);
    }

    public override bool MayHaveCurves()
    {
      return default(bool);
    }
    #endregion

    #region Properties and indexers
    public override System.Windows.Rect Bounds
    {
      get
      {
        return default(System.Windows.Rect);
      }
    }

    public Geometry Geometry1
    {
      get
      {
        return default(Geometry);
      }
      set
      {
      }
    }

    public Geometry Geometry2
    {
      get
      {
        return default(Geometry);
      }
      set
      {
      }
    }

    public GeometryCombineMode GeometryCombineMode
    {
      get
      {
        return default(GeometryCombineMode);
      }
      set
      {
      }
    }
    #endregion

    #region Fields
    public readonly static System.Windows.DependencyProperty Geometry1Property;
    public readonly static System.Windows.DependencyProperty Geometry2Property;
    public readonly static System.Windows.DependencyProperty GeometryCombineModeProperty;
    #endregion
  }
}
