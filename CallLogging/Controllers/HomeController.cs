using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CallLogging_Data;
using PagedList;
using System.IO;

namespace CallLogging.Controllers
{
  public class HomeController : Controller
  {
        private const int _firstPage=1;
        private const int _pageSize = 10;
    public ActionResult Index(int page=_firstPage, int pageSize= _pageSize)
    {
      CallLoggingViewModel vm = new CallLoggingViewModel();

      vm.HandleRequest(page,pageSize);
           
      return View(vm);
    }

    [HttpPost, ValidateInput(false)]
    public ActionResult Index(CallLoggingViewModel vm,  int page = _firstPage, int pageSize = _pageSize)
    {
      vm.IsValid = ModelState.IsValid;
            vm.HandleRequest(1, pageSize);
            try
            {
                /*Lopp for multiple files*/
                
                foreach (HttpPostedFileBase file in vm.Files)
                {
                    /*Geting the file name*/
                    string filename = System.IO.Path.GetFileName(file.FileName);
                    /*Saving the file in server folder*/
                    file.SaveAs(Server.MapPath("~/Images/" + filename));
                    string filepathtosave = "Images/" + filename;
                   // CreateAttachmentRecords(vm.Entity.MES_UID,vm.Entity.MES_CompanyId);
                    /*HERE WILL BE YOUR CODE TO SAVE THE FILE DETAIL IN DATA BASE*/
                }

                ViewBag.Message = "File Uploaded successfully.";
            }
            catch
            {
                ViewBag.Message = "Error while uploading the files.";
            }
            if (vm.IsValid) {
        // NOTE: Must clear the model state in order to bind
        //       the @Html helpers to the new model values
        ModelState.Clear();
      }
      else {
        foreach (KeyValuePair<string, string> item in vm.ValidationErrors) {
          ModelState.AddModelError(item.Key, item.Value);
        }
      }

      return View(vm);
    }

    public ActionResult About()
    {
      return View();
    }
        
        public JsonResult GetProblems(string Category,int CompanyId)
        {
            
            
            List<string> Problems;
            List<SelectListItem> ProblemsDropDowns;
            CallLoggingViewModel vm = new CallLoggingViewModel();
            Problems = vm.GetProblemCategory("problem", CompanyId, Category);
            ProblemsDropDowns = (from p in Problems
                                 select new SelectListItem { Value = p, Text = p }).ToList();
            //return Json(new SelectList(ProblemsDropDowns, "value", "text"));
            return Json(new SelectList(ProblemsDropDowns,"Value","Text"));
        }
  }
}