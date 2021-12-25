using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class AdvancedModel
    {
        public Boolean reCaptcha_isEnable { get; set; }
        [Required(ErrorMessage = "Lütfen site anahtarı giriniz.")]
        [Display(Name = "Site Anahtarı")]
        public String reCaptcha_siteKey { get; set; }
        [Required(ErrorMessage = "Lütfen gizli anahtarı giriniz.")]
        [Display(Name = "Gizli Anahtarı")]
        public String reCaptcha_hiddenKey { get; set; }
    }
}