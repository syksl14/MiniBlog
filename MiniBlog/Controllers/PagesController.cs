using MiniBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MiniBlog.Controllers
{
    public class PagesController : Controller
    {
        private AuthorsContext admin = new AuthorsContext();
        private ArticleContext db = new ArticleContext();
        // GET: Pages
        [_SessionControl]
        public ActionResult Index(String privacy = "T")
        {
            ViewBag.Privacy = privacy;
            if (privacy == "T")
            {
                var pages = from e in admin.Pages where e.Crud < 3 orderby e.PageOrder ascending select e;
                return PartialView("_List", pages.ToList());
            }
            else
            {
                var pages = from e in admin.Pages where e.Privacy == privacy && e.Crud < 3 orderby e.PageOrder ascending select e;
                return PartialView("_List", pages.ToList());
            }
        }

        [_SessionControl]
        public ActionResult Edit(int id)
        {
            var p = db.Page.Where(a => a.PageID == id).SingleOrDefault();
            PageModel model = new PageModel();
            model.PageID = p.PageID;
            model.Title = p.Title;
            model.HeaderText = p.HeaderText;
            model.Page = p.Page1;
            model.FriendlyName = p.FriendlyName;
            model.Keywords = p.Keywords;
            model.CoverPhoto = p.CoverPhoto;
            model.Privacy = p.Privacy;
            model.PageOrder = p.PageOrder;
            return PartialView("_EditPage", model);
        }

        [_SessionControl]
        public ActionResult New()
        {
            var item = admin.Pages
                   .OrderByDescending(p => p.PageID)
                   .FirstOrDefault();
            PageModel model = new PageModel();
            if (item != null)
            {
                model.PageOrder = item.PageOrder + 1;
            }
            else
            {
                model.PageOrder = 1;
            }
            return PartialView("_NewPage", model);
        }

        [_SessionControl]
        [HttpPost]
        public ActionResult Add(PageModel model)
        {
            if (ModelState.IsValid)
            {
                Page page = new Page();
                page.Title = model.Title;
                page.HeaderText = model.HeaderText;
                page.FriendlyName = model.FriendlyName;
                page.Keywords = model.Keywords;
                page.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
                page.Crud = 1; //1: new | 2: update | 3: deleted
                page.Page1 = model.Page;
                page.Privacy = model.Privacy == "true" ? "D" : "P";
                page.PageOrder = model.PageOrder;
                if (Request.Files["file"].ContentLength > 0)
                {
                    var file = Request.Files["file"];
                    var fileExtension = Path.GetExtension(file.FileName);
                    string guid = Guid.NewGuid().ToString();
                    var path = Path.Combine(Server.MapPath("~/Content/uploads/"), guid + fileExtension);
                    file.SaveAs(path);
                    if ((System.IO.File.Exists(Server.MapPath(page.CoverPhoto))))
                    {
                        System.IO.File.Delete(Server.MapPath(page.CoverPhoto));
                    }
                    String CoverPhotoPath = "/Content/uploads/" + guid + fileExtension;
                    page.CoverPhoto = CoverPhotoPath;
                }
                page.Date = DateTime.Now;
                db.Page.Add(page);
                db.SaveChanges();
                return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //ajax taraflı hata döndürme yapılacak.
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
            }
        }

        [_SessionControl]
        [HttpPost]
        public ActionResult Save(PageModel model)
        {
            if (ModelState.IsValid)
            {
                var current = db.Page.Find(model.PageID);
                current.Title = model.Title;
                current.HeaderText = model.HeaderText;
                current.FriendlyName = model.FriendlyName;
                current.Keywords = model.Keywords;
                current.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
                current.Crud = 2; //1: new | 2: update | 3: deleted
                current.Page1 = model.Page;
                current.Privacy = model.Privacy == "true" ? "D" : "P";
                current.PageOrder = model.PageOrder;
                if (Request.Files["file"].ContentLength > 0)
                {
                    var file = Request.Files["file"];
                    var fileExtension = Path.GetExtension(file.FileName);
                    string guid = Guid.NewGuid().ToString();
                    var path = Path.Combine(Server.MapPath("~/Content/uploads/"), guid + fileExtension);
                    file.SaveAs(path);
                    if ((System.IO.File.Exists(Server.MapPath(current.CoverPhoto))))
                    {
                        System.IO.File.Delete(Server.MapPath(current.CoverPhoto));
                    }
                    String CoverPhotoPath = "/Content/uploads/" + guid + fileExtension;
                    current.CoverPhoto = CoverPhotoPath;
                }
                current.Date = DateTime.Now;
                db.SaveChanges();
                return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //ajax taraflı hata döndürme yapılacak.
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
            }
        }

        [_SessionControl]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var current = db.Page.Find(id);
            current.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
            current.Crud = 3; //1: new | 2: update | 3: deleted
            db.SaveChanges();
            return RedirectToAction("Pages", "Admin");
        }
    }
}