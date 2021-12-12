using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiniBlog.Models;
using System.Net;

namespace MiniBlog.Controllers
{
    public class UserController : Controller
    {
        private AuthorsContext db = new AuthorsContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [_SessionControl]
        [HttpPost]
        public ActionResult Add(UserModel model)
        {
            if (ModelState.IsValid)
            {
                Author user = new Author();
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.Password = model.Password;
                user.AuthorityLevel = model.AuthorityLevel;
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Users", "Admin");
            }
            return View(model);
        }

        [_SessionControl]
        public ActionResult Edit(int id)
        {
            var c = db.User.Where(a => a.AuthorID == id).SingleOrDefault();
            UserModel model = new UserModel();
            model.Email = c.Email;
            model.Name = c.Name;
            model.Surname = c.Surname;
            model.AuthorID = c.AuthorID;
            model.AuthorityLevel = c.AuthorityLevel.Value;
            return PartialView("_EditUser", model);
        }

        [_SessionControl]
        [HttpPost]
        public ActionResult Save(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var current = db.User.Find(model.AuthorID);
                current.Name = model.Name;
                current.Surname = model.Surname;
                current.Email = model.Email;
                if(model.Password == null || model.Password == String.Empty)
                {
                    var usr = db.User.Where(a => a.AuthorID == model.AuthorID).SingleOrDefault();
                    current.Password = usr.Password;
                }
                else
                {
                    current.Password = model.Password;
                }
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
            Author detail = db.User.Find(id);
            db.User.Remove(detail);
            db.SaveChanges();
            return RedirectToAction("Users", "Admin");
        }
    }
}