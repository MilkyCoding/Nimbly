using System.Runtime.InteropServices;

namespace NimblyApp
{
    public partial class EditorComponent : UserControl
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int EM_GETFIRSTVISIBLELINE = 0xCE;
        internal readonly TextBox _textBox;
        internal readonly Panel _lineNumberPanel;
        internal int _lineNumberWidth = 45;
        private readonly TabsComponent _tabsComponent;
        private readonly Panel _editorContainer;

        public EditorComponent()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#1e1e1e");
            this.Padding = new Padding(0);

            // Создаем компонент вкладок
            _tabsComponent = new TabsComponent
            {
                Dock = DockStyle.Top
            };

            // Создаем контейнер для редактора и номеров строк
            _editorContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#1e1e1e"),
                Padding = new Padding(0)
            };

            // Панель для номеров строк
            _lineNumberPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = _lineNumberWidth,
                BackColor = ColorTranslator.FromHtml("#2d2d30"),
                BorderStyle = BorderStyle.None
            };
            _lineNumberPanel.Paint += LineNumberPanel_Paint;

            // Основное текстовое поле
            _textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#1e1e1e"),
                ForeColor = Color.White,
                Font = new Font("Consolas", 12),
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.Vertical,
                AcceptsReturn = true,
                AcceptsTab = true,
                WordWrap = false
            };

            // Привязка событий
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.FontChanged += TextBox_FontChanged;
            _textBox.Resize += TextBox_Resize;
            _textBox.KeyDown += TextBox_KeyDown;

            // Подписываемся на события вкладок
            _tabsComponent.TabSwitched += TabsComponent_TabSwitched;
            _tabsComponent.NewTabCreated += TabsComponent_NewTabCreated;
            _tabsComponent.ContentRequested += TabsComponent_ContentRequested;

            // Собираем структуру контролов
            _editorContainer.Controls.Add(_textBox);
            _editorContainer.Controls.Add(_lineNumberPanel);

            this.Controls.Add(_editorContainer);
            this.Controls.Add(_tabsComponent);

            // Инициализация после добавления контролов
            this.Load += (s, e) =>
            {
                UpdateLineNumberWidth();
                _lineNumberPanel.Invalidate();
                InitializeTabsHandling();
            };
        }

        private void TabsComponent_TabSwitched(object? sender, TabsComponent.TabEventArgs e)
        {
            _textBox.Text = e.Content;
            CurrentFileName = e.Title;
            IsModified = e.IsModified;
        }

        private void TabsComponent_NewTabCreated(object? sender, TabsComponent.TabEventArgs e)
        {
            _textBox.Text = e.Content;
            CurrentFileName = e.Title;
            IsModified = e.IsModified;
        }

        private void TabsComponent_ContentRequested(object? sender, EventArgs e)
        {
            _tabsComponent.SaveCurrentContent(_textBox.Text);
        }

        public void InitializeTabs()
        {
            if (_tabsComponent != null)
            {
                _tabsComponent.Initialize();
            }
        }

        public void CreateNewTab()
        {
            _tabsComponent.AddNewTab();
        }
    }
}