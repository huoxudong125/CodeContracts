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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace ClousotRoslynExtension.OptionsPagePackage {
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public class ClousotOptionPage : DialogPage {

    public ClousotOptionPage() {
      SetToDefaults();
    }

    const string DefaultWarningLevel = "low";

    [Category("Clousot/Roslyn")]
    [DisplayName("Warning Level")]
    [Description("low, mediumlow, medium, or full")]
    [DefaultValue(DefaultWarningLevel)]
    public string WarningLevel {
      get {
        return warningLevel;
      }
      set {
        var v = value.ToLower();
        if (v.Equals("low") || v.Equals("mediumlow") || v.Equals("medium") || v.Equals("full")) {
          warningLevel = value;
        } else {
          warningLevel = "low";
        }
      }
    }
      private string warningLevel;

    const string DefaultOtherOptions = "";

    [Category("Clousot/Roslyn")]
    [DisplayName("Other options")]
    [Description("a string of command-line options to pass directly to Clousot")]
    [DefaultValue(DefaultOtherOptions)]
    public string OtherOptions { get; set; }

    public override void ResetSettings() {
      base.ResetSettings();
      SetToDefaults();
    }

    private void SetToDefaults() {
      this.WarningLevel = DefaultWarningLevel;
      this.OtherOptions = DefaultOtherOptions;
    }
  }
}
