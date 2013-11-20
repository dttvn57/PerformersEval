using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PerformersEval.DAL;
using PerformersEval.Models;

namespace PerformersEval.Controllers
{
    [Authorize]
    [RequireHttps]
    public class UploadController : Controller
    {
        //
        // GET: /Upload/

        public ActionResult UploadDocument(string TIN = null)
        {
            FormsStatus f = new FormsStatus();
            if (TIN != null)
                f.Performer_TIN = TIN;

            return View();
        }

       [HttpPost]
        public ActionResult UploadDocument(HttpPostedFileBase file)
       {
           if (file != null && file.ContentLength > 0)
           {
               var fileName = Path.GetFileName(file.FileName);
               var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
               file.SaveAs(path);
           }

           return RedirectToAction("Upload");
        }
    }
}
