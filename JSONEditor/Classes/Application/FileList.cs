using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONEditor.Classes.Application
{
    public class FileList
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool EditedAfterLoad { get; set; }
        public JToken Token { get; set; }
    }
}