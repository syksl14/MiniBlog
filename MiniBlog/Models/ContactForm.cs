using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class ContactForm
    {
        [Required, Display(Name = "Adınız")]
        public string name { get; set; }
        [Required, Display(Name = "E-Posta Adresiniz")]
        public string email { get; set; }
        [Required, Display(Name = "Mesajınız")]
        public string message { get; set; }
    }
}