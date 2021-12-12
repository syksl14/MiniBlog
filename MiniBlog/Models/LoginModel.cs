using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [Display(Name = "E-Posta Adresi")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}