using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VGhoghari.AppCodes.Utilities {
  public class FilterTO {
    public string ColumnName { get; set; }
    public string Operator { get; set; }
    public string ColumnValue { get; set; }
  }
}