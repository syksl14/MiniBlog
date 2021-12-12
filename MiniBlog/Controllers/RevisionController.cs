using MiniBlog.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MiniBlog.Controllers
{
    public class RevisionController : Controller
    {
        private ArticleContext db = new ArticleContext();
        // GET: Revision
        [_SessionControl]
        public ActionResult View(int? id)
        {
            var revision = db.Revision.Where(a => a.RevisionID == id).SingleOrDefault();
            return PartialView("_View", revision);
        }
        [_SessionControl]
        public ActionResult Index(int? id = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Revision> revisions = db.Revision
                      .Where(a => a.ArticleID == id).ToList();
            if (revisions == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleID = id;
            return PartialView("_Index", revisions.ToList());
        }
        [_SessionControl]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revision r = db.Revision.Find(id);
            db.Revision.Remove(r);
            db.SaveChanges();
            return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
        }

        [_SessionControl] 
        public ActionResult Save(int id)
        {
            var revision = db.Revision.Where(a => a.RevisionID == id).SingleOrDefault();
            var current = db.Article.Find(revision.ArticleID);
            current.Article1 = revision.Revision1;
            current.UpdateDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Articles", "Admin");
        }
    }
}