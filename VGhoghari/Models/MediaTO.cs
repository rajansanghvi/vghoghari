using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VGhoghari.Models {
  public class MediaTO {
    public MediaTO() {
      this.Id = 0;
      this.FileName = string.Empty;
      this.HumanisedFileName = string.Empty;
      this.MimeType = string.Empty;
      this.Size = 0;
    }

    [JsonIgnore]
    public int Id { get; set; }
    public string FileName { get; set; }
    public string HumanisedFileName { get; set; }
    public string Path { get; set; }
    public string MimeType { get; set; }
    public int Size { get; set; }
  }
}