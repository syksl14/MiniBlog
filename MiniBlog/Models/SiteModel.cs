using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class SiteModel
    {
        [Required(ErrorMessage = "Lütfen geçerli bir URL giriniz.")]
        [Display(Name = "Temel Blog Adresi")]
        public string site_url { get; set; }
        [Required(ErrorMessage = "Lütfen bir blog adı giriniz.")]
        [Display(Name = "Blog Adı")]
        public string site_name { get; set; }
        [Required(ErrorMessage = "Lütfen bir blog başlık adı giriniz.")]
        [Display(Name = "Blog Başlık Adı")]
        public string site_header_name { get; set; }
        [Required(ErrorMessage = "Lütfen bir slogan giriniz.")]
        [Display(Name = "Blog Sloganı ")]
        public string slogan { get; set; } 
        [Display(Name = "Site Açıklaması ")]
        public string site_desc { get; set; }
        [Display(Name = "Anahtar Kelimeler")]
        public string site_keywords { get; set; }
    }
}