using NimblyApp.Services;

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
            _tabsComponent.NewTabCreated += OnTabSwitched; // Обрабатываем новые вкладки так же
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
                _tabsComponent.SaveCurrentContent(_textBox.Text);
            }
        }
    }
} 