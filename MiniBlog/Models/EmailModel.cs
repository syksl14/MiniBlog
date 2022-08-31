﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class EmailModel
    {
        [Required(ErrorMessage = "Lütfen geçerli bir alıcı e-posta adresi giriniz.")]
        [Display(Name = "Alıcı E-Posta Adresi")]
        public string Mail_From { get; set; }

        [Required(ErrorMessage = "Lütfen bir e-posta sunucusu giriniz.")]
        [Display(Name = "E-Posta Sunucusu (Giden)")]
        public string Mail_Host { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta sunucusunun port numarasını giriniz.")]
        [Display(Name = "Sunucu Portu (Giden)")]
        public int Mail_Port { get; set; } 
        public Boolean Mail_IsEnableSSL { get; set; }
        [Required(ErrorMessage = "Lütfen e-posta adını giriniz.")]
        [Display(Name = "E-Posta Adresi")]
        public string Mail_Address { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta kullanıcı adını giriniz.")]
        [Display(Name = "Kullanıcı adı")]
        public string Mail_UserName { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta şifrenizi giriniz.")]
        [Display(Name = "Şifre")]
        public string Mail_Password { get; set; }
    }
}