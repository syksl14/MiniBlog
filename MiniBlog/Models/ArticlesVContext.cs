using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiniBlog.Controllers
{
    public class ArticlesVContext : DbContext
    {
        public ArticlesVContext() : base("MyBlogEntities")
        {

        }
        public DbSet<Models.Articles_V> Articles_V { get; set; }
        public DbSet<Models.Article_V> Article_V { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Revisions_V> Revisions_V { get; set; }
    }
}