using System.ComponentModel;
using System.Windows.Forms;

namespace NimblyApp
{
    public partial class TabsComponent : UserControl
    {
        public class TabInfo
        {
            private string _title;
            public string Title 
            { 
                get => _title;
                set
                {
                    _title = value;
                    UpdateDisplay();
                }
            }
            public string Content { get; set; }
            public bool IsModified { get; set; }
            public string? FilePath { get; set; }
            public Label TabLabel { get; }
            public Button CloseButton { get; }
            public int TabNumber { get; }
            public Panel Container { get => TabLabel.Parent as Panel ?? throw new InvalidOperationException("Tab label has no parent"); }

            public TabInfo(string title, Label label, Button closeButton, int tabNumber)
            {
                _title = title;
                Content = string.Empty;
                IsModified = false;
                FilePath = null;
                TabLabel = label;
                CloseButton = closeButton;
                TabNumber = tabNumber;
                UpdateDisplay();
            }

            public void UpdateDisplay()
            {
                if (TabLabel != null)
                {
                    TabLabel.Text = $"{_title}{(IsModified ? "*" : "")}";
                }
            }
        }

        private readonly FlowLayoutPanel _tabsPanel;
        private readonly Button _newTabButton;
        private readonly List<TabInfo> _tabs;
        private TabInfo? _activeTab;
        private int _nextTabNumber = 1;

        public event EventHandler<TabInfo>? TabClosed;
        public int TabCount => _tabs.Count;

        public TabsComponent()
        {
            _tabs = new List<TabInfo>();

            this.Dock = DockStyle.Top;
            this.Height = 30;
            this.BackColor = ThemeColors.TabPanel;
            this.Padding = new Padding(0);

            // Панель для вкладок
            _tabsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                BackColor = ThemeColors.TabPanel
            };

            // Кнопка новой вкладки
            _newTabButton = new Button
            {
                Text = "+",
                Width = 25,
                Height = this.Height,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                ForeColor = ThemeColors.WhiteColor,
                BackColor = ThemeColors.TabPanel,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _newTabButton.FlatAppearance.BorderSize = 0;
            _newTabButton.Click += NewTabButton_Click;

            this.Controls.Add(_newTabButton);
            this.Controls.Add(_tabsPanel);
        }

        public void Initialize()
        {
            // Добавляем вкладку "New" по умолчанию
            AddNewTab();
        }

        public void UpdateActiveTabTitle(string newTitle)
        {
            if (_activeTab != null)
            {
                _activeTab.Title = newTitle;
            }
        }

        public void SetActiveTabModified(bool isModified)
        {
            if (_activeTab != null)
            {
                _activeTab.IsModified = isModified;
                _activeTab.UpdateDisplay();
            }
        }

        private void NewTabButton_Click(object? sender, EventArgs e)
        {
            AddNewTab();
        }

        internal void CloseTab(TabInfo tab)
        {
            if (tab == null) return;

            // Если это активная вкладка, сначала деактивируем её
            if (tab == _activeTab)
            {
                _activeTab = null;
            }

            // Удаляем вкладку из UI и списка
            if (tab.Container.Parent != null)
            {
                tab.Container.Parent.Controls.Remove(tab.Container);
            }
            _tabs.Remove(tab);

            // Вызываем событие закрытия вкладки
            TabClosed?.Invoke(this, tab);

            // Если остались вкладки, активируем предыдущую
            if (_activeTab == null && _tabs.Count > 0)
            {
                ActivateTab(_tabs[0]);
            }
        }

        public void AddNewTab()
        {
            // Создаем контейнер для вкладки
            var tabContainer = new Panel
            {
                Height = this.Height - 2,
                Width = 120,
                Margin = new Padding(2, 2, 0, 0),
                BackColor = ThemeColors.TabInactive,
                Padding = new Padding(0),
                Cursor = Cursors.Hand
            };

            // Создаем кнопку вкладки (теперь это Label)
            var tabLabel = new Label
            {
                Text = $"New-{_nextTabNumber}",
                Height = this.Height - 2,
                Dock = DockStyle.Fill,
                ForeColor = ThemeColors.WhiteColor,
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 20, 0), // Добавляем отступ справа для крестика
                Cursor = Cursors.Hand
            };

            // Создаем кнопку закрытия
            var closeButton = new Button
            {
                Text = "×",
                Width = 20,
                Height = this.Height - 2,
                FlatStyle = FlatStyle.Flat,
                ForeColor = ThemeColors.WhiteColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            closeButton.FlatAppearance.BorderSize = 0;

            // Добавляем эффекты при наведении на вкладку
            tabContainer.MouseEnter += (s, e) => {
                if (tabContainer.BackColor != ThemeColors.TabActive)
                {
                    tabContainer.BackColor = ThemeColors.DarkLightColor;
                }
                closeButton.BackColor = Color.Transparent;
            };
            tabContainer.MouseLeave += (s, e) => {
                if (tabContainer.BackColor != ThemeColors.TabActive)
                {
                    tabContainer.BackColor = ThemeColors.TabInactive;
                }
            };

            // Добавляем эффекты при наведении на кнопку закрытия
            closeButton.MouseEnter += (s, e) => closeButton.BackColor = ThemeColors.DarkLightColor;
            closeButton.MouseLeave += (s, e) => closeButton.BackColor = Color.Transparent;

            // Добавляем элементы в контейнер
            tabContainer.Controls.Add(closeButton);
            tabContainer.Controls.Add(tabLabel);

            // Создаем информацию о вкладке (после добавления в контейнер)
            var tabInfo = new TabInfo($"New-{_nextTabNumber}", tabLabel, closeButton, _nextTabNumber);
            _nextTabNumber++;

            // Добавляем обработчики событий
            tabLabel.Click += (s, e) => ActivateTab(tabInfo);
            closeButton.Click += (s, e) => CloseTab(tabInfo);

            // Добавляем вкладку в список и UI
            _tabs.Add(tabInfo);
            _tabsPanel.Controls.Add(tabContainer);

            // Активируем новую вкладку
            ActivateTab(tabInfo);

            // Вызываем событие создания новой вкладки
            OnNewTabCreated(tabInfo);
        }
    }
} 