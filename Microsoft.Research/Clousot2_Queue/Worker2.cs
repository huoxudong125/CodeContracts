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

using System.Collections.Generic;
using System.IO;
using Microsoft.Cci;
using Microsoft.Cci.Analysis;
using Microsoft.Research.CodeAnalysis;
using Microsoft.Research.DataStructures;

namespace Microsoft.Research.CodeAnalysis
{
  class Clousot2WorkerFactory : LocalClousotWorkerFactory<LocalDefAdaptor, IParameterTypeInformation, MethodReferenceAdaptor, FieldReferenceAdaptor, IPropertyDefinition, IEventDefinition, TypeReferenceAdaptor, ICustomAttribute, IAssemblyReference>
  {
    public Clousot2WorkerFactory(IScheduler scheduler, FList<string> args, ISimpleLineWriter output, IDB db)
      : base(scheduler, args, output, db)
    { }

    protected override int CallClousotMain(string[] args, IOutputFullResultsFactory<MethodReferenceAdaptor,IAssemblyReference> outputFactory, IEnumerable<Caching.IClousotCacheFactory> cacheAccessorFactories)
    {
      return CCI2Driver.Main(args, outputFactory, cacheAccessorFactories);
    }
  }
}
