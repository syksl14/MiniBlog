using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniBlog.Models
{
    public class PageModel
    {
        public int PageID { get; set; }
        [Required(ErrorMessage = "Lütfen bir başlık giriniz.")]
        [Display(Name = "Sayfa Başlığı")]
        public string Title { get; set; } 
        [Display(Name = "Sayfa Alt Başlığı")]
        public string HeaderText { get; set; }
        [Required(ErrorMessage = "Lütfen sayfa içeriğini giriniz.")]
        [Display(Name = "Sayfa")]
        [AllowHtml]
        public string Page { get; set; }
        [Required(ErrorMessage = "Lütfen sayfa ile ilgili etiketler giriniz.")]
        [Display(Name = "Etiketler")]
        public string Keywords { get; set; }
        [Required(ErrorMessage = "Lütfen kullanıcı dostu bir url adı giriniz.")]
        [Display(Name = "Kullanıcı Dostu URL Adı")]
        public string FriendlyName { get; set; }
        public string CoverPhoto { get; set; }
        public string Privacy { get; set; }
        [Required(ErrorMessage = "Lütfen bir sayfa sırası giriniz.")]
        [Display(Name = "Sayfa Sırası")]
        public int PageOrder { get; set; }
    }
}