using MiniBlog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MiniBlog.Views
{
    public class SiteSettingsController : Controller
    {
        SystemBase sysBase = new SystemBase();
        // GET: SiteSettings
        [_SessionControl]
        public ActionResult Index()
        {
            return View();
        }

        [_SessionControl]
        public ActionResult General()
        {
            SiteModel model = new SiteModel();
            model.site_url = ConfigurationManager.AppSettings["site_url"].ToString();
            model.site_header_name = ConfigurationManager.AppSettings["site_header_name"].ToString();
            model.site_keywords = ConfigurationManager.AppSettings["site_keywords"].ToString();
            model.site_desc = ConfigurationManager.AppSettings["site_desc"].ToString();
            model.slogan = ConfigurationManager.AppSettings["slogan"].ToString();
            model.site_name = ConfigurationManager.AppSettings["site_name"].ToString();
            return PartialView("_General", model);
        }

        [_SessionControl]
        [Route("SiteSettings/General/Save")]
        [HttpPost]
        public ActionResult GeneralSave(SiteModel model)
        {
            if (ModelState.IsValid)
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
                config.AppSettings.Settings["site_name"].Value = model.site_name;
                config.AppSettings.Settings["site_desc"].Value = model.site_desc;
                config.AppSettings.Settings["site_header_name"].Value = model.site_header_name;
                config.AppSettings.Settings["site_keywords"].Value = model.site_keywords;
                config.AppSettings.Settings["site_url"].Value = model.site_url;
                config.AppSettings.Settings["slogan"].Value = model.slogan;
                config.Save(ConfigurationSaveMode.Modified);
                return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
            }
        }

        [_SessionControl]
        public ActionResult Email()
        {
            EmailModel model = new EmailModel();
            model.Mail_Host = ConfigurationManager.AppSettings["Mail_Host"].ToString();
            model.Mail_From = ConfigurationManager.AppSettings["Mail_From"].ToString();
            model.Mail_IsEnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["Mail_IsEnableSSL"]);
            model.Mail_Password = ConfigurationManager.AppSettings["Mail_Password"].ToString();
            model.Mail_UserName = ConfigurationManager.AppSettings["Mail_UserName"].ToString();
            model.Mail_Address = ConfigurationManager.AppSettings["Mail_Address"].ToString();
            model.Mail_Port = Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Port"].ToString());
            return PartialView("_Email", model);
        }

        [_SessionControl]
        [Route("SiteSettings/EmailServer/Save")]
        [HttpPost]
        public async Task<ActionResult> EmailServerSave(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseMessage response = new ResponseMessage();
                response = await Helper.MailSend("Sınama E-Postası", "Eğer bu mesajı görüyorsanız, e-posta sunucu ayarları başarıyla kaydedildi.", model);
                if (response.Result == "OK")
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
                    config.AppSettings.Settings["Mail_Host"].Value = model.Mail_Host;
                    config.AppSettings.Settings["Mail_From"].Value = model.Mail_From;
                    config.AppSettings.Settings["Mail_IsEnableSSL"].Value = model.Mail_IsEnableSSL.ToString();
                    config.AppSettings.Settings["Mail_Password"].Value = model.Mail_Password;
                    config.AppSettings.Settings["Mail_UserName"].Value = model.Mail_UserName;
                    config.AppSettings.Settings["Mail_Port"].Value = model.Mail_Port.ToString();
                    config.AppSettings.Settings["Mail_Address"].Value = model.Mail_Address.ToString();
                    config.Save(ConfigurationSaveMode.Modified);
                    return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", "Ayarlar Kaydedilemedi. E-Posta sınaması başarısız oldu!<br>" + response.Error);
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
            }
        }

        [_SessionControl]
        public ActionResult Advanced()
        {
            AdvancedModel model = new AdvancedModel();
            model.reCaptcha_isEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["reCaptcha_isEnable"]);
            model.reCaptcha_siteKey = ConfigurationManager.AppSettings["reCaptcha_siteKey"].ToString();
            model.reCaptcha_hiddenKey = ConfigurationManager.AppSettings["reCaptcha_hiddenKey"].ToString();
            model.Themes = sysBase.Themes();
            return PartialView("_Advanced", model);
        }

        [_SessionControl]
        [Route("SiteSettings/Advanced/Theme/PreviewImage/{path}")]
        public ActionResult PreviewThemeImage(String path)
        {
            byte[] image = System.IO.File.ReadAllBytes(Server.MapPath(Helper.Base64Decode(path) + "/preview.jpg"));
            return base.File(image, "image/jpeg");
        }

        [_SessionControl]
        [Route("SiteSettings/Advanced/Save")]
        [HttpPost]
        public ActionResult AdvancedSave(AdvancedModel model)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
            if (model.reCaptcha_isEnable)
            {
                if (ModelState.IsValid)
                {
                    config.AppSettings.Settings["reCaptcha_siteKey"].Value = model.reCaptcha_siteKey;
                    config.AppSettings.Settings["reCaptcha_hiddenKey"].Value = model.reCaptcha_hiddenKey;
                }
                else
                {
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                config.AppSettings.Settings["reCaptcha_siteKey"].Value = "";
                config.AppSettings.Settings["reCaptcha_hiddenKey"].Value = "";
            }
            config.AppSettings.Settings["reCaptcha_isEnable"].Value = model.reCaptcha_isEnable.ToString();
            config.AppSettings.Settings["site_theme_main"].Value = Helper.Base64Decode(model.Theme) + "/_Layout.cshtml";
            config.AppSettings.Settings["site_theme_page"].Value = Helper.Base64Decode(model.Theme) + "/_LayoutDetail.cshtml";
            config.AppSettings.Settings["site_theme"].Value = Helper.Base64Decode(model.Theme);
            config.Save(ConfigurationSaveMode.Modified);
            return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}