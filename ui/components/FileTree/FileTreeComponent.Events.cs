using System;

namespace NimblyApp
{
    public partial class FileTreeComponent
    {
        // События для взаимодействия с файлами
        public event EventHandler<string>? FileOpened;
        public event EventHandler<string>? FileCreated;
        public event EventHandler<string>? FileDeleted;
        public event EventHandler<string>? FileRenamed;

        // События для взаимодействия с папками
        public event EventHandler<string>? FolderOpened;
        public event EventHandler<string>? FolderCreated;
        public event EventHandler<string>? FolderDeleted;
        public event EventHandler<string>? FolderRenamed;

        // Защищенные методы для вызова событий
        protected virtual void OnFileOpened(string filePath)
        {
            FileOpened?.Invoke(this, filePath);
        }

        protected virtual void OnFileCreated(string filePath)
        {
            FileCreated?.Invoke(this, filePath);
            RefreshTree();
        }

        protected virtual void OnFileDeleted(string filePath)
        {
            FileDeleted?.Invoke(this, filePath);
            RefreshTree();
        }

        protected virtual void OnFileRenamed(string filePath)
        {
            FileRenamed?.Invoke(this, filePath);
            RefreshTree();
        }

        protected virtual void OnFolderOpened(string folderPath)
        {
            FolderOpened?.Invoke(this, folderPath);
        }

        protected virtual void OnFolderCreated(string folderPath)
        {
            FolderCreated?.Invoke(this, folderPath);
            RefreshTree();
        }

        protected virtual void OnFolderDeleted(string folderPath)
        {
            FolderDeleted?.Invoke(this, folderPath);
            RefreshTree();
        }

        protected virtual void OnFolderRenamed(string folderPath)
        {
            FolderRenamed?.Invoke(this, folderPath);
            RefreshTree();
        }

        // Публичные методы для работы с файлами и папками
        public void CreateFile(string fileName)
        {
            if (string.IsNullOrEmpty(_currentPath)) return;

            string filePath = Path.Combine(_currentPath, fileName);
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                    OnFileCreated(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CreateFolder(string folderName)
        {
            if (string.IsNullOrEmpty(_currentPath)) return;

            string folderPath = Path.Combine(_currentPath, folderName);
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    OnFolderCreated(folderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании папки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteSelected()
        {
            var selectedNode = _treeView.SelectedNode;
            if (selectedNode?.Tag == null) return;

            string path = selectedNode.Tag.ToString()!;
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    OnFileDeleted(path);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    OnFolderDeleted(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RenameSelected(string newName)
        {
            var selectedNode = _treeView.SelectedNode;
            if (selectedNode?.Tag == null) return;

            string oldPath = selectedNode.Tag.ToString()!;
            string newPath = Path.Combine(Path.GetDirectoryName(oldPath)!, newName);

            try
            {
                if (File.Exists(oldPath))
                {
                    File.Move(oldPath, newPath);
                    OnFileRenamed(newPath);
                }
                else if (Directory.Exists(oldPath))
                {
                    Directory.Move(oldPath, newPath);
                    OnFolderRenamed(newPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переименовании: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 