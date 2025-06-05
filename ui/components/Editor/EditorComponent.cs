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
        private readonly Label _fileNameLabel;
        private readonly Panel _editorContainer;

        public EditorComponent()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#1e1e1e");
            this.Padding = new Padding(0);

            // Создаем метку для имени файла
            _fileNameLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 25,
                BackColor = ColorTranslator.FromHtml("#2d2d2d"),
                ForeColor = Color.White,
                Font = new Font("Consolas", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
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

            // Подписываемся на изменение имени файла
            this.FileNameChanged += (s, e) => _fileNameLabel.Text = GetDisplayFileName();

            // Собираем структуру контролов
            _editorContainer.Controls.Add(_textBox);
            _editorContainer.Controls.Add(_lineNumberPanel);

            this.Controls.Add(_editorContainer);
            this.Controls.Add(_fileNameLabel);

            // Инициализация после добавления контролов
            this.Load += (s, e) =>
            {
                UpdateLineNumberWidth();
                _lineNumberPanel.Invalidate();
                _fileNameLabel.Text = GetDisplayFileName();
            };
        }
    }

}