using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONEditor.Classes.RecentFiles
{
    public class RecentFiles
    {
        public string DisplayName { get; set; }
        public string Path { get; set; }
        public long Added { get; set; }
    }
}