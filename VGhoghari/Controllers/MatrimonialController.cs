using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGhoghari.AppCodes.Business_Layer;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;
using PagedList;
using PagedList.Mvc;
using VGhoghari.AppCodes.Enum;

namespace VGhoghari.Controllers {
  public class MatrimonialController : Controller {

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddBasicInfo(string code) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if(!string.IsNullOrWhiteSpace(code)) {
        if(!MatrimonialBL.IsMyBiodata(code)) {
          return RedirectToAction("Manage", "Matrimonial");
        }
      }
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddPersonalInfo(string code) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if(string.IsNullOrWhiteSpace(code)
        || !MatrimonialBL.IsMyBiodata(code)) {
        return RedirectToAction("Manage", "Matrimonial");
      }
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddProfessionalInfo(string code) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if(string.IsNullOrWhiteSpace(code)
        || !MatrimonialBL.IsMyBiodata(code)) {
        return RedirectToAction("Manage", "Matrimonial");
      }
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddFamilyInfo(string code) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if(string.IsNullOrWhiteSpace(code)
        || !MatrimonialBL.IsMyBiodata(code)) {
        return RedirectToAction("Manage", "Matrimonial");
      }
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddFamilyOccupationInfo(string code) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if(string.IsNullOrWhiteSpace(code)
        || !MatrimonialBL.IsMyBiodata(code)) {
        return RedirectToAction("Manage", "Matrimonial");
      }
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddSibblingInfo(string code) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if(string.IsNullOrWhiteSpace(code)
        || !MatrimonialBL.IsMyBiodata(code)) {
        return RedirectToAction("Manage", "Matrimonial");
      }
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddAdditionalInfo(string code) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if(string.IsNullOrWhiteSpace(code)
        || !MatrimonialBL.IsMyBiodata(code)) {
        return RedirectToAction("Manage", "Matrimonial");
      }
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult Manage(int? page, int? status) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      status = status ?? 0;
      enApprovalStatus biodataStatus = (enApprovalStatus) status;

      int count = 0;
      List<BiodataTO> biodataList = new List<BiodataTO>();

      switch(biodataStatus) {
        case enApprovalStatus.Pending:
          ViewBag.Status = "Pending";
          count = MatrimonialBL.CountMyBiodataByStatus(enApprovalStatus.Pending);
          if(count > 0) {
            biodataList = MatrimonialBL.GetMyBiodataListByStatus(page, enApprovalStatus.Pending);
          }
          break;
        case enApprovalStatus.Approved:
          ViewBag.Status = "Approved";
          count = MatrimonialBL.CountMyBiodataByStatus(enApprovalStatus.Approved);
          if(count > 0) {
            biodataList = MatrimonialBL.GetMyBiodataListByStatus(page, enApprovalStatus.Approved);
          }
          break;
        case enApprovalStatus.Rejected:
          ViewBag.Status = "Rejected";
          count = MatrimonialBL.CountMyBiodataByStatus(enApprovalStatus.Rejected);
          if(count > 0) {
            biodataList = MatrimonialBL.GetMyBiodataListByStatus(page, enApprovalStatus.Rejected);
          }
          break;
        default:
          ViewBag.Status = "Incomplete";
          count = MatrimonialBL.CountMyBiodataByStatus(enApprovalStatus.In_Complete);
          if(count > 0) {
            biodataList = MatrimonialBL.GetMyBiodataListByStatus(page, enApprovalStatus.In_Complete);
          }
          break;
      }
      
      var pagedData = new StaticPagedList<BiodataTO>(biodataList, page ?? 1, MatrimonialBL.MY_BIODATA_LIST_PAGE_SIZE, count);
      return View(pagedData);
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult MyBiodataDetails(string code) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if(string.IsNullOrWhiteSpace(code)
        || !MatrimonialBL.IsMyBiodata(code)) {
        return RedirectToAction("Manage", "Matrimonial");
      }
      BiodataTO biodataDetails = MatrimonialBL.GetMyBiodataDetails(code);
      return View(biodataDetails);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public ActionResult DeleteMyBiodata(string code, int? page, int? status) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      MatrimonialBL.DeleteMyBiodataByCode(code);
      return RedirectToAction("Manage", "Matrimonial", new { page = page, status = status });
    }

    [HttpGet]
    public ActionResult AllBiodata(int? page) {

      int count = MatrimonialBL.CountAllActiveBiodatas();
      List<BiodataTO> biodatas = new List<BiodataTO>();
      if(count > 0) {
        biodatas = MatrimonialBL.GetAllBiodatas(page);
      }
      var pagedData = new StaticPagedList<BiodataTO>(biodatas, page ?? 1, MatrimonialBL.BIODATA_LIST_PAGE_SIZE, count);
      return View(pagedData);
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult Details(string code, int? page) {
      if(!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      int count = MatrimonialBL.CountOfMyActiveBiodata();
      if(count <= 0) {
        RedirectToAction("AddBasicInfo", "Matrimonial");
      }

      BiodataTO biodata = MatrimonialBL.GetMyBiodataDetails(code);
      if(biodata == null) {
        return RedirectToAction("AllBiodata", "Matrimonial", new { page = page });
      }

      return View(biodata);
    }
  }
}