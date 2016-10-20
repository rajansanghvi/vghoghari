using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VGhoghari.Models {
  public class ProjectTO {
    public ProjectTO() {
      this.Code = string.Empty;
      this.Title = string.Empty;
      this.ShortDescription = string.Empty;
      this.Description = string.Empty;
      this.BannerImage = string.Empty;
      this.BannerImageData = string.Empty;
    }

    public string Code { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
    public string ContactEmail { get; set; }
    public string BannerImage { get; set; }
    public string BannerImageData { get; set; }
  }
}