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

namespace MiniBlog.Controllers
{
    public class HomeController : Controller
    {
        private ArticlesVContext db = new ArticlesVContext();

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

        [HttpPost]
        public async Task<ActionResult> ContactSend(ContactForm form)
        {
            ResponseMessage response = new ResponseMessage();
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Gönderen: {0} ({1})</p><p>Mesaj:</p><p>{2}</p>";
                    var message = new MailMessage();
                    message.From = new MailAddress("iletisim@selahattinyuksel.net");
                    message.To.Add(new MailAddress("iletisim@selahattinyuksel.net"));
                    message.Subject = "selahattinyuksel.NET İletişim Formu";
                    message.Body = string.Format(body, form.name, form.email, form.message);
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "demo@test.com",
                            Password = "111"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "selahattinyuksel.net";
                        smtp.Port = 25;
                        smtp.EnableSsl = false;
                        await smtp.SendMailAsync(message);
                        response.Result = "OK";
                    }
                }
                catch (Exception err)
                {
                    response.Result = "Error";
                    response.Error = err.Message;
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
            var articles = from e in db.Articles_V orderby e.ArticleID descending select e;

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
                xr.WriteElementString("loc", ConfigurationManager.AppSettings["site_url"].ToString()  + "/blog/" + Helper.FriendlyURLTitle(a.CategoryName) + "/" + Helper.FriendlyURLTitle(a.Title) + "-" + a.ArticleID);
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
