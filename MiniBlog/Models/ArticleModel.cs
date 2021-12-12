using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniBlog.Models
{
    public class ArticleModel
    {
        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Lütfen bir başlık giriniz.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Lütfen bir özet giriniz.")]
        [Display(Name = "Özet")]
        public string Summary { get; set; }
        [Required(ErrorMessage = "Lütfen yazınızı giriniz.")]
        [Display(Name = "Blog Yazısı")]
        [AllowHtml]
        public string Article { get; set; }
        [Required(ErrorMessage = "Lütfen konu ile ilgili etiketler giriniz.")]
        [Display(Name = "Etiketler")]
        public string Keywords{ get; set; }
        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Lütfen kategori seçiniz.")]
        public int? CategoryID { get; set; }
        public string CoverPhoto { get; set; } 
        public string Privacy { get; set; }
        public int RevisionCount { get; set; }
        public List<Models.Revision> Revisions { get; set; }
    }
}