using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONEditor.Classes.Application
{
    public class CopyClass
    {
        public object ValueToCopy { get; set; }
        public JTokenType ValueToCopyType { get; set; }
        public int NumberOfPathsRecieved { get; set; }
    }
}