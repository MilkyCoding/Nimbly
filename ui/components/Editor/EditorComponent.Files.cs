using System.ComponentModel;

namespace NimblyApp
{
    public partial class EditorComponent
    {
        private string _currentFileName = "";
        private bool _isModified = false;
        private string? _currentFilePath = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentFileName
        {
            get => _currentFileName;
            private set
            {
                _currentFileName = value;
                _tabsComponent.UpdateActiveTabTitle(value);
                OnFileNameChanged();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsModified
        {
            get => _isModified;
            private set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    _tabsComponent.SetActiveTabModified(value);
                    OnFileNameChanged();
                }
            }
        }

        public event EventHandler? FileNameChanged;

        private void OnFileNameChanged()
        {
            FileNameChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetDisplayFileName()
        {
            return $"{CurrentFileName}{(IsModified ? "*" : "")}";
        }

        public void NewFile()
        {
            CreateNewTab();
            _tabsComponent?.UpdateDiscordPresence();
        }

        public void OpenFile(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                string fileName = Path.GetFileName(filePath);
                
                // Создаем новую вкладку
                CreateNewTab();
                
                // Обновляем содержимое и информацию о файле
                _textBox.Text = content;
                CurrentFileName = fileName;
                _currentFilePath = filePath;
                IsModified = false;

                // Сохраняем исходное состояние в TabInfo
                if (_tabsComponent != null)
                {
                    _tabsComponent.SaveCurrentContent(content, filePath);
                    _tabsComponent.SetActiveTabModified(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _tabsComponent?.UpdateDiscordPresence();
        }

        public void SaveFile()
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SaveFileAs();
                return;
            }

            try
            {
                string content = _textBox.Text;
                File.WriteAllText(_currentFilePath, content);
                CurrentFileName = Path.GetFileName(_currentFilePath);
                IsModified = false;

                // Обновляем исходное состояние после сохранения
                if (_tabsComponent != null)
                {
                    _tabsComponent.SaveCurrentContent(content, _currentFilePath);
                    _tabsComponent.SetActiveTabModified(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _tabsComponent?.UpdateDiscordPresence();
        }

        public void SaveFileAs()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string content = _textBox.Text;
                        File.WriteAllText(saveFileDialog.FileName, content);
                        CurrentFileName = Path.GetFileName(saveFileDialog.FileName);
                        _currentFilePath = saveFileDialog.FileName;
                        IsModified = false;

                        // Обновляем исходное состояние после сохранения
                        if (_tabsComponent != null)
                        {
                            _tabsComponent.SaveCurrentContent(content, _currentFilePath);
                            _tabsComponent.SetActiveTabModified(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            _tabsComponent?.UpdateDiscordPresence();
        }
    }
} 