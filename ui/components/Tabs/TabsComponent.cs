using System.ComponentModel;

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
            public Button TabButton { get; }

            public TabInfo(string title, Button button)
            {
                _title = title;
                Content = string.Empty;
                IsModified = false;
                TabButton = button;
                UpdateDisplay();
            }

            public void UpdateDisplay()
            {
                if (TabButton != null)
                {
                    TabButton.Text = $"{_title}{(IsModified ? "*" : "")}";
                }
            }
        }

        private readonly FlowLayoutPanel _tabsPanel;
        private readonly Button _newTabButton;
        private readonly List<TabInfo> _tabs;
        private TabInfo? _activeTab;
        private int _newFileCounter = 0;

        public TabsComponent()
        {
            _tabs = new List<TabInfo>();

            this.Dock = DockStyle.Top;
            this.Height = 30;
            this.BackColor = ColorTranslator.FromHtml("#2d2d2d");
            this.Padding = new Padding(0);

            // Панель для вкладок
            _tabsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#2d2d2d")
            };

            // Кнопка новой вкладки
            _newTabButton = new Button
            {
                Text = "+",
                Width = 25,
                Height = this.Height,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#2d2d2d"),
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

        public void AddNewTab()
        {
            string title = _newFileCounter == 0 ? "New" : $"New-{_newFileCounter}";
            _newFileCounter++;

            var tab = CreateTabButton();
            var tabInfo = new TabInfo(title, tab);
            _tabs.Add(tabInfo);
            
            tab.Click += (s, e) => ActivateTab(tabInfo);
            _tabsPanel.Controls.Add(tab);

            ActivateTab(tabInfo);
            OnNewTabCreated(tabInfo);
            UpdateDiscordPresence();
        }

        private Button CreateTabButton()
        {
            var button = new Button
            {
                Height = this.Height - 2,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3d3d3d"),
                Font = new Font("Consolas", 10),
                Padding = new Padding(10, 0, 10, 0),
                Margin = new Padding(1, 1, 0, 0),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }
    }
} 