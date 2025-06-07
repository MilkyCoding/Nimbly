using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace NimblyApp
{
    public partial class FileTreeComponent : UserControl
    {
        private readonly TreeView _treeView;
        private readonly Panel _headerPanel;
        private readonly Label _folderLabel;
        private string _currentPath = string.Empty;

        public event EventHandler<string>? FileSelected;

        public FileTreeComponent()
        {
            this.Dock = DockStyle.Left;
            this.Width = 250;
            this.BackColor = ThemeColors.MainBackground;
            this.Padding = new Padding(0);

            // Создаем заголовок с названием папки
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = ThemeColors.TabPanel,
                Padding = new Padding(8, 0, 8, 0)
            };

            _folderLabel = new Label
            {
                Text = "Файлы",
                ForeColor = ThemeColors.WhiteColor,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _headerPanel.Controls.Add(_folderLabel);

            // Создаем дерево файлов
            _treeView = new TreeView
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.MainBackground,
                ForeColor = ThemeColors.WhiteColor,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                BorderStyle = BorderStyle.None,
                Indent = 20,
                ItemHeight = 22,
                ShowLines = false,
                ShowPlusMinus = true,
                ShowRootLines = false,
                HideSelection = false
            };

            _treeView.ImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16, 16)
            };
            LoadDefaultIcons();

            _treeView.NodeMouseClick += TreeView_NodeMouseClick;
            _treeView.BeforeExpand += TreeView_BeforeExpand;

            // Собираем компонент
            this.Controls.Add(_treeView);
            this.Controls.Add(_headerPanel);
        }

        private void LoadDefaultIcons()
        {
            if (_treeView.ImageList == null) return;

            // Загружаем стандартные иконки для файлов и папок
            try
            {
                _treeView.ImageList.Images.Add("folder", ResourceUtils.GetIcon("ui.assets.folder.png"));
                _treeView.ImageList.Images.Add("file", ResourceUtils.GetIcon("ui.assets.file.png"));
            }
            catch
            {
                // Если иконки не найдены, используем системные
                _treeView.ImageList = null;
            }
        }

        public void LoadFolder(string path)
        {
            if (!Directory.Exists(path)) return;

            _currentPath = path;
            _folderLabel.Text = Path.GetFileName(path);
            _treeView.Nodes.Clear();

            var rootNode = new TreeNode(Path.GetFileName(path))
            {
                Tag = path,
                ImageKey = "folder",
                SelectedImageKey = "folder"
            };
            _treeView.Nodes.Add(rootNode);

            LoadDirectoryContents(rootNode, path);
            rootNode.Expand();
        }

        private void LoadDirectoryContents(TreeNode parentNode, string path)
        {
            try
            {
                // Загружаем папки
                foreach (var dir in Directory.GetDirectories(path))
                {
                    var dirNode = new TreeNode(Path.GetFileName(dir))
                    {
                        Tag = dir,
                        ImageKey = "folder",
                        SelectedImageKey = "folder"
                    };
                    parentNode.Nodes.Add(dirNode);
                    // Добавляем фиктивный узел, чтобы показать +
                    dirNode.Nodes.Add(new TreeNode());
                }

                // Загружаем файлы
                foreach (var file in Directory.GetFiles(path))
                {
                    var fileNode = new TreeNode(Path.GetFileName(file))
                    {
                        Tag = file,
                        ImageKey = "file",
                        SelectedImageKey = "file"
                    };
                    parentNode.Nodes.Add(fileNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке содержимого папки: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TreeView_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            var node = e.Node;
            if (node?.Tag == null) return;

            string path = node.Tag.ToString()!;
            if (File.Exists(path))
            {
                FileSelected?.Invoke(this, path);
            }
        }

        private void TreeView_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node;
            if (node == null || node.Tag == null) return;

            // Если узел содержит только фиктивный узел, загружаем реальное содержимое
            if (node.Nodes.Count == 1 && string.IsNullOrEmpty(node.Nodes[0].Text))
            {
                node.Nodes.Clear();
                LoadDirectoryContents(node, node.Tag.ToString()!);
            }
        }

        public void RefreshTree()
        {
            if (!string.IsNullOrEmpty(_currentPath))
            {
                LoadFolder(_currentPath);
            }
        }
    }
} 