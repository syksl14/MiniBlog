//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Pages_V
    {
        public int PageID { get; set; }
        public int Crud { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string FriendlyName { get; set; }
        public string CoverPhoto { get; set; }
        public string Privacy { get; set; }
        public int PageOrder { get; set; }
        public System.DateTime Date { get; set; }
        public int CrudAuthorID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int AuthorID { get; set; }
        public string UserName { get; set; }
        public string ProfilePicture { get; set; }
        public string HeaderText { get; set; }
    }
}
