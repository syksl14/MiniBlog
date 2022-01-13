using MiniBlog.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace MiniBlog
{
    public class SystemBase
    {
        protected String themeDirectory = "~/Views/Shared/Themes/";
        private String defaultThemeDirectory = "";
        public List<ThemeModel> Themes()
        {
            var themes = Directory.GetDirectories(HttpContext.Current.Server.MapPath(getThemeContext()));
            List<ThemeModel> themeList = new List<ThemeModel>();
            foreach (var theme in themes)
            {
                FileInfo dir = new FileInfo(theme);
                ThemeModel t = new ThemeModel();
                t.Path = themeDirectory + dir.Name; 
                String themeDataJSON = Helper.FileRead(HttpContext.Current.Server.MapPath(getThemeContext() + dir.Name + "/theme.json"));
                JToken obj = JObject.Parse(themeDataJSON); 
                t.Name = (string)obj["Name"];
                t.Designer = (string)obj["Designer"];
                t.ReleaseDate = (string)obj["ReleaseDate"];
                t.SupportURL = (string)obj["SupportURL"];
                t.Version = (string)obj["Version"];
                if (ConfigurationManager.AppSettings["site_theme_main"].ToString().Contains(dir.Name))
                {
                    t.isActive = true;
                }
                themeList.Add(t);
            }
            return themeList;
        }
        public String getCurrentThemeContext()
        {
            this.themeDirectory = ConfigurationManager.AppSettings["site_theme"].ToString();
            return this.themeDirectory;
        }
        public String getThemeContext()
        {
            return this.themeDirectory;
        }
    }
}