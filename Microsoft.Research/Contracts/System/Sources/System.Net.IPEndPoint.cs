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

// File System.Net.IPEndPoint.cs
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


namespace System.Net
{
  public partial class IPEndPoint : EndPoint
  {
    #region Methods and constructors
    public override EndPoint Create(SocketAddress socketAddress)
    {
      return default(EndPoint);
    }

    public override bool Equals(Object comparand)
    {
      return default(bool);
    }

    public override int GetHashCode()
    {
      return default(int);
    }

    public IPEndPoint(long address, int port)
    {
    }

    public IPEndPoint(IPAddress address, int port)
    {
    }

    public override SocketAddress Serialize()
    {
      return default(SocketAddress);
    }

    public override string ToString()
    {
      return default(string);
    }
    #endregion

    #region Properties and indexers
    public IPAddress Address
    {
      get
      {
        return default(IPAddress);
      }
      set
      {
      }
    }

    public override System.Net.Sockets.AddressFamily AddressFamily
    {
      get
      {
        return default(System.Net.Sockets.AddressFamily);
      }
    }

    public int Port
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

    #region Fields
    public static int MaxPort;
    public static int MinPort;
    #endregion
  }
}
