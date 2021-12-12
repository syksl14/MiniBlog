using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiniBlog.Models;
using System.Net;

namespace MiniBlog.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryContext db = new CategoryContext();
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        [_SessionControl]
        [HttpPost]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category();
                category.CategoryName = model.CategoryName;
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Categories", "Admin");
            }
            return View(model);
        }

        [_SessionControl]
        public ActionResult Edit(int id)
        {
            var c = db.Category.Where(a => a.CategoryID == id).SingleOrDefault();
            CategoryModel model = new CategoryModel();
            model.CategoryName = c.CategoryName;
            model.CategoryID = c.CategoryID;
            return PartialView("_EditCategory", model);
        }

        [_SessionControl]
        [HttpPost]
        public ActionResult Save(Category model)
        {
            if (ModelState.IsValid)
            {
                var current = db.Category.Find(model.CategoryID);
                current.CategoryName = model.CategoryName;
                db.SaveChanges();
                return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
            }
            return View(model);
        }

        [_SessionControl]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category detail = db.Category.Find(id);
            db.Category.Remove(detail);
            db.SaveChanges();
            return RedirectToAction("Categories", "Admin");
        }
    }
}