using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONEditor.Classes.Application
{
    public class FolderList
    {
        public string Name { get; set; }
        public string FolderPath { get; set; }
        public List<FileList> Files { get; set; }
    }
}