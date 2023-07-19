using System;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JSONEditor.Classes.Tools
{
    public static class Helper
    {
        public static string ToSpacedName(this string Name, bool activate)
        {
            if (activate)
            {
                StringBuilder sb = new StringBuilder();
                char previous = '.';
                for (int i = 0; i < Name.Length; i++)
                {
                    if (i != 0 && char.IsUpper(Name[i]) || char.IsDigit(Name[i]) && !char.IsDigit(previous))
                    {
                        sb.Append(" " + Name[i]);
                        previous = Name[i];
                    }
                    else
                    {
                        sb.Append(Name[i]);
                        previous = Name[i];
                    }
                }
                return sb.ToString();
            }
            else
            {
                return Name;
            }
        }

        public static bool EmptyObject(this JProperty property)
        {
            if (property.First().ToString() == "{}" && property.Last().ToString() == "{}")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AreAllNodesExpanded(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (!node.IsExpanded) return false;
                if (!AreAllNodesExpanded(node.Nodes)) return false;
            }
            return true;
        }
        public static bool AreAllNodesCollapsed(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.IsExpanded) return false;
                if (!AreAllNodesCollapsed(node.Nodes)) return false;
            }
            return true;
        }
    }

    public static class StringExtension
    {
        public static string ToTrimmedString(this object source)
        {
            return source.ToString().Trim().Replace(" ", string.Empty).Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
        }
        public static string ToLiteral(this string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }
        public static string CapFirst(this string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
        public static string RemoveFilenameExtension(this string input, string extensionname)
        {
            return input.Replace($".{extensionname}", string.Empty);
        }
    }
}