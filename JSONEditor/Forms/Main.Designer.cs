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
            toolStripSeparator3 = new ToolStripSeparator();
            RecentFilesMenuItem = new ToolStripMenuItem();
            RecentFoldersMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            ExitMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            SettingsSpacedLabelsMenuItem = new ToolStripMenuItem();
            MainTreeView = new TreeView();
            MainTreeContextMenu = new ContextMenuStrip(components);
            MainTreeSearchMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            MainTree_CollapseAllMenuItem = new ToolStripMenuItem();
            MainTree_ExpandAllMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            MainTreeBatchEditMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            MainTreeDuplicateNodeMenuItem = new ToolStripMenuItem();
            MultiFileTreeView = new TreeView();
            MultiFileTreeViewContextMenu = new ContextMenuStrip(components);
            MultiFileTreeView_SaveSelectedNodeMenuItem = new ToolStripMenuItem();
            MultiFileTreeViewImgList = new ImageList(components);
            splitContainer1 = new SplitContainer();
            FileMenuToolTip = new ToolTip(components);
            ProgressLabel = new Label();
            MainMenu.SuspendLayout();
            MainTreeContextMenu.SuspendLayout();
            MultiFileTreeViewContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // MainMenu
            // 
            MainMenu.BackColor = Color.Gainsboro;
            MainMenu.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            MainMenu.Items.AddRange(new ToolStripItem[] { FileMenuItem, toolStripMenuItem1 });
            MainMenu.Location = new Point(0, 0);
            MainMenu.Margin = new Padding(0, 0, 0, 10);
            MainMenu.Name = "MainMenu";
            MainMenu.Size = new Size(934, 24);
            MainMenu.TabIndex = 0;
            MainMenu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            FileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenFileMenuItem, OpenFolderMenuItem, toolStripSeparator3, RecentFilesMenuItem, RecentFoldersMenuItem, toolStripSeparator5, ExitMenuItem });
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
            OpenFileMenuItem.Text = "Open Files";
            OpenFileMenuItem.Click += OpenFileMenuItem_Click;
            // 
            // OpenFolderMenuItem
            // 
            OpenFolderMenuItem.Image = Properties.Resources.Avosoft_Warm_Toolbar_Folder_open_48;
            OpenFolderMenuItem.Name = "OpenFolderMenuItem";
            OpenFolderMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
            OpenFolderMenuItem.Size = new Size(242, 22);
            OpenFolderMenuItem.Text = "Open Folder";
            OpenFolderMenuItem.Click += OpenFolderMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(239, 6);
            // 
            // RecentFilesMenuItem
            // 
            RecentFilesMenuItem.Image = Properties.Resources.Rokey_The_Last_Order_Candy_My_recent_documents_48;
            RecentFilesMenuItem.Name = "RecentFilesMenuItem";
            RecentFilesMenuItem.Size = new Size(242, 22);
            RecentFilesMenuItem.Text = "Recent Files";
            // 
            // RecentFoldersMenuItem
            // 
            RecentFoldersMenuItem.Image = Properties.Resources.Papirus_Team_Papirus_Places_Folder_yellow_recent_48;
            RecentFoldersMenuItem.Name = "RecentFoldersMenuItem";
            RecentFoldersMenuItem.Size = new Size(242, 22);
            RecentFoldersMenuItem.Text = "Recent Folders";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(239, 6);
            // 
            // ExitMenuItem
            // 
            ExitMenuItem.Image = Properties.Resources.Hopstarter_Sleek_Xp_Software_Windows_Close_Program_48;
            ExitMenuItem.Name = "ExitMenuItem";
            ExitMenuItem.ShortcutKeys = Keys.Control | Keys.F4;
            ExitMenuItem.Size = new Size(242, 22);
            ExitMenuItem.Text = "Exit";
            ExitMenuItem.Click += ExitMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { SettingsSpacedLabelsMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(74, 20);
            toolStripMenuItem1.Text = "Settings";
            // 
            // SettingsSpacedLabelsMenuItem
            // 
            SettingsSpacedLabelsMenuItem.Name = "SettingsSpacedLabelsMenuItem";
            SettingsSpacedLabelsMenuItem.Size = new Size(169, 22);
            SettingsSpacedLabelsMenuItem.Text = "Spaced Labels";
            SettingsSpacedLabelsMenuItem.Click += SettingsSpacedLabelsMenuItem_Click;
            // 
            // MainTreeView
            // 
            MainTreeView.BackColor = Color.White;
            MainTreeView.BorderStyle = BorderStyle.FixedSingle;
            MainTreeView.ContextMenuStrip = MainTreeContextMenu;
            MainTreeView.Dock = DockStyle.Fill;
            MainTreeView.Location = new Point(0, 0);
            MainTreeView.Name = "MainTreeView";
            MainTreeView.Size = new Size(710, 430);
            MainTreeView.TabIndex = 0;
            MainTreeView.AfterLabelEdit += MainTreeView_AfterLabelEdit;
            MainTreeView.NodeMouseDoubleClick += MainTreeView_NodeMouseDoubleClick;
            MainTreeView.KeyDown += MainTreeView_KeyDown;
            // 
            // MainTreeContextMenu
            // 
            MainTreeContextMenu.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            MainTreeContextMenu.Items.AddRange(new ToolStripItem[] { MainTreeSearchMenuItem, toolStripSeparator2, MainTree_CollapseAllMenuItem, MainTree_ExpandAllMenuItem, toolStripSeparator1, MainTreeBatchEditMenuItem, toolStripSeparator4, MainTreeDuplicateNodeMenuItem });
            MainTreeContextMenu.Name = "MainTreeContextMenu";
            MainTreeContextMenu.Size = new Size(174, 132);
            // 
            // MainTreeSearchMenuItem
            // 
            MainTreeSearchMenuItem.Image = Properties.Resources.searchicon;
            MainTreeSearchMenuItem.Name = "MainTreeSearchMenuItem";
            MainTreeSearchMenuItem.Size = new Size(173, 22);
            MainTreeSearchMenuItem.Text = "Search";
            MainTreeSearchMenuItem.Click += MainTreeSearchMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(170, 6);
            // 
            // MainTree_CollapseAllMenuItem
            // 
            MainTree_CollapseAllMenuItem.Image = Properties.Resources.Collapse_All_16x16;
            MainTree_CollapseAllMenuItem.Name = "MainTree_CollapseAllMenuItem";
            MainTree_CollapseAllMenuItem.Size = new Size(173, 22);
            MainTree_CollapseAllMenuItem.Text = "Collapse All";
            MainTree_CollapseAllMenuItem.Click += MainTree_CollapseAllMenuItem_Click;
            // 
            // MainTree_ExpandAllMenuItem
            // 
            MainTree_ExpandAllMenuItem.Image = Properties.Resources.Expand_All_16x16;
            MainTree_ExpandAllMenuItem.Name = "MainTree_ExpandAllMenuItem";
            MainTree_ExpandAllMenuItem.Size = new Size(173, 22);
            MainTree_ExpandAllMenuItem.Text = "Expand All";
            MainTree_ExpandAllMenuItem.Click += MainTree_ExpandAllMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(170, 6);
            // 
            // MainTreeBatchEditMenuItem
            // 
            MainTreeBatchEditMenuItem.Image = Properties.Resources.Rokey_Smooth_Metal_Msdos_batch_file_48;
            MainTreeBatchEditMenuItem.Name = "MainTreeBatchEditMenuItem";
            MainTreeBatchEditMenuItem.Size = new Size(173, 22);
            MainTreeBatchEditMenuItem.Text = "Batch Edit";
            MainTreeBatchEditMenuItem.Click += MainTreeBatchEditMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(170, 6);
            // 
            // MainTreeDuplicateNodeMenuItem
            // 
            MainTreeDuplicateNodeMenuItem.Image = Properties.Resources.Amitjakhu_Drip_Duplicate_48;
            MainTreeDuplicateNodeMenuItem.Name = "MainTreeDuplicateNodeMenuItem";
            MainTreeDuplicateNodeMenuItem.Size = new Size(173, 22);
            MainTreeDuplicateNodeMenuItem.Text = "Duplicate Node";
            MainTreeDuplicateNodeMenuItem.Click += MainTreeDuplicateNodeMenuItem_Click;
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
            MultiFileTreeView.Size = new Size(200, 430);
            MultiFileTreeView.TabIndex = 0;
            MultiFileTreeView.BeforeSelect += MultiFileTreeView_BeforeSelect;
            MultiFileTreeView.NodeMouseDoubleClick += MultiFileTreeView_NodeMouseDoubleClick;
            MultiFileTreeView.KeyDown += MultiFileTreeView_KeyDown;
            // 
            // MultiFileTreeViewContextMenu
            // 
            MultiFileTreeViewContextMenu.Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            MultiFileTreeViewContextMenu.Items.AddRange(new ToolStripItem[] { MultiFileTreeView_SaveSelectedNodeMenuItem });
            MultiFileTreeViewContextMenu.Name = "MultiFileTreeViewContextMenu";
            MultiFileTreeViewContextMenu.Size = new Size(156, 26);
            // 
            // MultiFileTreeView_SaveSelectedNodeMenuItem
            // 
            MultiFileTreeView_SaveSelectedNodeMenuItem.Image = Properties.Resources.Custom_Icon_Design_Pretty_Office_7_Save_48;
            MultiFileTreeView_SaveSelectedNodeMenuItem.Name = "MultiFileTreeView_SaveSelectedNodeMenuItem";
            MultiFileTreeView_SaveSelectedNodeMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            MultiFileTreeView_SaveSelectedNodeMenuItem.Size = new Size(155, 22);
            MultiFileTreeView_SaveSelectedNodeMenuItem.Text = "Save";
            MultiFileTreeView_SaveSelectedNodeMenuItem.Click += MultiFileTreeView_SaveSelectedNodeMenuItem_Click;
            // 
            // MultiFileTreeViewImgList
            // 
            MultiFileTreeViewImgList.ColorDepth = ColorDepth.Depth32Bit;
            MultiFileTreeViewImgList.ImageStream = (ImageListStreamer)resources.GetObject("MultiFileTreeViewImgList.ImageStream");
            MultiFileTreeViewImgList.TransparentColor = Color.Transparent;
            MultiFileTreeViewImgList.Images.SetKeyName(0, "Custom-Icon-Design-Flatastic-1-Document.48.png");
            MultiFileTreeViewImgList.Images.SetKeyName(1, "Custom-Icon-Design-Flatastic-1-Folder.48.png");
            MultiFileTreeViewImgList.Images.SetKeyName(2, "Oxygen-Icons.org-Oxygen-Places-folder-red.48.png");
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(9, 34);
            splitContainer1.Margin = new Padding(0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(MultiFileTreeView);
            splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(ProgressLabel);
            splitContainer1.Panel2.Controls.Add(MainTreeView);
            splitContainer1.Size = new Size(916, 430);
            splitContainer1.SplitterDistance = 200;
            splitContainer1.SplitterWidth = 6;
            splitContainer1.TabIndex = 3;
            splitContainer1.TabStop = false;
            // 
            // ProgressLabel
            // 
            ProgressLabel.Dock = DockStyle.Fill;
            ProgressLabel.Location = new Point(0, 0);
            ProgressLabel.Name = "ProgressLabel";
            ProgressLabel.Size = new Size(710, 430);
            ProgressLabel.TabIndex = 1;
            ProgressLabel.Text = "Updating Data... Please Wait...";
            ProgressLabel.TextAlign = ContentAlignment.MiddleCenter;
            ProgressLabel.Visible = false;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(934, 473);
            Controls.Add(splitContainer1);
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
            MainTreeContextMenu.ResumeLayout(false);
            MultiFileTreeViewContextMenu.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip MainMenu;
        private ToolStripMenuItem FileMenuItem;
        private ToolStripMenuItem OpenFileMenuItem;
        private ToolStripMenuItem OpenFolderMenuItem;
        private ToolStripMenuItem ExitMenuItem;
        private TreeView MainTreeView;
        private TreeView MultiFileTreeView;
        private ToolStripSeparator toolStripSeparator3;
        private ImageList MultiFileTreeViewImgList;
        private ContextMenuStrip MainTreeContextMenu;
        private ContextMenuStrip MultiFileTreeViewContextMenu;
        private ToolStripMenuItem MainTree_CollapseAllMenuItem;
        private ToolStripMenuItem MainTree_ExpandAllMenuItem;
        private ToolStripMenuItem MultiFileTreeView_SaveSelectedNodeMenuItem;
        private SplitContainer splitContainer1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem MainTreeDuplicateNodeMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem MainTreeSearchMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem SettingsSpacedLabelsMenuItem;
        private ToolStripMenuItem MainTreeBatchEditMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem RecentFilesMenuItem;
        private ToolStripMenuItem RecentFoldersMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolTip FileMenuToolTip;
        private Label ProgressLabel;
    }
}