using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class AdminContext : DbContext
    {
        public AdminContext(): base("MyBlogEntities")
        {

        }
        public DbSet<Models.Pages_V> Pages_V { get; set; }
        public DbSet<Models.Page> Pages { get; set; }
        public DbSet<Models.Folder> Folder { get; set; }
        public DbSet<Models.Folders_V> Folders_V { get; set; }
        public DbSet<Models.File> File { get; set; }
        public DbSet<Models.Files_V> Files_V { get; set; }
    }
}