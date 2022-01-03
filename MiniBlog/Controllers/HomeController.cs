using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiniBlog.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using PagedList;
using System.Xml;
using System.Text;
using System.Web.Routing;
using System.Configuration;
using System.Web.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniBlog.Controllers
{
    public class HomeController : Controller
    {
        private ArticlesVContext db = new ArticlesVContext();
        private AdminContext db2 = new AdminContext();
        // GET: Home 
        public ActionResult Index(int page = 1)
        {
            var articles = from e in db.Articles_V where e.Privacy == "P" orderby e.ArticleID descending select e; //Public Articles
            return View(articles.ToList().ToPagedList(page, 5));
        }
        // GET: Home/About
        [OutputCache(Duration = 86000)] //yaklaşık 24 saat önbellekte tutulur
        public ActionResult About()
        {
            return View();
        }
        // GET: Home/Contact
        [OutputCache(Duration = 86000)] //yaklaşık 24 saat önbellekte tutulur
        public ActionResult Contact()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Pages()
        {
            var pages = from e in db2.Pages_V where e.Privacy == "P" && e.Crud < 3 orderby e.PageOrder ascending select e;
            return PartialView("_Pages", pages.ToList());
        }

        [Route("Home/{friendurl}-{id:int}")]
        public ActionResult Page(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page p = db2.Pages
                      .Where(a => a.PageID == id && a.Privacy == "P")
                      .SingleOrDefault();
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        [HttpPost]
        public async Task<ActionResult> ContactSend(ContactForm form)
        {
            ResponseMessage response = new ResponseMessage();
            if (ModelState.IsValid)
            {
                bool reCapthca_pass = false;
                Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
                if (config.AppSettings.Settings["reCaptcha_isEnable"].Value.ToString() == "True")
                {
                    string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + config.AppSettings.Settings["reCaptcha_hiddenKey"].Value + "&response=" + form.key;
                    String data = (new WebClient()).DownloadString(url);
                    JToken objJson = JObject.Parse(data);
                    if((bool)objJson["success"])
                    {
                        reCapthca_pass = true;
                    }
                    else
                    {
                        response.Error = "Doğrulama başarısız lütfen robot olmadığınızı doğrulayınız.";
                        response.Result = "reCaptcha_VerifyError";
                    }
                } else
                {
                    reCapthca_pass = true;
                }
                if (reCapthca_pass)
                {
                    String body = "<p><strong>Gönderen:</strong></p>";
                    body += "<p>{0} ({1})</p>";
                    body += "<p><strong>Mesaj:</strong></p>";
                    body += "<p>{2}</p>";
                    String title = "İletişim Formu";
                    body = string.Format(body, form.name, form.email, Helper.StripHTML(form.message));
                    response = await Helper.MailSend(title, body, null);
                }

            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [ChildActionOnly]
        public ActionResult Categories()
        {
            var categories = from e in db.Categories orderby e.CategoryName ascending select e;
            return PartialView("_Categories", categories.ToList());
        }
        [ChildActionOnly]
        public ActionResult LatestArticles()
        {
            var articles = from e in db.Articles_V where e.Privacy == "P" orderby e.ArticleID descending select e;
            return PartialView("_LatestArticles", articles.Take(5));
        }
        [Route("SiteMap")]
        public ActionResult SiteMap()
        {
            var articles = from e in db.Articles_V where e.Privacy == "P" orderby e.ArticleID descending select e;

            Response.Clear();
            Response.ContentType = "text/xml";
            XmlTextWriter xr = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            xr.WriteStartDocument();
            xr.WriteStartElement("urlset");
            xr.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xr.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xr.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");

            xr.WriteStartElement("url");
            xr.WriteElementString("loc", ConfigurationManager.AppSettings["site_url"].ToString());
            xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
            xr.WriteElementString("changefreq", "daily");
            xr.WriteElementString("priority", "1");
            xr.WriteEndElement();

            foreach (var a in articles.ToList())
            {
                xr.WriteStartElement("url");
                xr.WriteElementString("loc", ConfigurationManager.AppSettings["site_url"].ToString() + "/blog/" + Helper.FriendlyURLTitle(a.CategoryName) + "/" + Helper.FriendlyURLTitle(a.Title) + "-" + a.ArticleID);
                xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                xr.WriteElementString("priority", "0.5");
                xr.WriteElementString("changefreq", "monthly");
                xr.WriteEndElement();
            }
            xr.WriteEndDocument();
            xr.Flush();
            xr.Close();
            Response.End();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
