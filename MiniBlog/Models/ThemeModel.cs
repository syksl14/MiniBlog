using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class ThemeModel
    {
        public String Name { get; set; } 
        public String Designer { get; set; }
        public String Version { get; set; }
        public String SupportURL { get; set; }
        public String Path { get; set; }
        public String ReleaseDate { get; set; }

        public bool isActive { get; set; }
    }
}