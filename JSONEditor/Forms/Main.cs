using JSONEditor.Classes.Application;
using JSONEditor.Classes.RecentFiles;
using JSONEditor.Classes.RecentFolders;
using JSONEditor.Forms;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;

namespace JSONEditor
{
    public partial class Main : Form
    {
        private Settings AppSettings { get; set; }

        private List<FileList> JsonFileList { get; set; }

        private JToken LoadedToken { get; set; }

        private JToken LoadedUnEditedToken { get; set; }

        private bool IsNodeEdited { get; set; }

        private TreeNode lastEditedNode { get; set; }

        private List<RecentFiles> RecentFilesList { get; set; }

        private List<RecentFolders> RecentFoldersList { get; set; }

        private BackgroundWorker batchWorker = new BackgroundWorker();

        public Main()
        {
            AppSettings = SettingsHelper.Load();
            JsonFileList = new List<FileList>();
            IsNodeEdited = false;
            lastEditedNode = null;
            LoadedUnEditedToken = null;
            RecentFilesList = RecentFilesHelper.Load();
            RecentFoldersList = RecentFoldersHelper.Load();

            batchWorker.WorkerReportsProgress = true;
            batchWorker.DoWork += BatchWorker_DoWork;
            batchWorker.RunWorkerCompleted += batchWorker_RunWorkerCompleted;
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SetWindowPositionAndSize();
            SetSettingsValues();
            EnableDisableTreeViewContextMenu();
            PopulateRecentFiles();
            PopulateRecentFolders();
        }

