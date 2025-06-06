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
        }

        private void OnTabSwitched(object? sender, TabsComponent.TabEventArgs e)
        {
            _textBox.Text = e.Content;
            CurrentFileName = e.Title;
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