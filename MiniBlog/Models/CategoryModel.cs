using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class CategoryModel
    {
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Lütfen bir kategori adı giriniz.")]
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }
    }
}