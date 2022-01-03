using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniBlog.Models
{
    public class MediaModel
    {
        public List<Files_V> Files { get; set; }
        public List<Folders_V> Folders { get; set; }
        public int FolderID { get; set; }
        public int ParentFolderID { get; set; }
    }
}