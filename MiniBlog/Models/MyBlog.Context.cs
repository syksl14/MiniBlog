﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MiniBlog.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyBlogEntities : DbContext
    {
        public MyBlogEntities()
            : base("name=MyBlogEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Article_V> Article_V { get; set; }
        public virtual DbSet<Articles_V> Articles_V { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Revision> Revisions { get; set; }
        public virtual DbSet<Revisions_V> Revisions_V { get; set; }
        public virtual DbSet<Users_V> Users_V { get; set; }
    }
}