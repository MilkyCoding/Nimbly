using NimblyApp.Services;
using System.Windows.Forms;
using System.IO;

namespace NimblyApp
{
    public partial class EditorComponent
    {
        public event EventHandler? EditorEmpty;

        private void InitializeTabsHandling()
        {
            if (_tabsComponent == null) return;

            // Подписываемся на события вкладок
            _tabsComponent.TabSwitched += OnTabSwitched;
            _tabsComponent.ContentRequested += OnContentRequested;
            _tabsComponent.NewTabCreated += OnNewTabCreated;
            _tabsComponent.TabClosed += OnTabClosed;
        }

        private void OnTabSwitched(object? sender, TabsComponent.TabEventArgs e)
        {
            // Показываем редактор при переключении на вкладку
            _textEditorContainer.Visible = true;
            
            // Загружаем содержимое вкладки
            _textBox.Text = e.Content;
            _currentFilePath = e.FilePath;
            _currentFileName = e.Title;
            _isModified = e.IsModified;
            OnFileNameChanged();
        }

        private void OnContentRequested(object? sender, EventArgs e)
        {
            if (_tabsComponent != null)
            {
                _tabsComponent.SaveCurrentContent(_textBox.Text, _currentFilePath);
            }
        }

        private void OnTabClosed(object? sender, TabsComponent.TabInfo tab)
        {
            // Если это была последняя вкладка, очищаем редактор
            if (_tabsComponent.TabCount == 0)
            {
                _textBox.Text = "";
                _textEditorContainer.Visible = false;
                _currentFileName = "";
                _currentFilePath = null;
                _isModified = false;
                OnFileNameChanged();
                EditorEmpty?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnNewTabCreated(object? sender, TabsComponent.TabEventArgs e)
        {
            // Показываем редактор при создании новой вкладки
            _textEditorContainer.Visible = true;
            _textBox.Text = "";
            _currentFileName = e.Title;
            _currentFilePath = null;
            _isModified = false;
            OnFileNameChanged();
        }
    }
} 