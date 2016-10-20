using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGhoghari.AppCodes.Business_Layer;
using VGhoghari.AppCodes.Enum;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;
using PagedList;
using PagedList.Mvc;

namespace VGhoghari.Areas.Admin.Controllers {
  public class MatrimonialController : Controller {

    public ActionResult BiodataCounts() {
      ViewBag.PendingCount = MatrimonialBL.GetBiodataCountByStatus(enApprovalStatus.Pending);
      ViewBag.ApprovedCount = MatrimonialBL.GetBiodataCountByStatus(enApprovalStatus.Approved);
      ViewBag.RejectedCount = MatrimonialBL.GetBiodataCountByStatus(enApprovalStatus.Rejected);

      return PartialView("BiodataCounts");
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult List(int? page, int? status) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      status = status ?? 1;
      enApprovalStatus biodataStatus = (enApprovalStatus) status;

      int count = 0;
      List<BiodataTO> biodataList = new List<BiodataTO>();

      switch(biodataStatus) {
        case enApprovalStatus.Pending:
          ViewBag.Status = "Pending";
          count = MatrimonialBL.GetBiodataCountByStatus(enApprovalStatus.Pending);
          if(count > 0) {
            biodataList = MatrimonialBL.GetBiodataListByStatus(page, enApprovalStatus.Pending);
          }
          break;
        case enApprovalStatus.Approved:
          ViewBag.Status = "Approved";
          count = MatrimonialBL.GetBiodataCountByStatus(enApprovalStatus.Approved);
          if(count > 0) {
            biodataList = MatrimonialBL.GetBiodataListByStatus(page, enApprovalStatus.Approved);
          }
          break;
        case enApprovalStatus.Rejected:
          ViewBag.Status = "Rejected";
          count = MatrimonialBL.GetBiodataCountByStatus(enApprovalStatus.Rejected);
          if(count > 0) {
            biodataList = MatrimonialBL.GetBiodataListByStatus(page, enApprovalStatus.Rejected);
          }
          break;
        default:
          ViewBag.Status = "Pending";
          count = MatrimonialBL.GetBiodataCountByStatus(enApprovalStatus.Pending);
          if(count > 0) {
            biodataList = MatrimonialBL.GetBiodataListByStatus(page, enApprovalStatus.Pending);
          }
          break;
      }

      var pagedData = new StaticPagedList<BiodataTO>(biodataList, page ?? 1, MatrimonialBL.MY_BIODATA_LIST_PAGE_SIZE, count);
      return View(pagedData);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Details(string code, int? page, int? status) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }
      
      BiodataTO biodata = MatrimonialBL.GetMyBiodataDetails(code);
      if(biodata == null) {
        return RedirectToAction("List", "Matrimonial", new { page = page, status = status });
      }

      return View(biodata);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult ApproveBiodata(string code, int? page, int? status) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      int rowsAffected = MatrimonialBL.UpdateApprovalStatus(code, enApprovalStatus.Approved);
      return RedirectToAction("List", "matrimonial", new { page = page, status = status });
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult RejectBiodata(string code, int? page, int? status) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      int rowsAffected = MatrimonialBL.UpdateApprovalStatus(code, enApprovalStatus.Rejected);
      return RedirectToAction("List", "matrimonial", new { page = page, status = status });
    }
  }
}