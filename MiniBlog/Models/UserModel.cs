using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class UserModel
    {
        public int AuthorID { get; set; }
        [Required(ErrorMessage = "Lütfen benzersiz bir kullanıcı adı giriniz.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Lütfen adınızı giriniz.")]
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Lütfen soyadınızı giriniz.")]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [Display(Name = "E-Posta Adresi")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        [Display(Name = "Seviye")]
        [Required(ErrorMessage = "Lütfen kullanıcı seviyesi seçiniz.")]
        public int AuthorityLevel { get; set; }
    }
}