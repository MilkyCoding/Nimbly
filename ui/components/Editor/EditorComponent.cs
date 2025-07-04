using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

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
        private readonly FileTreeComponent _fileTreeComponent;
        private readonly Panel _mainContentPanel;
        private readonly Splitter _fileTreeSplitter;
        private readonly Panel _textEditorContainer;

        public EditorComponent()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            this.Dock = DockStyle.Fill;
            this.BackColor = ThemeColors.MainBackground;
            this.Padding = new Padding(0);

            // Создаем компонент дерева файлов
            _fileTreeComponent = new FileTreeComponent
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = ThemeColors.MainBackground
            };

            // Создаем разделитель для дерева файлов
            _fileTreeSplitter = new Splitter
            {
                Dock = DockStyle.Left,
                Width = 4,
                BackColor = ThemeColors.Separator
            };

            // Создаем компонент вкладок
            _tabsComponent = new TabsComponent
            {
                Dock = DockStyle.Top
            };

            // Создаем основной контейнер для редактора и вкладок
            _mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.MainBackground
            };

            // Создаем контейнер для текстового редактора
            _textEditorContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.MainBackground,
                Padding = new Padding(0)
            };

            // Создаем контейнер для редактора и номеров строк
            _editorContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.MainBackground,
                Padding = new Padding(0)
            };

            // Панель для номеров строк
            _lineNumberPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = _lineNumberWidth,
                BackColor = ThemeColors.LineNumberPanel,
                BorderStyle = BorderStyle.None
            };
            _lineNumberPanel.Paint += LineNumberPanel_Paint;

            // Основное текстовое поле
            _textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                BackColor = ThemeColors.MainBackground,
                ForeColor = ThemeColors.WhiteColor,
                Font = new Font("Consolas", 12),
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.Vertical,
                AcceptsReturn = true,
                AcceptsTab = true,
                WordWrap = false
            };

            // Подписываемся на изменение цветов
            ThemeColors.ColorsChanged += ThemeColors_ColorsChanged;

            // Привязка событий
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.FontChanged += TextBox_FontChanged;
            _textBox.Resize += TextBox_Resize;
            _textBox.KeyDown += TextBox_KeyDown;

            // Подписываемся на события вкладок
            _tabsComponent.TabSwitched += TabsComponent_TabSwitched;
            _tabsComponent.NewTabCreated += TabsComponent_NewTabCreated;
            _tabsComponent.ContentRequested += TabsComponent_ContentRequested;

            // Подписываемся на события дерева файлов
            _fileTreeComponent.FileSelected += FileTreeComponent_FileSelected;

            // Собираем структуру контролов
            _editorContainer.Controls.Add(_textBox);
            _editorContainer.Controls.Add(_lineNumberPanel);
            
            // Добавляем редактор в контейнер
            _textEditorContainer.Controls.Add(_editorContainer);
            
            // Добавляем вкладки и контейнер редактора в основной контейнер
            _mainContentPanel.Controls.Add(_textEditorContainer);
            _mainContentPanel.Controls.Add(_tabsComponent);

            // Добавляем компоненты в главный контрол в правильном порядке
            this.Controls.Add(_mainContentPanel);
            this.Controls.Add(_fileTreeSplitter);
            this.Controls.Add(_fileTreeComponent);

            // Инициализация после добавления контролов
            this.Load += (s, e) =>
            {
                UpdateLineNumberWidth();
                _lineNumberPanel.Invalidate();
                InitializeTabsHandling();
            };
        }

        private void ThemeColors_ColorsChanged(object? sender, EventArgs e)
        {
            this.BackColor = ThemeColors.MainBackground;
            _mainContentPanel.BackColor = ThemeColors.MainBackground;
            _textEditorContainer.BackColor = ThemeColors.MainBackground;
            _editorContainer.BackColor = ThemeColors.MainBackground;
            _textBox.BackColor = ThemeColors.MainBackground;
            _textBox.ForeColor = ThemeColors.WhiteColor;
            _lineNumberPanel.BackColor = ThemeColors.LineNumberPanel;
            _fileTreeSplitter.BackColor = ThemeColors.Separator;
            _lineNumberPanel.Invalidate(); // Перерисовываем номера строк
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeColors.ColorsChanged -= ThemeColors_ColorsChanged;
            }
            base.Dispose(disposing);
        }

        private void FileTreeComponent_FileSelected(object? sender, string filePath)
        {
            OpenFile(filePath);
        }

        private void TabsComponent_TabSwitched(object? sender, TabsComponent.TabEventArgs e)
        {
            _textBox.Text = e.Content;
            CurrentFileName = e.Title;
            IsModified = e.IsModified;
            _currentFilePath = e.FilePath;
        }

        private void TabsComponent_NewTabCreated(object? sender, TabsComponent.TabEventArgs e)
        {
            _textBox.Text = e.Content;
            CurrentFileName = e.Title;
            IsModified = e.IsModified;
        }

        private void TabsComponent_ContentRequested(object? sender, EventArgs e)
        {
            if (_tabsComponent != null)
            {
                _tabsComponent.SaveCurrentContent(_textBox.Text, _currentFilePath);
            }
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

        public void LoadFolder(string path)
        {
            if (_fileTreeComponent != null)
            {
                _fileTreeComponent.LoadFolder(path);
            }
        }
    }
}