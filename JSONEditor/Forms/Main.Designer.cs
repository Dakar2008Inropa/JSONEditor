namespace JSONEditor
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            MainMenu = new MenuStrip();
            FileMenuItem = new ToolStripMenuItem();
            OpenFileMenuItem = new ToolStripMenuItem();
            OpenFolderMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            ExitMenuItem = new ToolStripMenuItem();
            TreePanel = new Panel();
            MainTreeView = new TreeView();
            MainTreeContextMenu = new ContextMenuStrip(components);
            MainTree_CollapseAllMenuItem = new ToolStripMenuItem();
            MainTree_ExpandAllMenuItem = new ToolStripMenuItem();
            MultiFilePanel = new Panel();
            MultiFileTreeView = new TreeView();
            MultiFileTreeViewContextMenu = new ContextMenuStrip(components);
            MultiFileTreeView_SaveSelectedNodeMenuItem = new ToolStripMenuItem();
            MultiFileTreeViewImgList = new ImageList(components);
            MainMenu.SuspendLayout();
            TreePanel.SuspendLayout();
            MainTreeContextMenu.SuspendLayout();
            MultiFilePanel.SuspendLayout();
            MultiFileTreeViewContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // MainMenu
            // 
            MainMenu.BackColor = Color.Gainsboro;
            MainMenu.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            MainMenu.Items.AddRange(new ToolStripItem[] { FileMenuItem });
            MainMenu.Location = new Point(0, 0);
            MainMenu.Margin = new Padding(0, 0, 0, 10);
            MainMenu.Name = "MainMenu";
            MainMenu.Size = new Size(799, 24);
            MainMenu.TabIndex = 0;
            MainMenu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            FileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenFileMenuItem, OpenFolderMenuItem, toolStripSeparator2, toolStripMenuItem1, toolStripSeparator3, ExitMenuItem });
            FileMenuItem.Name = "FileMenuItem";
            FileMenuItem.Size = new Size(41, 20);
            FileMenuItem.Text = "File";
            // 
            // OpenFileMenuItem
            // 
            OpenFileMenuItem.Image = Properties.Resources.Custom_Icon_Design_Pretty_Office_9_Open_file_48;
            OpenFileMenuItem.Name = "OpenFileMenuItem";
            OpenFileMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            OpenFileMenuItem.Size = new Size(242, 22);
            OpenFileMenuItem.Text = "Open File";
            OpenFileMenuItem.Click += OpenFileMenuItem_Click;
            // 
            // OpenFolderMenuItem
            // 
            OpenFolderMenuItem.Image = Properties.Resources.Avosoft_Warm_Toolbar_Folder_open_48;
            OpenFolderMenuItem.Name = "OpenFolderMenuItem";
            OpenFolderMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
            OpenFolderMenuItem.Size = new Size(242, 22);
            OpenFolderMenuItem.Text = "Open Folder";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(239, 6);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Image = Properties.Resources.Custom_Icon_Design_Pretty_Office_7_Save_48;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            toolStripMenuItem1.Size = new Size(242, 22);
            toolStripMenuItem1.Text = "Save All";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(239, 6);
            // 
            // ExitMenuItem
            // 
            ExitMenuItem.Image = Properties.Resources.Hopstarter_Sleek_Xp_Software_Windows_Close_Program_48;
            ExitMenuItem.Name = "ExitMenuItem";
            ExitMenuItem.ShortcutKeys = Keys.Control | Keys.F4;
            ExitMenuItem.Size = new Size(242, 22);
            ExitMenuItem.Text = "Exit";
            // 
            // TreePanel
            // 
            TreePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TreePanel.Controls.Add(MainTreeView);
            TreePanel.Location = new Point(305, 34);
            TreePanel.Margin = new Padding(0, 0, 0, 5);
            TreePanel.Name = "TreePanel";
            TreePanel.Size = new Size(485, 303);
            TreePanel.TabIndex = 1;
            // 
            // MainTreeView
            // 
            MainTreeView.BackColor = Color.White;
            MainTreeView.BorderStyle = BorderStyle.FixedSingle;
            MainTreeView.ContextMenuStrip = MainTreeContextMenu;
            MainTreeView.Dock = DockStyle.Fill;
            MainTreeView.Location = new Point(0, 0);
            MainTreeView.Name = "MainTreeView";
            MainTreeView.Size = new Size(485, 303);
            MainTreeView.TabIndex = 0;
            MainTreeView.AfterLabelEdit += MainTreeView_AfterLabelEdit;
            MainTreeView.NodeMouseDoubleClick += MainTreeView_NodeMouseDoubleClick;
            MainTreeView.KeyDown += MainTreeView_KeyDown;
            // 
            // MainTreeContextMenu
            // 
            MainTreeContextMenu.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            MainTreeContextMenu.Items.AddRange(new ToolStripItem[] { MainTree_CollapseAllMenuItem, MainTree_ExpandAllMenuItem });
            MainTreeContextMenu.Name = "MainTreeContextMenu";
            MainTreeContextMenu.Size = new Size(149, 48);
            // 
            // MainTree_CollapseAllMenuItem
            // 
            MainTree_CollapseAllMenuItem.Image = Properties.Resources.Collapse_All_16x16;
            MainTree_CollapseAllMenuItem.Name = "MainTree_CollapseAllMenuItem";
            MainTree_CollapseAllMenuItem.Size = new Size(148, 22);
            MainTree_CollapseAllMenuItem.Text = "Collapse All";
            MainTree_CollapseAllMenuItem.Click += MainTree_CollapseAllMenuItem_Click;
            // 
            // MainTree_ExpandAllMenuItem
            // 
            MainTree_ExpandAllMenuItem.Image = Properties.Resources.Expand_All_16x16;
            MainTree_ExpandAllMenuItem.Name = "MainTree_ExpandAllMenuItem";
            MainTree_ExpandAllMenuItem.Size = new Size(148, 22);
            MainTree_ExpandAllMenuItem.Text = "Expand All";
            MainTree_ExpandAllMenuItem.Click += MainTree_ExpandAllMenuItem_Click;
            // 
            // MultiFilePanel
            // 
            MultiFilePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            MultiFilePanel.Controls.Add(MultiFileTreeView);
            MultiFilePanel.Location = new Point(9, 34);
            MultiFilePanel.Margin = new Padding(0, 0, 5, 0);
            MultiFilePanel.Name = "MultiFilePanel";
            MultiFilePanel.Size = new Size(291, 303);
            MultiFilePanel.TabIndex = 2;
            // 
            // MultiFileTreeView
            // 
            MultiFileTreeView.BackColor = Color.White;
            MultiFileTreeView.BorderStyle = BorderStyle.FixedSingle;
            MultiFileTreeView.ContextMenuStrip = MultiFileTreeViewContextMenu;
            MultiFileTreeView.Dock = DockStyle.Fill;
            MultiFileTreeView.ImageIndex = 0;
            MultiFileTreeView.ImageList = MultiFileTreeViewImgList;
            MultiFileTreeView.Indent = 20;
            MultiFileTreeView.Location = new Point(0, 0);
            MultiFileTreeView.Name = "MultiFileTreeView";
            MultiFileTreeView.SelectedImageIndex = 0;
            MultiFileTreeView.Size = new Size(291, 303);
            MultiFileTreeView.TabIndex = 0;
            MultiFileTreeView.NodeMouseDoubleClick += MultiFileTreeView_NodeMouseDoubleClick;
            // 
            // MultiFileTreeViewContextMenu
            // 
            MultiFileTreeViewContextMenu.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            MultiFileTreeViewContextMenu.Items.AddRange(new ToolStripItem[] { MultiFileTreeView_SaveSelectedNodeMenuItem });
            MultiFileTreeViewContextMenu.Name = "MultiFileTreeViewContextMenu";
            MultiFileTreeViewContextMenu.Size = new Size(108, 26);
            // 
            // MultiFileTreeView_SaveSelectedNodeMenuItem
            // 
            MultiFileTreeView_SaveSelectedNodeMenuItem.Image = Properties.Resources.Custom_Icon_Design_Pretty_Office_7_Save_48;
            MultiFileTreeView_SaveSelectedNodeMenuItem.Name = "MultiFileTreeView_SaveSelectedNodeMenuItem";
            MultiFileTreeView_SaveSelectedNodeMenuItem.Size = new Size(107, 22);
            MultiFileTreeView_SaveSelectedNodeMenuItem.Text = "Save";
            MultiFileTreeView_SaveSelectedNodeMenuItem.Click += MultiFileTreeView_SaveSelectedNodeMenuItem_Click;
            // 
            // MultiFileTreeViewImgList
            // 
            MultiFileTreeViewImgList.ColorDepth = ColorDepth.Depth32Bit;
            MultiFileTreeViewImgList.ImageStream = (ImageListStreamer)resources.GetObject("MultiFileTreeViewImgList.ImageStream");
            MultiFileTreeViewImgList.TransparentColor = Color.Transparent;
            MultiFileTreeViewImgList.Images.SetKeyName(0, "Custom-Icon-Design-Flatastic-1-Document.48.png");
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(799, 346);
            Controls.Add(MultiFilePanel);
            Controls.Add(TreePanel);
            Controls.Add(MainMenu);
            Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = MainMenu;
            MinimumSize = new Size(815, 355);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inropa JSON Editor";
            FormClosing += Main_FormClosing;
            Load += Main_Load;
            MainMenu.ResumeLayout(false);
            MainMenu.PerformLayout();
            TreePanel.ResumeLayout(false);
            MainTreeContextMenu.ResumeLayout(false);
            MultiFilePanel.ResumeLayout(false);
            MultiFileTreeViewContextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip MainMenu;
        private ToolStripMenuItem FileMenuItem;
        private Panel TreePanel;
        private ToolStripMenuItem OpenFileMenuItem;
        private ToolStripMenuItem OpenFolderMenuItem;
        private ToolStripMenuItem ExitMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private Panel MultiFilePanel;
        private TreeView MainTreeView;
        private TreeView MultiFileTreeView;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripMenuItem1;
        private ImageList MultiFileTreeViewImgList;
        private ContextMenuStrip MainTreeContextMenu;
        private ContextMenuStrip MultiFileTreeViewContextMenu;
        private ToolStripMenuItem MainTree_CollapseAllMenuItem;
        private ToolStripMenuItem MainTree_ExpandAllMenuItem;
        private ToolStripMenuItem MultiFileTreeView_SaveSelectedNodeMenuItem;
    }
}