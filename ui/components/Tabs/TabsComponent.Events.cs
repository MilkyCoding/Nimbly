using System.ComponentModel;
using NimblyApp.Services;

namespace NimblyApp
{
    public partial class TabsComponent
    {
        public class TabEventArgs : EventArgs
        {
            public string Title { get; }
            public string Content { get; }
            public bool IsModified { get; }
            public string? FilePath { get; }

            public TabEventArgs(TabInfo tab)
            {
                Title = tab.Title;
                Content = tab.Content;
                IsModified = tab.IsModified;
                FilePath = tab.FilePath;
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
            TabSwitched?.Invoke(this, new TabEventArgs(tab));
            UpdateDiscordPresence();
        }

        protected virtual void OnNewTabCreated(TabInfo tab)
        {
            NewTabCreated?.Invoke(this, new TabEventArgs(tab));
        }

        protected virtual void OnContentRequested()
        {
            ContentRequested?.Invoke(this, EventArgs.Empty);
        }

        // Метод для сохранения текущего контента перед переключением вкладки
        public void SaveCurrentContent(string content, string? filePath)
        {
            if (_activeTab != null)
            {
                _activeTab.Content = content;
                _activeTab.IsModified = true;
                _activeTab.FilePath = filePath;
                _activeTab.UpdateDisplay();
            }
        }

        // Метод активации вкладки
        private void ActivateTab(TabInfo tab)
        {
            // Сначала запрашиваем текущий контент для сохранения
            OnContentRequested();

            if (_activeTab != null)
            {
                _activeTab.TabButton.BackColor = ThemeColors.TabInactive;
            }

            _activeTab = tab;
            _activeTab.TabButton.BackColor = ThemeColors.TabActive;
            
            OnTabSwitched(tab);
            UpdateDiscordPresence();
        }

        public void UpdateDiscordPresence()
        {
            if (_activeTab != null && this.Visible)
            {
                DiscordRPCService.UpdatePresence("Editing", $"File: {_activeTab.Title}");
            }
            else
            {
                DiscordRPCService.UpdatePresence("Idle", "No file open");
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            UpdateDiscordPresence();
        }
    }
} 