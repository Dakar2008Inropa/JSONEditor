using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONEditor.Classes.Application
{
    public class TreeNodeTagClass
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public object JsonObject { get; set; }
        public TreeNodeTagClassParent Parent { get; set; }
        public object NormalValue { get; set; }
        public object PatchedValue { get; set; }
        public object ToolName { get; set; }
        public object ToolNumber { get; set; }
        public bool UsesEnum { get; set; }
        public bool IsToolName { get; set; }
    }
}