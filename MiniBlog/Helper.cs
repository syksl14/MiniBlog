using MiniBlog.Controllers;
using MiniBlog.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MiniBlog
{
    public static class Helper
    {

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
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
                string encodedUrl = (incomingText ?? "").ToLower();
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");
                encodedUrl = encodedUrl.Replace("'", "");
                encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");
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
                string encodedUrl = (incomingText ?? "").ToLower();
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");
                encodedUrl = encodedUrl.Replace("'", "");
                encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");
                encodedUrl = encodedUrl.Trim('-');
                return encodedUrl;
            }
            else
            {
                return "";
            }
        }

        public static string FriendlyURL(this UrlHelper urlHelper, string incomingText)
        {

            if (incomingText != null)
            {
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
                incomingText = incomingText.Trim();
                incomingText = Regex.Replace(incomingText, @"\&+", "and");
                incomingText = incomingText.Replace("'", "");
                incomingText = Regex.Replace(incomingText, @"-+", "-");
                incomingText = incomingText.Trim('-');
                return incomingText;
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
            rssWriter.WriteElementString("copyright", "(c) " + DateTime.Now.Year + ", SelahattinYuksel.NET");
            rssWriter.WriteElementString("pubDate", DateTime.Now.ToString("dd.MM.yyyy"));
            rssWriter.WriteElementString("language", "tr-TR");
            rssWriter.WriteElementString("webMaster", "iletisim@selahattinyuksel.net");
            ArticlesVContext db = new ArticlesVContext();
            var articles = from e in db.Articles_V where e.Privacy == "P" orderby e.ArticleID descending select e;
            foreach (var rss in articles.ToList().ToPagedList(1, 10))
            {
                rssWriter.WriteStartElement("item");
                rssWriter.WriteElementString("title", rss.Title);
                rssWriter.WriteElementString("description", rss.Summary);
                rssWriter.WriteElementString("link", ConfigurationManager.AppSettings["site_url"].ToString() + "/blog/" + FriendlyURLTitle(rss.CategoryName) + "/" + FriendlyURLTitle(rss.Title) + "-" + rss.ArticleID);
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

        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public async static Task<ResponseMessage> MailSend(String title, String body, EmailModel model)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                if (model == null)
                {
                    model = new EmailModel();
                    model.Mail_Host = ConfigurationManager.AppSettings["Mail_Host"].ToString();
                    model.Mail_Port = Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Port"]);
                    model.Mail_IsEnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["Mail_IsEnableSSL"]);
                    model.Mail_From = ConfigurationManager.AppSettings["Mail_From"].ToString();
                    model.Mail_UserName = ConfigurationManager.AppSettings["Mail_UserName"].ToString();
                    model.Mail_Password = ConfigurationManager.AppSettings["Mail_Password"].ToString();
                }
                var message = new MailMessage();
                message.From = new MailAddress(model.Mail_UserName);
                message.To.Add(new MailAddress(model.Mail_From));
                message.Subject = title + " - " + ConfigurationManager.AppSettings["site_header_name"].ToString();
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = model.Mail_UserName,
                        Password = model.Mail_Password
                    };
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Host = model.Mail_Host;
                    smtp.Port = model.Mail_Port;
                    smtp.EnableSsl = model.Mail_IsEnableSSL;
                    smtp.Timeout = 5000;
                    await smtp.SendMailAsync(message);
                    response.Result = "OK";
                }
            }
            catch (Exception err)
            {
                response.Result = "Error";
                if(err.InnerException != null)
                {
                    response.Error = err.InnerException.Message + " " + err.InnerException.InnerException.Message;
                }
                else
                {
                    response.Error = err.Message;
                }
            }
            return response;
        }

    }
}
