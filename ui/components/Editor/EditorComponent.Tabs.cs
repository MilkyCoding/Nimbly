using NimblyApp.Services;
using System.Windows.Forms;
using System.IO;

namespace NimblyApp
{
    public partial class EditorComponent
    {
        private void InitializeTabsHandling()
        {
            if (_tabsComponent == null) return;

            // Подписываемся на события вкладок
            _tabsComponent.TabSwitched += OnTabSwitched;
            _tabsComponent.ContentRequested += OnContentRequested;
            _tabsComponent.NewTabCreated += OnTabSwitched;
            _tabsComponent.TabClosed += OnTabClosed;
        }

        private void OnTabSwitched(object? sender, TabsComponent.TabEventArgs e)
        {
            _textBox.Text = e.Content;
            IsModified = e.IsModified;
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
            // Если вкладка была изменена, спрашиваем пользователя о сохранении
            if (tab.IsModified)
            {
                var result = MessageBox.Show(
                    $"Сохранить изменения в файле {tab.Title}?",
                    "Сохранение",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!string.IsNullOrEmpty(tab.FilePath))
                        {
                            File.WriteAllText(tab.FilePath, tab.Content);
                        }
                        else
                        {
                            using (var saveDialog = new SaveFileDialog())
                            {
                                saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                                saveDialog.FilterIndex = 1;
                                saveDialog.RestoreDirectory = true;

                                if (saveDialog.ShowDialog() == DialogResult.OK)
                                {
                                    File.WriteAllText(saveDialog.FileName, tab.Content);
                                }
                                else
                                {
                                    return; // Отменяем закрытие если пользователь отменил сохранение
                                }
                            }
                        }
                        break;

                    case DialogResult.Cancel:
                        return; // Отменяем закрытие
                }
            }

            // Если это была последняя вкладка, очищаем редактор
            if (_tabsComponent.TabCount == 1) // Проверяем, что это была последняя вкладка
            {
                _textBox.Text = string.Empty;
                CurrentFileName = string.Empty;
                _currentFilePath = null;
                IsModified = false;
            }
        }
    }
} 