        private void SetWindowPositionAndSize()
        {
            if (AppSettings.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            Location = new Point(AppSettings.WindowPosition.X, AppSettings.WindowPosition.Y);
            Size = new Size(AppSettings.WindowSize.Width, AppSettings.WindowSize.Height);
            splitContainer1.SplitterDistance = AppSettings.SplitterDistance;
        }

        private void SetSettingsValues()
        {
            SettingsSpacedLabelsMenuItem.Checked = AppSettings.SpacedLabels;
        }

        private bool IsTreeViewEmpty(TreeView view)
        {
            if (view.Nodes.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void EnableDisableTreeViewContextMenu()
        {
            if (IsTreeViewEmpty(MainTreeView))
            {
                MainTreeView.ContextMenuStrip = null;
            }
            else
            {
                MainTreeView.ContextMenuStrip = MainTreeContextMenu;
            }

            if (IsTreeViewEmpty(MultiFileTreeView))
            {
                MultiFileTreeView.ContextMenuStrip = null;
            }
            else
            {
                MultiFileTreeView.ContextMenuStrip = MultiFileTreeViewContextMenu;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsNodeEdited)
            {
                var selectedNode = lastEditedNode;
                FileList selectedFileList = selectedNode.Tag as FileList;
                var result = MessageBox.Show($"You have unsaved changes to {selectedNode.Text}\nDo you want to save them?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes || result == DialogResult.No)
                {
                    if (result == DialogResult.Yes)
                    {
                        JsonHelper.WriteToJsonFile(LoadedToken, selectedFileList.FilePath);
                    }
                    selectedFileList.EditedAfterLoad = false;
                    IsNodeEdited = false;
                    selectedNode.ForeColor = Color.Black;
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            SaveSettings();
        }

        private static TreeNode[] GetDirectoryNodes(DirectoryInfo directoryInfo, string fileFilter, Form mainform)
        {
            var nodes = new List<TreeNode>();
            mainform.Invoke((MethodInvoker)delegate
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    if (DirectoryContainsFile(directory, fileFilter))
                    {
                        var subNodes = GetDirectoryNodes(directory, fileFilter, mainform);
                        nodes.Add(new TreeNode(directory.Name, 1, 1, subNodes));
                        mainform.Text = $"Inropa JSON Editor - Loading Data - {directory.Name}";
                    }
                }
                foreach (var file in directoryInfo.GetFiles(fileFilter))
                {
                    TreeNode fileNode = new TreeNode(file.Name, 0, 0);
                    FileList jsonFile = new FileList();
                    jsonFile.Name = file.Name;
                    jsonFile.FilePath = file.FullName;
                    mainform.Text = $"Inropa JSON Editor - Loading Data - {jsonFile.Name}";
                    fileNode.Tag = jsonFile;

                    nodes.Add(fileNode);
                }
            });
            return nodes.ToArray();
        }

        private static bool DirectoryContainsFile(DirectoryInfo directory, string fileFilter)
        {
            // Check if the current directory contains the file
            if (directory.GetFiles(fileFilter).Length > 0)
            {
                return true;
            }

            // Recursively check if any of the subdirectories contain the file
            foreach (var subDirectory in directory.GetDirectories())
            {
                if (DirectoryContainsFile(subDirectory, fileFilter))
                {
                    return true;
                }
            }

            // If no file was found in this directory or any subdirectory, return false
            return false;
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
            AppSettings.SplitterDistance = splitContainer1.SplitterDistance;
            AppSettings.SpacedLabels = SettingsSpacedLabelsMenuItem.Checked;
            SettingsHelper.Save(AppSettings);
        }

        private void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON Files (*.json)|*.json";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                JsonFileList.Clear();
                MultiFileTreeView.Nodes.Clear();
                MainTreeView.Nodes.Clear();
                foreach (var file in ofd.FileNames)
                {
                    FileList flist = new FileList();
                    flist.Name = Path.GetFileNameWithoutExtension(file);
                    flist.FilePath = file;
                    JsonFileList.Add(flist);

                    if (!RecentFilesList.Exists(x => x.Path == flist.FilePath))
                    {
                        if (RecentFilesList.Count == 10)
                        {
                            RecentFilesList.RemoveAt(0);
                        }
                        RecentFiles newRecentFile = new RecentFiles();
                        newRecentFile.DisplayName = flist.Name;
                        newRecentFile.Path = flist.FilePath;
                        newRecentFile.Added = DateTime.Now.ToFileTimeUtc();
                        RecentFilesList.Add(newRecentFile);
                    }
                }
                RecentFilesHelper.Save(RecentFilesList);
                PopulateRecentFiles();
                UpdateFileTreeView(null, null);
            }
        }

        private void PopulateRecentFiles()
        {
            RecentFilesMenuItem.DropDownItems.Clear();
            foreach (var file in RecentFilesList)
            {
                ToolStripMenuItem recentFile = new ToolStripMenuItem();
                recentFile.Text = file.DisplayName;
                recentFile.ToolTipText = file.Path;
                recentFile.Image = Properties.Resources.Fatcow_Farm_Fresh_Json_16;
                recentFile.Tag = file.Path;
                recentFile.Click += RecentFile_Click;
                RecentFilesMenuItem.DropDownItems.Add(recentFile);
            }
        }

        private void RecentFile_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            JsonFileList.Clear();
            FileList flist = new FileList();
            flist.Name = Path.GetFileNameWithoutExtension(toolStripMenuItem.Tag.ToString());
            flist.FilePath = toolStripMenuItem.Tag.ToString();
            JsonFileList.Add(flist);
            MultiFileTreeView.Nodes.Clear();
            MainTreeView.Nodes.Clear();
            UpdateFileTreeView(null, null);
        }

        private void PopulateRecentFolders()
        {
            RecentFoldersMenuItem.DropDownItems.Clear();
            foreach (var folder in RecentFoldersList)
            {
                ToolStripMenuItem recentFolder = new ToolStripMenuItem();
                recentFolder.Text = folder.DisplayName;
                recentFolder.ToolTipText = folder.Path;
                recentFolder.Tag = folder.Path;
                recentFolder.Image = Properties.Resources.Custom_Icon_Design_Flatastic_1_Folder_48;
                recentFolder.Click += RecentFolder_Click;
                RecentFoldersMenuItem.DropDownItems.Add(recentFolder);
            }
        }

        private void RecentFolder_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            MultiFileTreeView.Nodes.Clear();
            MainTreeView.Nodes.Clear();
            UpdateFileTreeView(toolStripMenuItem.Tag.ToString(), "*.json", true);
        }

