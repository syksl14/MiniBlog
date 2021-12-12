using MiniBlog.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MiniBlog.Controllers
{
    public class ArticleController : Controller
    {
        private ArticlesVContext db = new ArticlesVContext();
        private ArticleContext articleDb = new ArticleContext();
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        // GET: Article/Search/keyword
        public ActionResult Search(string content)
        {
            ViewBag.Content = content;
            if (content == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //P = Public
            //D = Draft
            List<Articles_V> article = db.Articles_V
                      .Where(a => a.Keywords.Contains(content) && a.Privacy == "P" || a.Title.Contains(content) && a.Privacy == "P").ToList();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        // GET: Article/Tag/keyword
        public ActionResult Tag(string keyword)
        {
            ViewBag.Keyword = "#" + keyword.Trim();
            if (keyword == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Articles_V> article = db.Articles_V
                      .Where(a => a.Keywords.Contains(keyword.Trim()) && a.Privacy == "P").ToList();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        // GET: Article/Details/5/title
        [Route("blog/{category}/{title}-{id:int}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article_V article = db.Article_V
                      .Where(a => a.ArticleID == id && a.Privacy == "P")
                      .SingleOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (Session["Article" + id] == null)
                {
                    var current = articleDb.Article.Find(id);
                    current.ReadNumber = article.ReadNumber + 1;
                    articleDb.SaveChanges();
                    Session["Article" + id] = true;
                }
            }
            return View(article);
        }

        [_SessionControl]
        [HttpPost] 
        public ActionResult Add(ArticleModel model)
        {
            if (ModelState.IsValid)
            {
                String CoverPhotoPath = "";
                if (Request.Files["file"].ContentLength > 0)
                {
                    var file = Request.Files["file"];
                    var fileName = Path.GetFileName(file.FileName);
                    string guid = Guid.NewGuid().ToString();
                    var path = Path.Combine(Server.MapPath("~/Content/uploads/"), guid + fileName);
                    file.SaveAs(path);
                    CoverPhotoPath = "/Content/uploads/" + guid + fileName;
                }
                Article a = new Article();
                a.AuthorID = Convert.ToInt32(User.Identity.Name);
                a.CategoryID = model.CategoryID.Value;
                a.Title = model.Title;
                a.Summary = model.Summary;
                a.Keywords = model.Keywords;
                a.Article1 = model.Article;
                a.Privacy = model.Privacy == "true" ? "D" : "P";
                if (CoverPhotoPath != "")
                {
                    a.CoverPhoto = CoverPhotoPath;
                }
                a.Date = DateTime.Now;
                articleDb.Article.Add(a);
                articleDb.SaveChanges();
                return RedirectToAction("Articles", "Admin");
            }
            return View(model);
        }

        [_SessionControl]
        public ActionResult Edit(int id)
        {
            var article = articleDb.Article.Where(a => a.ArticleID == id).SingleOrDefault();
            var categories = from e in db.Categories orderby e.CategoryName ascending select e;
            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName", article.CategoryID);
            ArticleModel model = new ArticleModel();
            model.ArticleID = article.ArticleID;
            model.Title = article.Title;
            model.Article = article.Article1;
            model.Summary = article.Summary;
            model.Keywords = article.Keywords;
            model.CategoryID = article.CategoryID;
            model.CoverPhoto = article.CoverPhoto;
            model.Privacy = article.Privacy;
            var revisions = articleDb.Revision.Where(a => a.ArticleID == id).OrderByDescending(a => a.RevisionID).Take(5);
            var rev_count = articleDb.Revision.Count();
            model.Revisions = revisions.ToList();
            model.RevisionCount = rev_count;

            return PartialView("_EditArticle", model);
        }

        [_SessionControl]
        [HttpPost]
        public ActionResult Save(ArticleModel model)
        {
            if (ModelState.IsValid)
            {
              
                    var current = articleDb.Article.Find(model.ArticleID);

                    DateTime updateDate = DateTime.Now;
                    Revision r = new Revision();
                    r.Revision1 = current.Article1;
                    r.Date = updateDate;
                    r.ArticleID = model.ArticleID;
                    articleDb.Revision.Add(r);

                    current.CategoryID = model.CategoryID.Value;
                    current.Title = model.Title;
                    current.Summary = model.Summary;
                    current.Keywords = model.Keywords;
                    current.Article1 = model.Article;
                    current.Privacy = model.Privacy == "true" ? "D": "P";
                    current.UpdateDate = updateDate;
                    if (Request.Files["file"].ContentLength > 0)
                    {
                        var file = Request.Files["file"];
                        var fileExtension = "." + Path.GetExtension(file.FileName);
                        string guid = Guid.NewGuid().ToString();
                        var path = Path.Combine(Server.MapPath("~/Content/uploads/"), guid + fileExtension);
                        file.SaveAs(path);
                        if ((System.IO.File.Exists(Server.MapPath(model.CoverPhoto))))
                        {
                            System.IO.File.Delete(Server.MapPath(model.CoverPhoto));
                        }
                        String CoverPhotoPath = "/Content/uploads/" + guid + fileExtension;
                        current.CoverPhoto = CoverPhotoPath;
                    }
                    articleDb.SaveChanges();
                    return RedirectToAction("Articles", "Admin");
                    //return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
                
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
            Article detail = articleDb.Article.Find(id);
            articleDb.Article.Remove(detail);
            articleDb.SaveChanges();
            return RedirectToAction("Articles", "Admin");
        }

        //Article/Category/page/id
        [Route("blog/{category}/{CategoryID:int}")]
        public ActionResult Category(int page = 1, int CategoryID = 0, string category = "")
        {
            Category c = db.Categories.Where(a => a.CategoryID == CategoryID).SingleOrDefault();
            ViewBag.CategoryName = c.CategoryName;
            var articles = from e in db.Articles_V where e.Privacy == "P" orderby e.ArticleID descending select e;
            return View(articles.Where(a => a.CategoryID == CategoryID).ToList().ToPagedList(page, 5));
        }

        public ActionResult Rss()
        {
            Helper.GenerateRSS();
            return View();
        }
    }
}