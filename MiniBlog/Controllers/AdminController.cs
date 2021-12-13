using MiniBlog.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web.Mvc;
using System.Web.Security;

namespace MiniBlog.Controllers
{
    public class AdminController : Controller
    {
        private ArticlesVContext db = new ArticlesVContext();
        private AuthorsContext admin = new AuthorsContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                FormsAuthentication.SignOut();
                return View();
            }
            return Redirect("/Admin/Home");
        }

        [_SessionControl]
        public ActionResult Users()
        {
            var levels = from e in admin.Level orderby e.LevelID ascending select e;
            ViewBag.Levels = new SelectList(levels, "LevelID", "Name");
            return View();
        }

        [_SessionControl]
        public ActionResult Drafts(int page= 1)
        {
            var articles = from e in db.Articles_V where e.Privacy == "D" orderby e.ArticleID descending select e; //Draft articles...
            return PartialView("_Drafts", articles.ToList()); 
        }

        [_SessionControl]
        public ActionResult Revisions()
        {
            var articles = from e in db.Revisions_V orderby e.RevisionID descending select e; //Revisions articles...
            return PartialView("_Revisions", articles.ToList());
        }

        [_SessionControl]
        public ActionResult Home(int page = 1)
        {
            return View();
        }
        [_SessionControl]
        public ActionResult Articles()
        {
            var articles = from e in db.Articles_V where e.Privacy == "P" orderby e.ArticleID descending select e; //Public articles...
            var categories = from e in db.Categories orderby e.CategoryName ascending select e;
            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
            return View(articles.ToList());
        }
        [_SessionControl]
        public ActionResult Categories(int page = 1)
        {
            CategoryContext db = new CategoryContext();
            var categories = from e in db.Category orderby e.CategoryName ascending select e;
            return View(categories.ToList().ToPagedList(page, 10));
        }
     
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(LoginModel model, string returnurl)
        {
            if (ModelState.IsValid)
            {
                List<Author> authors = admin.User.Where(a => a.Email == model.EMail && a.Password == model.Password).ToList();
                if (authors.Count > 0)
                {
                    FormsAuthentication.SetAuthCookie(authors[0].AuthorID.ToString(), true);
                    return RedirectToAction("Home", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "E-Posta veya şifre hatalı!");
                }
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Admin");
        }

        [ChildActionOnly]
        public ActionResult NavBar()
        {
            int AuthorID = Convert.ToInt32(User.Identity.Name);
            var navbar = admin.User.Where(a => a.AuthorID == AuthorID).SingleOrDefault();
            return PartialView("_NavBar", navbar);
        }
    }
}