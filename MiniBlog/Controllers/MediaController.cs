﻿using MiniBlog.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MiniBlog.Controllers
{
    public class MediaController : Controller
    {
        private AdminContext db = new AdminContext();
        // GET: Media
        [_SessionControl]
        public ActionResult Index()
        {
            MediaModel model = new MediaModel();
            var files = from e in db.Files_V where e.Crud < 3 orderby e.FileName descending select e;
            var folders = from e in db.Folders_V where e.Crud < 3 orderby e.FolderName descending select e;
            model.Files = files.ToList();
            model.Folders = folders.ToList();
            return PartialView("_Index", model);
        }

        [_SessionControl]
        [HttpPost]
        [Route("Media/File/Upload")]
        public ActionResult Upload(MediaModel model)
        {
            bool pass = false;
            var folder = db.Folder.Where(a => a.FolderID == model.FolderID).SingleOrDefault();
            if (folder != null)
            {
                var file = Request.Files["file"];
                var fileExtension = Path.GetExtension(file.FileName);
                if (folder.FolderFileTypes != null)
                {
                    if (folder.FolderFileTypes.Length > 0)
                    {
                        foreach (String fileType in folder.FolderFileTypes.Split(',').ToList())
                        {
                            if (fileExtension == fileType)
                            {
                                pass = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    pass = true;
                }
                if (pass)
                {
                    String folderName = folder.ParentFolderID.ToString() + folder.FolderID.ToString();
                    String folderPath = Server.MapPath("~/Content/uploads/" + folderName);
                    string guid = Guid.NewGuid().ToString();
                    var path = Path.Combine(folderPath, guid + fileExtension);
                    file.SaveAs(path);
                    String filePath = "/Content/uploads/" + folderName + "/" + guid + fileExtension;
                    Models.File f = new Models.File();
                    f.FileName = file.FileName;
                    f.FilePath = filePath;
                    f.Date = DateTime.Now;
                    f.FileDescription = "";
                    f.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
                    f.Crud = 1;
                    f.FileSize = file.ContentLength;
                    f.FolderID = folder.FolderID;
                    f.FileHash = Helper.CalculateMD5(Server.MapPath(filePath));
                    f.AuthorID = Convert.ToInt32(User.Identity.Name);
                    db.File.Add(f);
                    db.SaveChanges();
                    return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 500;
                    ModelState.AddModelError("", "Dosya Yüklenemedi! Bu dosya türüne izin verilmiyor!\nİzin verilen dosya türleri: " + folder.FolderFileTypes);
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                Response.StatusCode = 500;
                ModelState.AddModelError("", "Dosya Yüklenemedi! Sanal Dizin Veritabanında Mevcut Değil!");
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.DenyGet);
            }
        }

        [_SessionControl]
        public ActionResult NewFolder(int? id)
        {
            FolderModel model = new FolderModel();
            model.ParentFolderID = (int)id;
            return PartialView("_NewFolder", model);
        }

        [_SessionControl]
        [HttpPost]
        [Route("Media/Folder/Add")]
        public ActionResult FolderAdd(FolderModel model)
        {
            if (ModelState.IsValid)
            {
                Folder folder = new Folder();
                folder.FolderName = model.FolderName;
                folder.FolderDescription = model.FolderDescription;
                folder.ParentFolderID = model.ParentFolderID;
                folder.AuthorID = Convert.ToInt32(User.Identity.Name);
                folder.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
                folder.Crud = 1;
                folder.Date = DateTime.Now;
                folder.CreateDate = DateTime.Now;
                folder.FolderFileTypes = model.FolderFileTypes;
                db.Folder.Add(folder);
                db.SaveChanges();
                String folderName = folder.ParentFolderID.ToString() + folder.FolderID.ToString();
                String folderPath = Server.MapPath("~/Content/uploads/" + folderName);
                if (!Directory.Exists(folderPath))
                {
                    DirectoryInfo dI = Directory.CreateDirectory(folderPath);
                }
                return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
            }
        }
        [_SessionControl]
        [HttpPost]
        [Route("Media/Folder/Save")]
        public ActionResult FolderSave(FolderModel model)
        {
            if (ModelState.IsValid)
            {
                var current = db.Folder.Find(model.FolderID);
                current.FolderName = model.FolderName;
                current.FolderDescription = model.FolderDescription;
                current.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
                current.Crud = 2;
                current.Date = DateTime.Now;
                current.FolderFileTypes = model.FolderFileTypes;
                db.SaveChanges();
                return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
            }
        }

        [_SessionControl]
        public ActionResult FolderEdit(int id)
        {
            var p = db.Folder.Where(a => a.FolderID == id).SingleOrDefault();
            FolderModel model = new FolderModel();
            model.FolderID = p.FolderID;
            model.FolderName = p.FolderName;
            model.FolderDescription = p.FolderDescription;
            model.FolderFileTypes = p.FolderFileTypes;
            model.ParentFolderID = p.ParentFolderID;
            return PartialView("_EditFolder", model);
        }

        [_SessionControl]
        [Route("Media/File/Delete/{id:int}")]
        public ActionResult FileDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var current = db.File.Find(id);
            current.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
            current.Crud = 3; //1: new | 2: update | 3: deleted
            current.Date = DateTime.Now;
            db.SaveChanges();
            return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
        }

        [_SessionControl]
        [Route("Media/Folder/Delete/{id:int}")]
        public ActionResult FolderDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var current = db.Folder.Find(id);
            current.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
            current.Crud = 3; //1: new | 2: update | 3: deleted
            current.Date = DateTime.Now; 
            db.SaveChanges();
            var files = from e in db.Files_V where e.Crud < 3 && e.FolderID == id orderby e.FileName descending select e;
            foreach (var file in files.ToList())
            {
                var f = db.File.Find(file.FileID);
                f.CrudAuthorID = Convert.ToInt32(User.Identity.Name);
                f.Crud = 3; //1: new | 2: update | 3: deleted
                f.Date = DateTime.Now;
                db.SaveChanges();
            }
            return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}