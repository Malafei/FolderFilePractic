using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderFilePractic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ListView.View = View.Tile; // у вигляді таблиця        
            ListView.TileSize = new Size(150, 50);
            ListView.FullRowSelect = true; // дозволяє виділяти елемент клікнувши на будь-яке місце рядка

        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                treeView1.Nodes.Clear();
                string rootFolderName = new DirectoryInfo(folderBrowser.SelectedPath).Name;
                TreeNode rootNote = new TreeNode(rootFolderName)
                {
                    Tag = folderBrowser.SelectedPath
                };
                treeView1.Nodes.Add(rootNote);
                LoadFoldersToTreeTest(folderBrowser.SelectedPath, rootNote);
            }
        }

        void LoadFoldersToTreeTest(string rootFolder, TreeNode rootNode)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(rootFolder);

                foreach (var dir in info.GetDirectories())
                {
                    if (dir.Attributes.HasFlag(FileAttributes.Hidden | FileAttributes.System))
                        continue;

                    TreeNode node = new TreeNode(dir.Name)
                    {
                        Tag = dir.FullName
                    };
                    rootNode.Nodes.Add(node);

                    if (dir.GetDirectories().Count() > 0)
                    {
                        LoadFoldersToTreeTest(dir.FullName, node);
                    }
                }
            }
            catch { }
        }
        private void LoadFilesToListBox(string dirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            ListView.Items.Clear();
            foreach (var item in dir.GetFiles())
            {
                ListViewItem list = new ListViewItem(item.Name);
                list.SubItems.Add(item.CreationTime.ToString());
                list.SubItems.Add($"{(item.Length / 1024)} KB");
                list.SubItems.Add(item.LastWriteTime.ToString());
                list.SubItems.Add(item.LastAccessTime.ToString());
                list.SubItems.Add(item.Extension.ToString());
                ListView.Items.Add(list);
            }
            //ListView.Items.AddRange(dir.GetFiles().Select(f => f.Name));
        }


        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            LoadFilesToListBox(treeView1.SelectedNode.Tag.ToString());
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.View = View.Tile; // у вигляді таблиця        
            ListView.TileSize = new Size(150, 50); // розмір плитки в режимі Tile

        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.View = View.Details;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.View = View.List;
        }

        private void descToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ListView.Sorting = System.Windows.Forms.SortOrder.Descending;
        }

        private void ascendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
        }
    }

}
