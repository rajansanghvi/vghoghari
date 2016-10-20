using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VGhoghari.Models {

  public class EventTO {

    public EventTO() {
      this.Code = string.Empty;
      this.Tags = string.Empty;
      this.TagList = new List<string>();
      this.Title = string.Empty;
      this.ShortDescription = string.Empty;
      this.Description = string.Empty;
      this.StartDate = DateTime.Now;
      this.FormattedStartDate = string.Empty;
      this.FormattedEndDate = string.Empty;
      this.EndDate = null;
      this.CostPerPerson = 0;
      this.TotalCapacity = 0;
      this.Venue = string.Empty;
      this.Country = string.Empty;
      this.State = string.Empty;
      this.City = string.Empty;
      this.BannerImage = string.Empty;
      this.BannerImageData = string.Empty;
    }

    public string Code { get; set; }

    public string Tags { get; set; }
    public List<string> TagList { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public string FormattedStartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string FormattedEndDate { get; set; }
    public int CostPerPerson { get; set; }
    public int TotalCapacity { get; set; }
    public string Venue { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
    public string ContactEmail { get; set; }
    public string BannerImage { get; set; }
    public string BannerImageData { get; set; }
  }
}