        private async void UpdateFileTreeView(string root, string fileFilter, bool FolderStructure = false)
        {
            MultiFileTreeView.Nodes.Clear();
            MultiFileTreeView.BeginUpdate();
            if (FolderStructure && !string.IsNullOrEmpty(root) && !string.IsNullOrEmpty(fileFilter))
            {
                var rootDirInfo = new DirectoryInfo(root);
                await Task.Run(() =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        FileMenuItem.Enabled = false;
                        this.Text = "Inropa JSON Editor - Loading Data...";
                    });
                    TreeNode[] Nodes = GetDirectoryNodes(rootDirInfo, fileFilter, this);
                    MultiFileTreeView.Invoke(new Action(() =>
                    {
                        MultiFileTreeView.Nodes.Add(new TreeNode(rootDirInfo.Name, 2, 2, Nodes));
                    }));
                });
                this.Text = "Inropa JSON Editor";
                FileMenuItem.Enabled = true;
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
            MultiFileTreeView.Nodes[0].Expand();
            EnableDisableTreeViewContextMenu();
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
                var currentPropertyName = property.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked);
                if (IsEndValue(property))
                {
                    if (property.Children().FirstOrDefault().Type == JTokenType.Float)
                    {
                        childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode($"{currentPropertyName}:{float.Parse(property.Value.ToTrimmedString()).ToTrimmedString().Replace(',', '.')}"))];
                        TreeNodeTagClass treeNodeTag = new TreeNodeTagClass();
                        treeNodeTag.Name = property.Name;
                        treeNodeTag.NameSpaced = property.Name.ToSpacedName(true);
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
                        treeNodeTag.NameSpaced = property.Name.ToSpacedName(true);
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
                            childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)))];
                        }
                        else
                        {
                            childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name.ToTrimmedString()))];
                        }
                        TreeNodeTagClass treeNodeTag = new TreeNodeTagClass();
                        treeNodeTag.Name = property.Name;
                        treeNodeTag.NameSpaced = property.Name.ToSpacedName(true);
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
            if (InvokeRequired)
            {
                Invoke(new Action(() => ColorizeEditedNode(node, color, foregroundC)));
                return;
            }
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
                                        node.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: False";
                                        JPropNode.Value = false;
                                    }
                                    else
                                    {
                                        node.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: True";
                                        JPropNode.Value = true;
                                    }
                                    if (node.Text != oldvalue)
                                    {
                                        ColorizeEditedNode(node, Color.DarkOrange, Color.Black);
                                    }
                                    WriteToSelectedNode();
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
                string oldvalue = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {JPropNode.Value}";
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
                            node.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {result.ToTrimmedString()}";
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
                            node.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {result.ToTrimmedString()}";
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
                        node.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {label}";
                        JPropNode.Value = label;
                    }
                    if (!UseOldValue && oldvalue != node.Text)
                    {
                        ColorizeEditedNode(node, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
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
                    WriteToSelectedNode();
                }
            }
        }

        private void MainTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            TreeNode selectedNode = MainTreeView.SelectedNode;
            if (selectedNode != null && selectedNode.Level > 0)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    MainTreeView_NodeMouseDoubleClick(MainTreeView, new TreeNodeMouseClickEventArgs(selectedNode, MouseButtons.Left, 2, 0, 0));
                }
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    if (e.KeyCode == Keys.Multiply)
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
                                selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode();
                            }
                            else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                            {
                                int value = (int)JPropNode.Value;
                                value *= 2;
                                JPropNode.Value = value;
                                selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode();
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
                                    WriteToSelectedNode();
                                }
                                else if (JsonValue.Type == JTokenType.Float)
                                {
                                    float value = Convert.ToSingle(JsonValue.Value);
                                    value *= 2;
                                    JsonValue.Value = value;
                                    selectedNode.Text = value.ToString();
                                    ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                    WriteToSelectedNode();
                                }
                            }
                        }
                    }
                    if (e.KeyCode == Keys.Divide)
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
                                selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode();
                            }
                            else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                            {
                                int value = (int)JPropNode.Value;
                                value /= 2;
                                JPropNode.Value = value;
                                selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode();
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
                                    WriteToSelectedNode();
                                }
                                else if (JsonValue.Type == JTokenType.Float)
                                {
                                    float value = Convert.ToSingle(JsonValue.Value);
                                    value /= 2;
                                    JsonValue.Value = value;
                                    selectedNode.Text = value.ToString();
                                    ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                    WriteToSelectedNode();
                                }
                            }
                        }
                    }
                    if (e.KeyCode == Keys.Add)
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
                                selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode();
                            }
                            else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                            {
                                int value = (int)JPropNode.Value;
                                value += 5;
                                JPropNode.Value = value;
                                selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode();
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
                                    WriteToSelectedNode();
                                }
                                else if (JsonValue.Type == JTokenType.Float)
                                {
                                    float value = Convert.ToSingle(JsonValue.Value);
                                    value += 5;
                                    JsonValue.Value = value;
                                    selectedNode.Text = value.ToString();
                                    ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                    WriteToSelectedNode();
                                }
                            }
                        }
                    }
                    if (e.KeyCode == Keys.Subtract)
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
                                selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode();
                            }
                            else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                            {
                                int value = (int)JPropNode.Value;
                                value -= 5;
                                JPropNode.Value = value;
                                selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                WriteToSelectedNode();
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
                                    WriteToSelectedNode();
                                }
                                else if (JsonValue.Type == JTokenType.Float)
                                {
                                    float value = Convert.ToSingle(JsonValue.Value);
                                    value -= 5;
                                    JsonValue.Value = value;
                                    selectedNode.Text = value.ToString();
                                    ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                                    WriteToSelectedNode();
                                }
                            }
                        }
                    }
                    if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    {
                        MainTreeView.LabelEdit = true;
                        var selectedNodeTag = selectedNode.Tag as TreeNodeTagClass;
                        var JPropToken = selectedNodeTag.JsonObject as JToken;
                        var JProp = selectedNodeTag.JsonObject as JProperty;
                        var oldvalue = selectedNode.Text;
                        if (JPropToken.FirstOrDefault().Type == JTokenType.Integer || JPropToken.FirstOrDefault().Type == JTokenType.Float)
                        {
                            if (JPropToken.FirstOrDefault().Value<int>() > 0)
                            {
                                JPropToken.FirstOrDefault().Replace(JPropToken.FirstOrDefault().Value<int>() * -1);
                            }
                            else if (JPropToken.FirstOrDefault().Value<int>() < 0)
                            {
                                JPropToken.FirstOrDefault().Replace(JPropToken.FirstOrDefault().Value<int>() * -1);
                            }
                            selectedNode.Text = $"{JProp.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {JPropToken.FirstOrDefault().Value<string>()}";
                            if (selectedNode.Text != oldvalue)
                            {
                                ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            }
                            WriteToSelectedNode();
                        }
                    }
                }
            }
        }

        private void WriteToSelectedNode()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(WriteToSelectedNode));
                return;
            }

            TreeNode selectedNode = MultiFileTreeView.SelectedNode;
            if (selectedNode != null)
            {
                FileList selectedItem = selectedNode.Tag as FileList;
                selectedItem.EditedAfterLoad = true;
                IsNodeEdited = true;
                lastEditedNode = selectedNode;
                selectedNode.ForeColor = Color.Red;
            }
        }

        private void MultiFileTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            FileList selectedNode = e.Node.Tag as FileList;
            if (selectedNode != null)
            {
                LoadedToken = JsonHelper.LoadJsonData(selectedNode.FilePath);
                LoadedUnEditedToken = JsonHelper.LoadJsonData(selectedNode.FilePath);
                if (TokenIsEmpty(LoadedToken))
                {
                    if (MessageBox.Show($"JSON file is empty or invalid and can't be opened here, do you wish to remove the file from the treeview?", "Error reading file", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        e.Node.Remove();
                    }
                    return;
                }
                DisplayTreeView(LoadedToken, selectedNode.Name);
                MainTreeView.Nodes[0].Expand();
                MainTreeView.SelectedNode = MainTreeView.Nodes[0];
                MainTreeView.Focus();
                EnableDisableTreeViewContextMenu();
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
            JsonHelper.WriteToJsonFile(LoadedToken, selectedFileList.FilePath);
            selectedFileList.EditedAfterLoad = false;
            IsNodeEdited = false;
            selectedNode.ForeColor = Color.Black;
            ChangeColorOfAllNodes(MainTreeView, Color.White, Color.Black);
        }

        private void ChangeColorOfAllNodes(TreeView treeView, Color backColor, Color foreColor)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                RecursiveColorChange(node, backColor, foreColor);
            }
        }

        private void RecursiveColorChange(TreeNode startNode, Color backColor, Color foreColor)
        {
            startNode.BackColor = backColor;
            startNode.ForeColor = foreColor;

            foreach (TreeNode node in startNode.Nodes)
            {
                RecursiveColorChange(node, backColor, foreColor);
            }
        }

        private void OpenFolderMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder that contains the files you want to edit.";
            fbd.ShowNewFolderButton = false;
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
            {
                if (!RecentFoldersList.Exists(x => x.Path == fbd.SelectedPath))
                {
                    if (RecentFoldersList.Count == 10)
                    {
                        RecentFoldersList.RemoveAt(0);
                    }
                    RecentFolders newRecentFolder = new RecentFolders();
                    DirectoryInfo dirInfo = new DirectoryInfo(fbd.SelectedPath);
                    newRecentFolder.DisplayName = dirInfo.Name;
                    newRecentFolder.Path = fbd.SelectedPath;
                    newRecentFolder.Added = DateTime.Now.ToFileTimeUtc();
                    RecentFoldersList.Add(newRecentFolder);
                }
                RecentFoldersHelper.Save(RecentFoldersList);
                PopulateRecentFolders();
                MultiFileTreeView.Nodes.Clear();
                MainTreeView.Nodes.Clear();
                UpdateFileTreeView(fbd.SelectedPath, "*.json", true);
            }
        }

        public static bool TokenIsEmpty(JToken token)
        {
            if (token == null)
            {
                return true;
            }
            else
            {
                return (token.Type == JTokenType.Array && !token.HasValues) ||
                       (token.Type == JTokenType.Object && !token.HasValues) ||
                       (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                       (token.Type == JTokenType.Null);
            }
        }

        private void MultiFileTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (IsNodeEdited && e.Node != lastEditedNode)
            {
                var selectedNode = lastEditedNode;
                FileList selectedFileList = selectedNode.Tag as FileList;
                var result = MessageBox.Show($"You have unsaved changes to {selectedNode.Text}\nDo you want to save them?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes || result == DialogResult.No)
                {
                    if (result == DialogResult.Yes)
                    {
                        JsonHelper.WriteToJsonFile(LoadedToken, selectedFileList.FilePath);
                        ChangeColorOfAllNodes(MainTreeView, Color.White, Color.Black);
                    }
                    selectedFileList.EditedAfterLoad = false;
                    IsNodeEdited = false;
                    selectedNode.ForeColor = Color.Black;
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CompareJTokensAndNotify(JToken originalToken, JToken editedToken)
        {
            if (!JToken.DeepEquals(originalToken, editedToken))
            {
                WriteToSelectedNode();
            }
        }

        private void DuplicateNodeAndJToken(TreeNode originalNode)
        {
            // Clone the Tree Node
            TreeNode clonedNode = (TreeNode)originalNode.Clone();
            //if text on originalNode is a number, increase this by 1
            if (int.TryParse(originalNode.Text, out int result))
            {
                clonedNode.Text = (result + 1).ToString();
            }
            else
            {
                clonedNode.Text = originalNode.Text;
            }
            originalNode.Parent.Nodes.Add(clonedNode);

            // Clone the JToken
            TreeNodeTagClass originalNodeTag = (TreeNodeTagClass)originalNode.Tag;

            JToken originalToken = (JToken)originalNodeTag.JsonObject;
            JToken clonedToken = originalToken.DeepClone();

            // Get the parent JToken

            TreeNodeTagClassParent parentNodeTag = originalNodeTag.Parent;

            JToken parentToken = (JToken)parentNodeTag.JsonObject;

            // Add the cloned JToken to the parent
            if (parentToken.Type == JTokenType.Array)
            {
                ((JArray)parentToken).Add(clonedToken);
            }
            else if (parentToken.Type == JTokenType.Object)
            {
                ((JObject)parentToken).Add(originalToken.Path.Split('.')[^1], clonedToken);
            }

            // Update the tag for the cloned node
            clonedNode.Tag = clonedToken;

            CompareJTokensAndNotify(LoadedUnEditedToken, LoadedToken);
        }

        private void MainTreeDuplicateNodeMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to duplicate this node?", "Duplicate?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                TreeNode selectedNode = MainTreeView.SelectedNode;
                if (selectedNode != null && selectedNode.Level > 0)
                {
                    DuplicateNodeAndJToken(selectedNode);
                }
            }
        }

        private void MultiFileTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            TreeNode selectedNode = MultiFileTreeView.SelectedNode;
            if (selectedNode != null && selectedNode.Level > 0)
            {
                FileList selectedNodeTag = selectedNode.Tag as FileList;
                if (selectedNodeTag != null && e.KeyCode == Keys.Enter)
                {
                    MultiFileTreeView_NodeMouseDoubleClick(sender, new TreeNodeMouseClickEventArgs(selectedNode, MouseButtons.Left, 2, 0, 0));
                }
            }
        }

        private void SettingsSpacedLabelsMenuItem_Click(object sender, EventArgs e)
        {
            if (SettingsSpacedLabelsMenuItem.Checked)
            {
                SettingsSpacedLabelsMenuItem.Checked = false;
            }
            else
            {
                SettingsSpacedLabelsMenuItem.Checked = true;
            }
        }

        private void SearchAndHighlightNodes(string searchValue, TreeView treeView, Color backColor, Color foreColor)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                RecursiveSearchAndHighlight(node, searchValue, backColor, foreColor);
            }
        }

        private void RecursiveSearchAndHighlight(TreeNode startNode, string searchValue, Color backColor, Color foreColor)
        {
            if (startNode.Tag != null)
            {
                TreeNodeTagClass nodeTag = (TreeNodeTagClass)startNode.Tag;
                if (!string.IsNullOrEmpty(nodeTag.Name) && nodeTag.Name.ToLower().Contains(searchValue.ToLower()))
                {
                    HighlightNodeAndParents(startNode, backColor, foreColor);
                }
            }
            else
            {
                if (startNode.Text.ToLower().Contains(searchValue.ToLower()))
                {
                    HighlightNodeAndParents(startNode, backColor, foreColor);
                }
            }

            foreach (TreeNode node in startNode.Nodes)
            {
                RecursiveSearchAndHighlight(node, searchValue, backColor, foreColor);
            }
        }

        private void HighlightNodeAndParents(TreeNode node, Color backColor, Color foreColor)
        {
            while (node != null)
            {
                node.BackColor = backColor;
                node.ForeColor = foreColor;
                node = node.Parent;
            }
        }

        private void MainTreeSearchMenuItem_Click(object sender, EventArgs e)
        {
            SearchMainTreeForm searchMainTreeForm = new SearchMainTreeForm();
            if (searchMainTreeForm.ShowDialog() == DialogResult.OK)
            {
                SearchAndHighlightNodes(searchMainTreeForm.SearchData, MainTreeView, Color.Yellow, Color.Black);
            }
        }

        private void MainTreeBatchEditMenuItem_Click(object sender, EventArgs e)
        {
            BatchEditForm batchEditForm = new BatchEditForm();
            if (batchEditForm.ShowDialog() == DialogResult.OK && !batchWorker.IsBusy)
            {
                ShowUpdateLabel();
                batchWorker.RunWorkerAsync(new { Param1 = batchEditForm.SearchData, Param2 = MainTreeView, Param3 = batchEditForm.SearchMatchValue, Param4 = batchEditForm.WhatToDoValue, Param5 = batchEditForm.FactorValue });
            }
        }

        private void ShowUpdateLabel()
        {
            ProgressLabel.Visible = true;
            this.Cursor = Cursors.WaitCursor;
        }

        private void HideUpdateLabel()
        {
            ProgressLabel.Visible = false;
            this.Cursor = Cursors.Default;
        }

        private void BatchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            dynamic data = e.Argument;
            foreach (TreeNode node in data.Param2.Nodes)
            {
                RecursiveSearchAndBatchEdit(node, data.Param1, data.Param3, data.Param4, data.Param5);
            }
        }

        private void batchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HideUpdateLabel();
            if (e.Error != null)
            {
                MessageBox.Show($"Error: {e.Error.Message}", "Batch edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Batch edit completed", "Batch edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RecursiveSearchAndBatchEdit(TreeNode startNode, string searchValue, int searchmatchvalue, int whattodovalue, int factor)
        {
            if (startNode.Tag != null)
            {
                TreeNodeTagClass nodeTag = (TreeNodeTagClass)startNode.Tag;
                if (searchmatchvalue == 0)
                {
                    if (!string.IsNullOrEmpty(nodeTag.Name) && nodeTag.Name.ToLower().Contains(searchValue.ToLower()))
                    {
                        BatchEditNode(startNode, whattodovalue, factor);
                    }
                }
                else if (searchmatchvalue == 1)
                {
                    if (!string.IsNullOrEmpty(nodeTag.Name) && nodeTag.Name.ToLower().StartsWith(searchValue.ToLower()))
                    {
                        BatchEditNode(startNode, whattodovalue, factor);
                    }
                }
                else if (searchmatchvalue == 2)
                {
                    if (!string.IsNullOrEmpty(nodeTag.Name) && nodeTag.Name.ToLower().EndsWith(searchValue.ToLower()))
                    {
                        BatchEditNode(startNode, whattodovalue, factor);
                    }
                }
                else if (searchmatchvalue == 3 && !string.IsNullOrEmpty(nodeTag.Name) && nodeTag.Name.ToLower().Equals(searchValue.ToLower()))
                {
                    BatchEditNode(startNode, whattodovalue, factor);
                }
            }
            else
            {
                if (searchmatchvalue == 0)
                {
                    if (startNode.Text.ToLower().Contains(searchValue.ToLower()))
                    {
                        BatchEditNode(startNode, whattodovalue, factor);
                    }
                }
                else if (searchmatchvalue == 1)
                {
                    if (startNode.Text.ToLower().StartsWith(searchValue.ToLower()))
                    {
                        BatchEditNode(startNode, whattodovalue, factor);
                    }
                }
                else if (searchmatchvalue == 2)
                {
                    if (startNode.Text.ToLower().EndsWith(searchValue.ToLower()))
                    {
                        BatchEditNode(startNode, whattodovalue, factor);
                    }
                }
                else if (searchmatchvalue == 3 && startNode.Text.ToLower().Equals(searchValue.ToLower()))
                {
                    BatchEditNode(startNode, whattodovalue, factor);
                }
            }
            foreach (TreeNode node in startNode.Nodes)
            {
                RecursiveSearchAndBatchEdit(node, searchValue, searchmatchvalue, whattodovalue, factor);
            }
        }

        private void BatchEditNode(TreeNode selectedNode, int whattodovalue, int factor)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => BatchEditNode(selectedNode, whattodovalue, factor)));
                return;
            }
            if (whattodovalue == 0)
            {
                //Add
                var JPropTag = selectedNode.Tag as TreeNodeTagClass;
                var JPropNode = JPropTag.JsonObject as JProperty;
                if (JPropNode != null)
                {
                    if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                    {
                        float value = Convert.ToSingle(JPropNode.Value);
                        value += factor;
                        JPropNode.Value = value;
                        selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                        ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
                    }
                    else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                    {
                        int value = (int)JPropNode.Value;
                        value += factor;
                        JPropNode.Value = value;
                        selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                        ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
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
                            value += factor;
                            JsonValue.Value = value;
                            selectedNode.Text = value.ToString();
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode();
                        }
                        else if (JsonValue.Type == JTokenType.Float)
                        {
                            float value = Convert.ToSingle(JsonValue.Value);
                            value += factor;
                            JsonValue.Value = value;
                            selectedNode.Text = value.ToString();
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode();
                        }
                    }
                }
            }
            else if (whattodovalue == 1)
            {
                //Subtract
                var JPropTag = selectedNode.Tag as TreeNodeTagClass;
                var JPropNode = JPropTag.JsonObject as JProperty;
                if (JPropNode != null)
                {
                    if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                    {
                        float value = Convert.ToSingle(JPropNode.Value);
                        value -= factor;
                        JPropNode.Value = value;
                        selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                        ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
                    }
                    else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                    {
                        int value = (int)JPropNode.Value;
                        value -= factor;
                        JPropNode.Value = value;
                        selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                        ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
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
                            value -= factor;
                            JsonValue.Value = value;
                            selectedNode.Text = value.ToString();
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode();
                        }
                        else if (JsonValue.Type == JTokenType.Float)
                        {
                            float value = Convert.ToSingle(JsonValue.Value);
                            value -= factor;
                            JsonValue.Value = value;
                            selectedNode.Text = value.ToString();
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode();
                        }
                    }
                }
            }
            else if (whattodovalue == 2)
            {
                //Multiply
                var JPropTag = selectedNode.Tag as TreeNodeTagClass;
                var JPropNode = JPropTag.JsonObject as JProperty;
                if (JPropNode != null)
                {
                    if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                    {
                        float value = Convert.ToSingle(JPropNode.Value);
                        value *= factor;
                        JPropNode.Value = value;
                        selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                        ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
                    }
                    else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                    {
                        int value = (int)JPropNode.Value;
                        value *= factor;
                        JPropNode.Value = value;
                        selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                        ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
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
                            value *= factor;
                            JsonValue.Value = value;
                            selectedNode.Text = value.ToString();
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode();
                        }
                        else if (JsonValue.Type == JTokenType.Float)
                        {
                            float value = Convert.ToSingle(JsonValue.Value);
                            value *= factor;
                            JsonValue.Value = value;
                            selectedNode.Text = value.ToString();
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode();
                        }
                    }
                }
            }
            else if (whattodovalue == 3)
            {
                //Divide
                var JPropTag = selectedNode.Tag as TreeNodeTagClass;
                var JPropNode = JPropTag.JsonObject as JProperty;
                if (JPropNode != null)
                {
                    if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Float)
                    {
                        float value = Convert.ToSingle(JPropNode.Value);
                        value /= factor;
                        JPropNode.Value = value;
                        selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                        ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
                    }
                    else if (JPropNode.Children().FirstOrDefault().Type == JTokenType.Integer)
                    {
                        int value = (int)JPropNode.Value;
                        value /= factor;
                        JPropNode.Value = value;
                        selectedNode.Text = $"{JPropNode.Name.ToSpacedName(SettingsSpacedLabelsMenuItem.Checked)}: {value.ToTrimmedString()}";
                        ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                        WriteToSelectedNode();
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
                            value /= factor;
                            JsonValue.Value = value;
                            selectedNode.Text = value.ToString();
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode();
                        }
                        else if (JsonValue.Type == JTokenType.Float)
                        {
                            float value = Convert.ToSingle(JsonValue.Value);
                            value /= factor;
                            JsonValue.Value = value;
                            selectedNode.Text = value.ToString();
                            ColorizeEditedNode(selectedNode, Color.DarkOrange, Color.Black);
                            WriteToSelectedNode();
                        }
                    }
                }
            }
        }

        private int GetTotalNodesToProcess(TreeView treeView)
        {
            int totalNodes = 0;

            foreach (TreeNode node in treeView.Nodes)
            {
                totalNodes += GetTotalNodesRecursive(node);
            }

            return totalNodes;
        }

        private int GetTotalNodesRecursive(TreeNode node)
        {
            int totalNodes = 1;

            foreach (TreeNode childNode in node.Nodes)
            {
                totalNodes += GetTotalNodesRecursive(childNode);
            }

            return totalNodes;
        }
    }
}