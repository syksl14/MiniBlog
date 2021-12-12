using MiniBlog.Controllers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MiniBlog
{
    public static class Helper
    {
        public static string FriendlyURLTitle(string incomingText)
        {
            if (incomingText != null)
            {
                incomingText = incomingText.Replace("ş", "s");
                incomingText = incomingText.Replace("Ş", "s");
                incomingText = incomingText.Replace("İ", "i");
                incomingText = incomingText.Replace("I", "i");
                incomingText = incomingText.Replace("ı", "i");
                incomingText = incomingText.Replace("ö", "o");
                incomingText = incomingText.Replace("Ö", "o");
                incomingText = incomingText.Replace("ü", "u");
                incomingText = incomingText.Replace("Ü", "u");
                incomingText = incomingText.Replace("Ç", "c");
                incomingText = incomingText.Replace("ç", "c");
                incomingText = incomingText.Replace("ğ", "g");
                incomingText = incomingText.Replace("Ğ", "g");
                incomingText = incomingText.Replace(" ", "-");
                incomingText = incomingText.Replace("---", "-");
                incomingText = incomingText.Replace("?", "");
                incomingText = incomingText.Replace("/", "");
                incomingText = incomingText.Replace(".", "");
                incomingText = incomingText.Replace("'", "");
                incomingText = incomingText.Replace("#", "");
                incomingText = incomingText.Replace("%", "");
                incomingText = incomingText.Replace("&", "");
                incomingText = incomingText.Replace("*", "");
                incomingText = incomingText.Replace("!", "");
                incomingText = incomingText.Replace("@", "");
                incomingText = incomingText.Replace("+", "");
                incomingText = incomingText.ToLower();
                incomingText = incomingText.Trim();
                // tüm harfleri küçült
                string encodedUrl = (incomingText ?? "").ToLower();
                // & ile " " yer değiştirme
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");
                // " " karakterlerini silme
                encodedUrl = encodedUrl.Replace("'", "");
                // geçersiz karakterleri sil
                encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");
                // tekrar edenleri sil
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");
                // karakterlerin arasına tire koy
                encodedUrl = encodedUrl.Trim('-');
                return encodedUrl;
            }
            else
            {
                return "";
            }
        }
        public static string FriendlyURLTitle(this UrlHelper urlHelper, string incomingText)
        {

            if (incomingText != null)
            {
                incomingText = incomingText.Replace("ş", "s");
                incomingText = incomingText.Replace("Ş", "s");
                incomingText = incomingText.Replace("İ", "i");
                incomingText = incomingText.Replace("I", "i");
                incomingText = incomingText.Replace("ı", "i");
                incomingText = incomingText.Replace("ö", "o");
                incomingText = incomingText.Replace("Ö", "o");
                incomingText = incomingText.Replace("ü", "u");
                incomingText = incomingText.Replace("Ü", "u");
                incomingText = incomingText.Replace("Ç", "c");
                incomingText = incomingText.Replace("ç", "c");
                incomingText = incomingText.Replace("ğ", "g");
                incomingText = incomingText.Replace("Ğ", "g");
                incomingText = incomingText.Replace(" ", "-");
                incomingText = incomingText.Replace("---", "-");
                incomingText = incomingText.Replace("?", "");
                incomingText = incomingText.Replace("/", "");
                incomingText = incomingText.Replace(".", "");
                incomingText = incomingText.Replace("'", "");
                incomingText = incomingText.Replace("#", "");
                incomingText = incomingText.Replace("%", "");
                incomingText = incomingText.Replace("&", "");
                incomingText = incomingText.Replace("*", "");
                incomingText = incomingText.Replace("!", "");
                incomingText = incomingText.Replace("@", "");
                incomingText = incomingText.Replace("+", "");
                incomingText = incomingText.ToLower();
                incomingText = incomingText.Trim();
                // tüm harfleri küçült
                string encodedUrl = (incomingText ?? "").ToLower();
                // & ile " " yer değiştirme
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");
                // " " karakterlerini silme
                encodedUrl = encodedUrl.Replace("'", "");
                // geçersiz karakterleri sil
                encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");
                // tekrar edenleri sil
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");
                // karakterlerin arasına tire koy
                encodedUrl = encodedUrl.Trim('-');
                return encodedUrl;
            }
            else
            {
                return "";
            }
        }
        public static void GenerateRSS()
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/xml";
            XmlTextWriter rssWriter = new XmlTextWriter(HttpContext.Current.Response.OutputStream, Encoding.UTF8);
            rssWriter.WriteStartDocument(); 
            rssWriter.WriteStartElement("rss");
            rssWriter.WriteAttributeString("version", "1.0");
            rssWriter.WriteStartElement("channel");
            rssWriter.WriteElementString("title", "selahattinyuksel.net RSS");
            rssWriter.WriteElementString("link", "selahattinyuksel.net/Article/Rss");
            rssWriter.WriteElementString("description", ConfigurationManager.AppSettings["site_name"].ToString() + " -- " + ConfigurationManager.AppSettings["slogan"].ToString());
            rssWriter.WriteElementString("copyright", "(c) "+ DateTime.Now.Year + ", SelahattinYuksel.NET");
            rssWriter.WriteElementString("pubDate", DateTime.Now.ToString("dd.MM.yyyy"));
            rssWriter.WriteElementString("language", "tr-TR");
            rssWriter.WriteElementString("webMaster", "iletisim@selahattinyuksel.net");
            ArticlesVContext db = new ArticlesVContext();
            var articles = from e in db.Articles_V orderby e.ArticleID descending select e;
            foreach (var rss in articles.ToList().ToPagedList(1, 10))
            {
                rssWriter.WriteStartElement("item");
                rssWriter.WriteElementString("title", rss.Title);
                rssWriter.WriteElementString("description", rss.Summary);
                rssWriter.WriteElementString("link", ConfigurationManager.AppSettings["site_url"].ToString() +"/blog/" + FriendlyURLTitle(rss.CategoryName) + "/" +  FriendlyURLTitle(rss.Title) + "-" + rss.ArticleID);
                rssWriter.WriteElementString("pubDate", rss.Date.ToString("dd.MM.yyyy"));
                rssWriter.WriteEndElement();
            }
            rssWriter.WriteEndDocument(); 
            rssWriter.Flush();
            rssWriter.Close();
            HttpContext.Current.Response.End();
        }

        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} saniye önce", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("{0} dakika önce", timeSpan.Minutes) :
                    "yaklaşık kaç dakika önce";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("{0} saat önce", timeSpan.Hours) :
                    "yaklaşık kaç saat önce";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("{0} gün önce", timeSpan.Days) :
                    "dün";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("{0} ay önce", timeSpan.Days / 30) :
                    "yaklaşık bir ay önce";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("{0} yıl önce", timeSpan.Days / 365) :
                    "yaklaşık bir yıl önce";
            }

            return result;
        }
    }
}
 