using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class FolderModel
    {
        public int FolderID { get; set; }
        [Required(ErrorMessage = "Lütfen bir klasör adı giriniz.")]
        [Display(Name = "Klasör Adı")]
        public String FolderName { get; set; }
        public int ParentFolderID { get; set; }
        [Display(Name = "Açıklama")]
        public String FolderDescription { get; set; }
        [Display(Name = "İzin Verilen Dosya Türleri (Örn: .jpeg,.jpg,.tiff)")]
        public String FolderFileTypes { get; set; }

    }
}