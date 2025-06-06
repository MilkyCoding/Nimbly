using System.ComponentModel;

namespace NimblyApp
{
    public partial class TabsComponent
    {
        public class TabEventArgs : EventArgs
        {
            public string Title { get; }
            public string Content { get; }
            public bool IsModified { get; }

            public TabEventArgs(string title, string content, bool isModified)
            {
                Title = title;
                Content = content;
                IsModified = isModified;
            }
        }

        // Событие переключения вкладки (когда нужно загрузить контент в редактор)
        public event EventHandler<TabEventArgs>? TabSwitched;

        // Событие создания новой вкладки
        public event EventHandler<TabEventArgs>? NewTabCreated;

        // Событие для получения текущего контента из редактора перед переключением
        public event EventHandler? ContentRequested;

        protected virtual void OnTabSwitched(TabInfo tab)
        {
            TabSwitched?.Invoke(this, new TabEventArgs(
                tab.Title,
                tab.Content,
                tab.IsModified
            ));
        }

        protected virtual void OnNewTabCreated(TabInfo tab)
        {
            NewTabCreated?.Invoke(this, new TabEventArgs(
                tab.Title,
                tab.Content,
                tab.IsModified
            ));
        }

        protected virtual void OnContentRequested()
        {
            ContentRequested?.Invoke(this, EventArgs.Empty);
        }

        // Метод для сохранения текущего контента перед переключением вкладки
        public void SaveCurrentContent(string content)
        {
            if (_activeTab != null)
            {
                _activeTab.Content = content;
                _activeTab.IsModified = true;
                _activeTab.UpdateDisplay();
            }
        }

        // Переопределяем метод активации вкладки
        private void ActivateTab(TabInfo tab)
        {
            // Сначала запрашиваем текущий контент для сохранения
            OnContentRequested();

            if (_activeTab != null)
            {
                _activeTab.TabButton.BackColor = ColorTranslator.FromHtml("#3d3d3d");
            }

            _activeTab = tab;
            _activeTab.TabButton.BackColor = ColorTranslator.FromHtml("#4d4d4d");
            
            OnTabSwitched(tab);
        }
    }
} 