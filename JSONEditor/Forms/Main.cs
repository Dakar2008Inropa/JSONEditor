using JSONEditor.Classes.Application;
using JSONEditor.Classes.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace JSONEditor
{
    public partial class Main : Form
    {
        private Settings AppSettings { get; set; }

        private List<FileList> JsonFileList { get; set; }

        private List<FolderList> JsonFolders { get; set; }

        private JToken LoadedToken { get; set; }

        public Main()
        {
            AppSettings = SettingsHelper.Load();
            JsonFileList = new List<FileList>();
            JsonFolders = new List<FolderList>();
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SetWindowPositionAndSize();
        }

        private void SetWindowPositionAndSize()
        {
            if (AppSettings.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            Location = new Point(AppSettings.WindowPosition.X, AppSettings.WindowPosition.Y);
            Size = new Size(AppSettings.WindowSize.Width, AppSettings.WindowSize.Height);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                AppSettings.Maximized = true;
            }
            else
            {
                AppSettings.WindowPosition.X = Location.X;
                AppSettings.WindowPosition.Y = Location.Y;
                AppSettings.WindowSize.Width = Size.Width;
                AppSettings.WindowSize.Height = Size.Height;
                AppSettings.Maximized = false;
            }
            SettingsHelper.Save(AppSettings);
        }

        private void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON Files (*.json)|*.json";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                JsonFileList.Clear();
                JsonFileList.Add(new FileList() { Name = Path.GetFileNameWithoutExtension(ofd.FileName), FilePath = ofd.FileName, Token = JsonHelper.LoadJsonData(ofd.FileName) });
                UpdateFileTreeView();
            }
        }

        private void UpdateFileTreeView(bool FolderStructure = false)
        {
            MultiFileTreeView.Nodes.Clear();
            MultiFileTreeView.BeginUpdate();
            if (FolderStructure)
            {
                foreach (FolderList folder in JsonFolders)
                {
                    TreeNode tNode = new TreeNode();
                    tNode = MultiFileTreeView.Nodes[MultiFileTreeView.Nodes.Add(new TreeNode(folder.Name, 0, 0))];
                    tNode.Tag = folder;
                    foreach (FileList file in folder.Files)
                    {
                        TreeNode fNode = new TreeNode();
                        fNode = tNode.Nodes[tNode.Nodes.Add(new TreeNode(file.Name, 0, 0))];
                        fNode.Tag = file;
                    }
                }
            }
            else
            {
                foreach (FileList jsonFile in JsonFileList)
                {
                    TreeNode tNode = new TreeNode();
                    tNode = MultiFileTreeView.Nodes[MultiFileTreeView.Nodes.Add(new TreeNode(jsonFile.Name, 0, 0))];
                    tNode.Tag = jsonFile;
                }
            }
            MultiFileTreeView.EndUpdate();
        }

        private void DisplayTreeView(JToken root, string rootName)
        {
            MainTreeView.BeginUpdate();
            try
            {
                MainTreeView.Nodes.Clear();
                var tNode = MainTreeView.Nodes[MainTreeView.Nodes.Add(new TreeNode(rootName))];
                TreeNodeTagClass treeNodeTagClass = new TreeNodeTagClass();
                treeNodeTagClass.JsonObject = root;
                treeNodeTagClass.Type = root.Type.ToString();
                tNode.Tag = treeNodeTagClass;
                AddNode(root, tNode);
            }
            finally
            {
                MainTreeView.SelectedNode = MainTreeView.Nodes[0];
                MainTreeView.SelectedNode.EnsureVisible();
            }
            MainTreeView.EndUpdate();
        }

        private void AddNode(JToken token, TreeNode inTreeNode)
        {
            TreeNode childNode = new TreeNode();
            CheckToken(token, childNode, inTreeNode);
        }

        private void CheckToken(JToken token, TreeNode childNode, TreeNode inTreeNode)
        {
            if (token == null)
            {
                return;
            }
            else
            {
                if (token.HasValues)
                {
                    if (token is JObject)
                    {
                        GetJObjectProperties(childNode, inTreeNode, token);
                    }
                    else if (token is JArray)
                    {
                        GetJArray(childNode, inTreeNode, token);
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
                    }
                }
                else
                {
                    childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToTrimmedString()))];
                    TreeNodeTagClass treeNodeTag = new TreeNodeTagClass();
                    treeNodeTag.Path = token.Path;
                    treeNodeTag.JsonObject = token;
                    treeNodeTag.Type = token.Type.ToString();
                    var parentToken = token.Parent;
                    TreeNodeTagClassParent treeNodeTag_Parent = new TreeNodeTagClassParent();
                    treeNodeTag_Parent.Path = parentToken.Path;
                    treeNodeTag_Parent.Type = parentToken.Type.ToString();
                    treeNodeTag_Parent.JsonObject = parentToken;
                    treeNodeTag.Parent = treeNodeTag_Parent;
                    childNode.Tag = treeNodeTag;
                    childNode.Name = token.Path;
                }
            }
        }

        private void GetJObjectProperties(TreeNode childNode, TreeNode inTreeNode, JToken token)
        {
            var obj = (JObject)token;
            List<string> propertynameList = new List<string>()
            {
                "LineSegmentsBuilder",
                "PathValidator",
                "SegmentNames",
                "OffsetXYZTarget",
                "OffsetXYZWorld",
                "OffsetXYZSurface",
                "LinkToNeighboringSegments",
                "SegmentLength",
                "SegmentWidth",
                "StartEnd",
                "rotationAxis",
                "rotationCenter",
                "rotationMatrix",
                "axisToLargesteMeshAxisWith"
            };
            foreach (var property in obj.Properties())
            {
                var currentPropertyName = property.Name.ToSpacedName();
                if (IsEndValue(property))
                {
                    if (property.Children().FirstOrDefault().Type == JTokenType.Float)
                    {
                        childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode($"{currentPropertyName}:{float.Parse(property.Value.ToTrimmedString()).ToTrimmedString().Replace(',', '.')}"))];
                        TreeNodeTagClass treeNodeTag = new TreeNodeTagClass();
                        treeNodeTag.Name = property.Name;
                        treeNodeTag.Path = property.Path;
                        treeNodeTag.JsonObject = property;
                        treeNodeTag.Type = property.Type.ToString();
                        TreeNodeTagClassParent treeNodeTagClass_Parent = new TreeNodeTagClassParent();
                        treeNodeTagClass_Parent.Path = property.Parent.Path;
                        treeNodeTagClass_Parent.JsonObject = property.Parent;
                        treeNodeTagClass_Parent.Type = property.Parent.Type.ToString();
                        treeNodeTag.Parent = treeNodeTagClass_Parent;
                        childNode.Tag = treeNodeTag;
                        childNode.Name = property.Path;
                    }
                    else
                    {
                        TreeNodeTagClass treeNodeTag = new TreeNodeTagClass();
                        childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode($"{currentPropertyName}: {property.Value.ToString().TrimStart()}"))];
                        treeNodeTag.Name = property.Name;
                        treeNodeTag.Path = property.Path;
                        treeNodeTag.JsonObject = property;
                        treeNodeTag.Type = property.Type.ToString();
                        TreeNodeTagClassParent treeNodeTagClass_Parent = new TreeNodeTagClassParent();
                        treeNodeTagClass_Parent.Path = property.Parent.Path;
                        treeNodeTagClass_Parent.JsonObject = property.Parent;
                        treeNodeTagClass_Parent.Type = property.Parent.Type.ToString();
                        treeNodeTag.Parent = treeNodeTagClass_Parent;
                        childNode.Tag = treeNodeTag;
                        childNode.Name = property.Path;
                    }
                }
                else
                {
                    if (property.EmptyObject() == false)
                    {
                        if (propertynameList.Contains(property.Name))
                        {
                            childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name.ToSpacedName()))];
                        }
                        else
                        {
                            childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name.ToTrimmedString()))];
                        }
                        TreeNodeTagClass treeNodeTag = new TreeNodeTagClass();
                        treeNodeTag.Name = property.Name;
                        treeNodeTag.Path = property.Path;
                        treeNodeTag.JsonObject = property;
                        treeNodeTag.Type = property.Type.ToString();
                        TreeNodeTagClassParent treeNodeTagClass_Parent = new TreeNodeTagClassParent();
                        treeNodeTagClass_Parent.Path = property.Parent.Path;
                        treeNodeTagClass_Parent.JsonObject = property.Parent;
                        treeNodeTagClass_Parent.Type = property.Parent.Type.ToString();
                        treeNodeTag.Parent = treeNodeTagClass_Parent;
                        childNode.Tag = treeNodeTag;
                        childNode.Name = property.Path;
                        AddNode(property.Value, childNode);
                    }
                }
            }
        }

        private void GetJArray(TreeNode childNode, TreeNode inTreeNode, JToken token)
        {
            var array = (JArray)token;
            for (int i = 0; i < array.Count; i++)
            {
                childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToTrimmedString()))];
                TreeNodeTagClass treeNodeTag = new TreeNodeTagClass();
                treeNodeTag.Path = array[i].Path;
                treeNodeTag.JsonObject = array[i];
                treeNodeTag.Type = array[i].Type.ToString();
                TreeNodeTagClassParent treeNodeTagClass_Parent = new TreeNodeTagClassParent();
                treeNodeTagClass_Parent.Path = array[i].Parent.Path;
                treeNodeTagClass_Parent.JsonObject = array[i].Parent;
                treeNodeTagClass_Parent.Type = array[i].Parent.Type.ToString();
                treeNodeTag.Parent = treeNodeTagClass_Parent;
                childNode.Tag = treeNodeTag;
                childNode.Name = array[i].Path;
                AddNode(array[i], childNode);
            }
        }

        static bool IsEndValue(JProperty property, bool isDataSetLoader = false)
        {
            return (property.Children().Count() == 1 && (property.Children().FirstOrDefault().Type != JTokenType.Object) && (property.Children().FirstOrDefault().Type != JTokenType.Array));
        }

        private void ColorizeEditedNode(TreeNode node, Color color, Color foregroundC)
        {
            node.BackColor = color;
            node.ForeColor = foregroundC;
            var parent = node.Parent;
            for (int i = node.Level; i > 0; i--)
            {
                parent.BackColor = color;
                parent.ForeColor = foregroundC;
                parent = parent.Parent;
            }
        }

        private void MainTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var JNodeTag = this.MainTreeView.SelectedNode.Tag as TreeNodeTagClass;
            if (JNodeTag != null)
            {
                var CurrentJProp = JNodeTag.JsonObject as JProperty;
                if (CurrentJProp != null)
                {
                    var JToken = JNodeTag.JsonObject as JToken;
                    if (JToken.First().Type == JTokenType.Array)
                    {
                        MainTreeView.LabelEdit = false;
                    }
                    else
                    {
                        if (MainTreeView.SelectedNode.Nodes.Count == 0)
                        {
                            MainTreeView.LabelEdit = true;
                            if (JNodeTag.Type == "Property")
                            {
                                var JPropNode = JNodeTag.JsonObject as JProperty;
                                this.MainTreeView.SelectedNode.Text = JPropNode.Value.ToTrimmedString();
                                if (JPropNode.Value.Type != JTokenType.Boolean)
                                {
                                    this.MainTreeView.SelectedNode.BeginEdit();
                                }
                                else
                                {
                                    var node = this.MainTreeView.SelectedNode;
                                    var oldvalue = node.Text;
                                    if (this.MainTreeView.SelectedNode.Text == "True")
                                    {
                                        node.Text = $"{JPropNode.Name.ToSpacedName()}: False";
                                        JPropNode.Value = false;
                                    }
                                    else
                                    {
                                        node.Text = $"{JPropNode.Name.ToSpacedName()}: True";
                                        JPropNode.Value = true;
                                    }
                                    if (node.Text != oldvalue)
                                    {
                                        ColorizeEditedNode(node, Color.DarkOrange, Color.Black);
                                    }
                                    WriteToSelectedNode(LoadedToken);
                                }
                            }
                            else
                            {
                                if (JNodeTag.Type == "Token")
                                {
                                    var JPropToken = JNodeTag.JsonObject as JToken;
                                    if (JPropToken.Type == JTokenType.Integer)
                                    {
                                        this.MainTreeView.SelectedNode.Text = JPropToken.Value<int>().ToTrimmedString();
                                    }
                                    else if (JPropToken.Type == JTokenType.Float)
                                    {
                                        this.MainTreeView.SelectedNode.Text = JPropToken.Value<float>().ToTrimmedString();
                                    }
                                    else
                                    {
                                        this.MainTreeView.SelectedNode.Text = JPropToken.Value<string>().ToTrimmedString();
                                    }
                                    if (JPropToken.Type != JTokenType.Boolean)
                                    {
                                        this.MainTreeView.SelectedNode.BeginEdit();
                                    }
                                }
                                else
                                {
                                    var JPropArray = JNodeTag.JsonObject as JArray;
                                    if (JPropArray.Type == JTokenType.Integer)
                                    {
                                        this.MainTreeView.SelectedNode.Text = JPropArray.Value<int>().ToTrimmedString();
                                    }
                                    else if (JPropArray.Type == JTokenType.Float)
                                    {
                                        this.MainTreeView.SelectedNode.Text = JPropArray.Value<float>().ToTrimmedString();
                                    }
                                    else
                                    {
                                        this.MainTreeView.SelectedNode.Text = JPropArray.Value<string>().ToTrimmedString();
                                    }
                                    if (JPropArray.Type != JTokenType.Boolean)
                                    {
                                        this.MainTreeView.SelectedNode.BeginEdit();
                                    }
                                }
                            }
                        }
                        else
                        {
                            MainTreeView.LabelEdit = false;
                        }
                    }
                }
                else
                {
                    var CurrentObject = JNodeTag.JsonObject as JValue;
                    if (CurrentObject != null)
                    {
                        MainTreeView.LabelEdit = true;
                        this.MainTreeView.SelectedNode.BeginEdit();
                    }
                }
            }
        }

        private void MainTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var treeview = sender as TreeView;
            var node = treeview.SelectedNode;
            var JPropTag = node.Tag as TreeNodeTagClass;
            var JPropNode = JPropTag.JsonObject as JProperty;
            bool UseOldValue = false;

            if (JPropNode != null)
            {
                string oldvalue = $"{JPropNode.Name.ToSpacedName()}: {JPropNode.Value}";
                object oldJPropValue = null;
                e.CancelEdit = true;
                string label = e.Label;
                if (e.Label == null)
                {
                    UseOldValue = true;
                }
                else
                {
                    string result;
                    if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                    {
                        oldJPropValue = (float)JPropNode.Value;
                        float floatvalue = 0;
                        label = label.Contains('.') ? label.Replace('.', ',') : label;
                        if (float.TryParse(label, out floatvalue))
                        {
                            result = floatvalue.ToString().Replace(',', '.');
                            node.Text = $"{JPropNode.Name.ToSpacedName()}: {result.ToTrimmedString()}";
                            JPropNode.Value = floatvalue;
                        }
                        else
                        {
                            UseOldValue = true;
                        }
                    }
                    else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                    {
                        oldJPropValue = (int)JPropNode.Value;
                        int intvalue = 0;
                        if (int.TryParse(label, out intvalue))
                        {
                            result = intvalue.ToString();
                            node.Text = $"{JPropNode.Name.ToSpacedName()}: {result.ToTrimmedString()}";
                            JPropNode.Value = intvalue;
                        }
                        else
                        {
                            UseOldValue = true;
                        }
                    }
                    else
                    {
                        oldJPropValue = (string)JPropNode.Value;
                        node.Text = $"{JPropNode.Name.ToSpacedName()}: {label}";
                        JPropNode.Value = label;
                    }
                    if (!UseOldValue && oldvalue != node.Text)
                    {
                        ColorizeEditedNode(node, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode(LoadedToken);
                    }
                }
                if (UseOldValue)
                {
                    node.Text = oldvalue;
                    if (oldJPropValue != null)
                    {
                        if (oldJPropValue is int)
                        {
                            JPropNode.Value = (int)oldJPropValue;
                        }
                        else if (oldJPropValue is float)
                        {
                            JPropNode.Value = (float)oldJPropValue;
                        }
                        else
                        {
                            JPropNode.Value = (string)oldJPropValue;
                        }
                    }
                }
            }
            else
            {
                var JsonValue = JPropTag.JsonObject as JValue;
                if (JsonValue != null)
                {
                    node.Text = JsonValue.ToString();
                    if (JsonValue.Type == JTokenType.Integer)
                    {
                        int intvalue = 0;
                        if (int.TryParse(e.Label, out intvalue))
                        {
                            node.Text = intvalue.ToString();
                            JsonValue.Value = intvalue;
                        }
                        else
                        {
                            node.Text = JsonValue.Value.ToString();
                        }
                    }
                    else if (JsonValue.Type == JTokenType.Float)
                    {
                        float floatvalue = 0;
                        if (float.TryParse(e.Label, out floatvalue))
                        {
                            node.Text = floatvalue.ToString();
                            JsonValue.Value = floatvalue;
                        }
                        else
                        {
                            node.Text = JsonValue.Value.ToString();
                        }
                    }
                    else
                    {
                        node.Text = e.Label;
                        JsonValue.Value = e.Label;
                    }
                    ColorizeEditedNode(node, Color.DarkOrange, Color.Black);
                    WriteToSelectedNode(LoadedToken);
                }
            }
        }

        private void MainTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            TreeNode selectedNode = MainTreeView.SelectedNode;
            if (selectedNode != null && selectedNode.Level > 1)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    MainTreeView_NodeMouseDoubleClick(MainTreeView, new TreeNodeMouseClickEventArgs(selectedNode, MouseButtons.Left, 2, 0, 0));
                }
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Multiply)
                {
                    //Double up value
                    var JPropTag = selectedNode.Tag as TreeNodeTagClass;
                    var JPropNode = JPropTag.JsonObject as JProperty;
                    if (JPropNode != null)
                    {
                        if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                        {
                            float value = Convert.ToSingle(JPropNode.Value);
                            value *= 2;
                            JPropNode.Value = value;
                            selectedNode.Text = $"{JPropNode.Name.ToSpacedName()}: {value.ToTrimmedString()}";
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode(LoadedToken);
                        }
                        else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                        {
                            int value = (int)JPropNode.Value;
                            value *= 2;
                            JPropNode.Value = value;
                            selectedNode.Text = $"{JPropNode.Name.ToSpacedName()}: {value.ToTrimmedString()}";
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode(LoadedToken);
                        }
                    }
                    else
                    {
                        var JsonValue = JPropTag.JsonObject as JValue;
                        if (JsonValue != null)
                        {
                            if (JsonValue.Type == JTokenType.Integer)
                            {
                                int value = (int)JsonValue.Value;
                                value *= 2;
                                JsonValue.Value = value;
                                selectedNode.Text = value.ToString();
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode(LoadedToken);
                            }
                            else if (JsonValue.Type == JTokenType.Float)
                            {
                                float value = Convert.ToSingle(JsonValue.Value);
                                value *= 2;
                                JsonValue.Value = value;
                                selectedNode.Text = value.ToString();
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode(LoadedToken);
                            }
                        }
                    }
                }
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Divide)
                {
                    //Half the value
                    var JPropTag = selectedNode.Tag as TreeNodeTagClass;
                    var JPropNode = JPropTag.JsonObject as JProperty;
                    if (JPropNode != null)
                    {
                        if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                        {
                            float value = Convert.ToSingle(JPropNode.Value);
                            value /= 2;
                            JPropNode.Value = value;
                            selectedNode.Text = $"{JPropNode.Name.ToSpacedName()}: {value.ToTrimmedString()}";
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode(LoadedToken);
                        }
                        else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                        {
                            int value = (int)JPropNode.Value;
                            value /= 2;
                            JPropNode.Value = value;
                            selectedNode.Text = $"{JPropNode.Name.ToSpacedName()}: {value.ToTrimmedString()}";
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode(LoadedToken);
                        }
                    }
                    else
                    {
                        var JsonValue = JPropTag.JsonObject as JValue;
                        if (JsonValue != null)
                        {
                            if (JsonValue.Type == JTokenType.Integer)
                            {
                                int value = (int)JsonValue.Value;
                                value /= 2;
                                JsonValue.Value = value;
                                selectedNode.Text = value.ToString();
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode(LoadedToken);
                            }
                            else if (JsonValue.Type == JTokenType.Float)
                            {
                                float value = Convert.ToSingle(JsonValue.Value);
                                value /= 2;
                                JsonValue.Value = value;
                                selectedNode.Text = value.ToString();
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode(LoadedToken);
                            }
                        }
                    }
                }
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Add)
                {
                    var JPropTag = selectedNode.Tag as TreeNodeTagClass;
                    var JPropNode = JPropTag.JsonObject as JProperty;
                    if (JPropNode != null)
                    {
                        if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                        {
                            float value = Convert.ToSingle(JPropNode.Value);
                            value += 5;
                            JPropNode.Value = value;
                            selectedNode.Text = $"{JPropNode.Name.ToSpacedName()}: {value.ToTrimmedString()}";
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode(LoadedToken);
                        }
                        else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                        {
                            int value = (int)JPropNode.Value;
                            value += 5;
                            JPropNode.Value = value;
                            selectedNode.Text = $"{JPropNode.Name.ToSpacedName()}: {value.ToTrimmedString()}";
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode(LoadedToken);
                        }
                    }
                    else
                    {
                        var JsonValue = JPropTag.JsonObject as JValue;
                        if (JsonValue != null)
                        {
                            if (JsonValue.Type == JTokenType.Integer)
                            {
                                int value = (int)JsonValue.Value;
                                value += 5;
                                JsonValue.Value = value;
                                selectedNode.Text = value.ToString();
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode(LoadedToken);
                            }
                            else if (JsonValue.Type == JTokenType.Float)
                            {
                                float value = Convert.ToSingle(JsonValue.Value);
                                value += 5;
                                JsonValue.Value = value;
                                selectedNode.Text = value.ToString();
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode(LoadedToken);
                            }
                        }
                    }
                }
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Subtract)
                {
                    var JPropTag = selectedNode.Tag as TreeNodeTagClass;
                    var JPropNode = JPropTag.JsonObject as JProperty;
                    if (JPropNode != null)
                    {
                        if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                        {
                            float value = Convert.ToSingle(JPropNode.Value);
                            value -= 5;
                            JPropNode.Value = value;
                            selectedNode.Text = $"{JPropNode.Name.ToSpacedName()}: {value.ToTrimmedString()}";
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode(LoadedToken);
                        }
                        else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                        {
                            int value = (int)JPropNode.Value;
                            value -= 5;
                            JPropNode.Value = value;
                            selectedNode.Text = $"{JPropNode.Name.ToSpacedName()}: {value.ToTrimmedString()}";
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode(LoadedToken);
                        }
                    }
                    else
                    {
                        var JsonValue = JPropTag.JsonObject as JValue;
                        if (JsonValue != null)
                        {
                            if (JsonValue.Type == JTokenType.Integer)
                            {
                                int value = (int)JsonValue.Value;
                                value -= 5;
                                JsonValue.Value = value;
                                selectedNode.Text = value.ToString();
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode(LoadedToken);
                            }
                            else if (JsonValue.Type == JTokenType.Float)
                            {
                                float value = Convert.ToSingle(JsonValue.Value);
                                value -= 5;
                                JsonValue.Value = value;
                                selectedNode.Text = value.ToString();
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode(LoadedToken);
                            }
                        }
                    }
                }
            }
        }

        private void WriteToSelectedNode(JToken root)
        {
            TreeNode selectedNode = MultiFileTreeView.SelectedNode;
            if (selectedNode != null)
            {
                FileList selectedItem = selectedNode.Tag as FileList;
                selectedItem.EditedAfterLoad = true;
                selectedItem.Token = root;
                selectedNode.NodeFont = new Font("Verdana", 9.75f, FontStyle.Bold);
            }
        }

        private void MultiFileTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            FileList selectedNode = e.Node.Tag as FileList;
            if (selectedNode != null && selectedNode.Token != null)
            {
                LoadedToken = selectedNode.Token;
                DisplayTreeView(LoadedToken, selectedNode.Name);
            }
        }

        private void MainTree_CollapseAllMenuItem_Click(object sender, EventArgs e)
        {
            MainTreeView.CollapseAll();
        }

        private void MainTree_ExpandAllMenuItem_Click(object sender, EventArgs e)
        {
            MainTreeView.ExpandAll();
            MainTreeView.SelectedNode.EnsureVisible();
        }

        private void MultiFileTreeView_SaveSelectedNodeMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = MultiFileTreeView.SelectedNode;
            FileList selectedFileList = selectedNode.Tag as FileList;
            JsonHelper.WriteToJsonFile(selectedFileList.Token, selectedFileList.FilePath);
            selectedNode.NodeFont = new Font("Verdana", 9.75f, FontStyle.Regular);
        }

        private void OpenFolderMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder that contains the files you want to edit.";
            fbd.ShowNewFolderButton = false;
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                string[] directories = Directory.GetDirectories(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                foreach (string folder in directories)
                {
                    string[] files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        DirectoryInfo thisDir = new DirectoryInfo(folder);
                        FolderList newFolder = new FolderList();
                        newFolder.Name = thisDir.Name;
                        newFolder.FolderPath = thisDir.FullName;
                        List<FileList> thisFolderFiles = new List<FileList>();
                        foreach (string file in files)
                        {
                            FileList fl = new FileList();
                            fl.FilePath = file;
                            fl.Name = Path.GetFileNameWithoutExtension(file);
                            fl.Token = JsonHelper.LoadJsonData(file);
                            fl.EditedAfterLoad = false;
                            if (!TokenIsEmpty(fl.Token))
                            {
                                thisFolderFiles.Add(fl);
                            }
                        }
                        newFolder.Files = thisFolderFiles;
                        JsonFolders.Add(newFolder);
                    }
                }
                UpdateFileTreeView(true);
            }
        }

        public static bool TokenIsEmpty(JToken token)
        {
            return (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }
}