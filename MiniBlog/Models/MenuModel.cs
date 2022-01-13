using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class MenuModel
    {
        public List<Pages_V> Pages { get; set; }
        public string ListItemClass { get; set; }
        public string TextItemClass { get; set; }
    }
}