using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class AuthorsContext : DbContext
    {
        public AuthorsContext() : base("MyBlogEntities")
        {

        }
        public DbSet<Author> User { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Users_V> Users { get; set; }
        public DbSet<Pages_V> Pages { get; set; }
    }
}