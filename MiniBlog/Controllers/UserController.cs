using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiniBlog.Models;
using System.Net;
using PagedList;
using System.IO;

namespace MiniBlog.Controllers
{
    public class UserController : Controller
    {
        private AuthorsContext admin = new AuthorsContext();
        private AuthorsContext db = new AuthorsContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [_SessionControl]
        public ActionResult List(int page = 1, int level = 0)
        {
            ViewBag.Level = level;
            if (level > 0)
            {
                var users = from e in admin.Users where e.AuthorityLevel.Value.Equals(level) orderby e.AuthorID descending select e;
                return PartialView("_List", users.ToList().ToPagedList(page, 5));
            }
            else
            {
                var users = from e in admin.Users orderby e.AuthorID descending select e;
                return PartialView("_List", users.ToList().ToPagedList(page, 5));
            }
        }

        [_SessionControl]
        [HttpPost]
        public ActionResult Add(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var usercheck = db.User.Where(a => a.Email == model.Email || a.UserName == model.UserName).SingleOrDefault();
                if(usercheck == null)
                {
                    String PhotoPath = "";
                    if (Request.Files["file"].ContentLength > 0)
                    {
                        var file = Request.Files["file"];
                        var fileName = Path.GetFileName(file.FileName);
                        string guid = Guid.NewGuid().ToString();
                        var path = Path.Combine(Server.MapPath("~/Content/uploads/"), guid + fileName);
                        file.SaveAs(path);
                        PhotoPath = "/Content/uploads/" + guid + fileName;
                    }
                    Author user = new Author();
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Email = model.Email;
                    user.Password = Helper.CreateMD5(model.Password);
                    user.AuthorityLevel = model.AuthorityLevel;
                    user.UserName = model.UserName;
                    if (PhotoPath != "")
                    {
                        user.ProfilePicture = PhotoPath;
                    }
                    db.User.Add(user);
                    db.SaveChanges();
                    return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
                }else
                {
                    ModelState.AddModelError("", "Böyle bir kullanıcı zaten mevcut!");
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
                } 
            }
            else
            {
                //ajax taraflı hata döndürme yapılacak.
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
            } 
        }

        [_SessionControl]
        public ActionResult New()
        {
            var levels = from e in admin.Level orderby e.LevelID ascending select e;
            ViewBag.Levels = new SelectList(levels, "LevelID", "Name");
            return PartialView("_NewUser");
        }

        [_SessionControl]
        public ActionResult Edit(int id)
        {
            var c = db.User.Where(a => a.AuthorID == id).SingleOrDefault();
            UserModel model = new UserModel();
            var levels = from e in admin.Level orderby e.LevelID ascending select e;
            ViewBag.Levels = new SelectList(levels, "LevelID", "Name", model.AuthorityLevel);
            model.Email = c.Email;
            model.Name = c.Name;
            model.Surname = c.Surname;
            model.AuthorID = c.AuthorID;
            model.AuthorityLevel = c.AuthorityLevel.Value;
            model.ProfilePicture = c.ProfilePicture;
            model.UserName = c.UserName;
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
                if (model.Password == null || model.Password == String.Empty)
                {
                    var usr = db.User.Where(a => a.AuthorID == model.AuthorID).SingleOrDefault();
                    current.Password = Helper.CreateMD5(usr.Password);
                }
                else
                { 
                    current.Password = Helper.CreateMD5(model.Password);
                }
                if (model.UserName != current.UserName)
                {
                    var usercheck = db.User.Where(a => a.UserName == model.UserName).SingleOrDefault();
                    if (usercheck == null)
                    { 
                        current.UserName = model.UserName;
                    }else
                    {
                        ModelState.AddModelError("", "Böyle bir kullanıcı adı kullanılıyor. Lütfen başka bir kullanıcı adı giriniz!");
                        return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (model.Email != current.Email)
                {
                    var usercheck = db.User.Where(a => a.Email == model.Email).SingleOrDefault();
                    if (usercheck == null)
                    {
                        current.Email = model.Email;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Böyle e-posta adresi kullanılıyor. Lütfen başka bir e-posta adresi giriniz!");
                        return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (Request.Files["file"].ContentLength > 0)
                {
                    var file = Request.Files["file"];
                    var fileExtension = Path.GetExtension(file.FileName);
                    string guid = Guid.NewGuid().ToString();
                    var path = Path.Combine(Server.MapPath("~/Content/uploads/"), guid + fileExtension);
                    file.SaveAs(path);
                    if ((System.IO.File.Exists(Server.MapPath(current.ProfilePicture))))
                    {
                        System.IO.File.Delete(Server.MapPath(current.ProfilePicture));
                    }
                    String CoverPhotoPath = "/Content/uploads/" + guid + fileExtension;
                    current.ProfilePicture = CoverPhotoPath;
                }
                current.AuthorityLevel = model.AuthorityLevel;

                db.SaveChanges();
                return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
            }else
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
            Author detail = db.User.Find(id);
            db.User.Remove(detail);
            db.SaveChanges();
            return RedirectToAction("Users", "Admin");
        }
    }
}