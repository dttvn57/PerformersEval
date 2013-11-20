using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PerformersEval.DAL;
using PerformersEval.Models;

namespace PerformersEval.Controllers
{
    public class W9Controller : Controller
    {
        PerformersDB _db = new PerformersDB();

        //
        // GET: /FormsStatus/

        public ActionResult Index()
        {
            return View(new FormsStatus());
        }

        //
        // GET: /FormsStatus/Create
        public ActionResult Create()
        {
            SubW9 w9 = new FormsStatus
            {
            };
            return View(w9);
        }

        [HttpPost]
        public ActionResult Create(FormsStatus s)
        {
            if (ModelState.IsValid)
            {
            }

            return View(s);
        }

        public ActionResult Edit(string TIN)  //string TIN = "AClibAdmin"
        {
            // Taking care of the Admin/Manager who has been given the backdoor
            if (TIN.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return View("ListOfSubW9s", _db.SubW9s.ToList());
            }

            if (TIN != null)         // search by TIN
            {
                var subW9s = _db.SubW9s.Where(w => (w.TIN.Equals(TIN)));
                int cnt = subW9s.Count();
                if (cnt == 1)
                {
                    SubW9 w = subW9s.First();
                    return View(w);
                }
                else
                if (cnt == 0)
                {
                    TempData["OBJECT"] = null;
                    TempData["STATUS"] = "No Sub W-9 Found";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OBJECT"] = null;
                    TempData["STATUS"] = "Error: more than one Sub W-9. Please see the Admin.";
                    return RedirectToAction("Index");
                }
            }

            TempData["OBJECT"] = null;
            TempData["STATUS"] = "No Sub W-9 Found";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(SubW9 w9)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(w9).State = System.Data.EntityState.Modified;
                _db.SaveChanges();

                TempData["OBJECT"] = w9;
                TempData["STATUS"] = "Modification is saved successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(w9);
            }
        }

        public ActionResult Details(int id = 0)
        {
            SubW9 w9 = _db.SubW9s.Find(id);
            if (w9 == null)
            {
                TempData["STATUS"] = "Sub W-9 not Found.";
                return RedirectToAction("ListOfSubW9s");
            }
            return View("Details", w9);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
