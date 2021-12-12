using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class CategoryContext : DbContext
    {
        public CategoryContext() : base("MyBlogEntities")
        {

        }
        public DbSet<Category> Category { get; set; }
    }
}