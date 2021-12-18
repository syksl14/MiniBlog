using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class ArticleContext : DbContext
    {
        public ArticleContext() : base("MyBlogEntities")
        {

        }
        public DbSet<Models.Article> Article { get; set; } 
        public DbSet<Models.Revision> Revision { get; set; } 
        public DbSet<Page> Page { get; set; }
    }
}