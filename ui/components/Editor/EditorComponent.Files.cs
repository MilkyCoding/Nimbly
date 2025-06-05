using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.ComponentModel;

namespace NimblyApp
{
    public partial class EditorComponent
    {
        private string _currentFileName = "New File";
        private bool _isModified = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentFileName
        {
            get => _currentFileName;
            private set
            {
                _currentFileName = value;
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
            _textBox.Clear();
            CurrentFileName = "New File";
            IsModified = false;
        }

        public void OpenFile(string filePath)
        {
            try
            {
                _textBox.Text = File.ReadAllText(filePath);
                CurrentFileName = Path.GetFileName(filePath);
                IsModified = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveFile(string filePath)
        {
            try
            {
                File.WriteAllText(filePath, _textBox.Text);
                CurrentFileName = Path.GetFileName(filePath);
                IsModified = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 