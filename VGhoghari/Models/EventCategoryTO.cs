using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VGhoghari.Models {
  public class EventCategoryTO {

    public EventCategoryTO() {
      this.Name = string.Empty;
      this.Description = string.Empty;
    }

    public string Name { get; set; }
    public string Description { get; set; }
  }
}