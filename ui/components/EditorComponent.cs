using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace NimblyApp
{
    public class EditorComponent : UserControl
    {

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_VSCROLL = 0x115;
        private const int EM_GETFIRSTVISIBLELINE = 0xCE;
        private readonly TextBox _textBox;
        private readonly Panel _lineNumberPanel;
        private int _lineNumberWidth = 45;

        public EditorComponent()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#1e1e1e");
            this.Padding = new Padding(0);

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
                Dock = DockStyle.None,
                Location = new Point(_lineNumberWidth, 0),
                Size = new Size(ClientSize.Width - _lineNumberWidth, ClientSize.Height),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
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
            _textBox.SizeChanged += (s, e) => _textBox.Size = new Size(ClientSize.Width - _lineNumberWidth, ClientSize.Height);
            _textBox.KeyDown += TextBox_KeyDown;

            this.Controls.Add(_lineNumberPanel);
            this.Controls.Add(_textBox);

            // Инициализация после добавления контролов
            this.Load += (s, e) =>
            {
                UpdateLineNumberWidth();
                _lineNumberPanel.Invalidate();
            };
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            UpdateLineNumberWidth();
            _lineNumberPanel.Invalidate();

            return;

        }

        private void LineNumberPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var panel = sender as Panel;
            if (panel == null) return;

            // Очищаем фон
            g.Clear(ColorTranslator.FromHtml("#2d2d30"));

            // Рисуем разделительную линию
            using (var pen = new Pen(ColorTranslator.FromHtml("#3e3e42"), 1))
            {
                g.DrawLine(pen, panel.Width - 1, 0, panel.Width - 1, panel.Height);
            }

            // Настройки шрифта и кисти
            using (var brush = new SolidBrush(ColorTranslator.FromHtml("#858585")))
            using (var currentLineBrush = new SolidBrush(ThemeColors.DarkLightColor))
            {
                var fontHeight = (float)Math.Ceiling(_textBox.Font.GetHeight(g));
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Center
                };

                // Получаем параметры для отрисовки
                int firstVisibleLine = GetFirstVisibleLine();
                int visibleLines = GetVisibleLinesCount();
                int totalLines = Math.Max(_textBox.Lines.Length, 1);
                int currentLine = _textBox.GetLineFromCharIndex(_textBox.SelectionStart);

                // Рисуем номера строк
                for (int i = 0; i < visibleLines && (firstVisibleLine + i) < totalLines; i++)
                {
                    int lineNumber = firstVisibleLine + i + 1;
                    float y = i * fontHeight;
                    var rect = new RectangleF(0, y, panel.Width, fontHeight);

                    // Подсветка текущей строки
                    if (firstVisibleLine + i == currentLine)
                    {
                        g.FillRectangle(currentLineBrush, rect);
                    }

                    // Номер строки
                    var textRect = new RectangleF(5, y, panel.Width - 10, fontHeight);
                    g.DrawString(lineNumber.ToString(), _textBox.Font, brush, textRect, format);
                }
            }
        }


        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            UpdateLineNumberWidth();
            _lineNumberPanel.Invalidate();
        }

        private void TextBox_FontChanged(object? sender, EventArgs e)
        {
            UpdateLineNumberWidth();
            _lineNumberPanel.Invalidate();
        }

        private void TextBox_Resize(object? sender, EventArgs e)
        {
            _lineNumberPanel.Invalidate();
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Control && !e.Alt && !e.Shift)
            {
                // Отменяем стандартное поведение Tab
                e.SuppressKeyPress = true;

                // Вставляем 4 пробелов вместо символа табуляции
                _textBox.SelectedText = "    "; // 4 пробелов
            }
        }

        private void UpdateLineNumberWidth()
        {
            int lineCount = Math.Max(_textBox.Lines.Length, 1);
            int digits = (int)Math.Floor(Math.Log10(lineCount)) + 1;

            // Минимум 2 цифры
            digits = Math.Max(digits, 2);

            string sampleText = new string('9', digits);
            var textSize = TextRenderer.MeasureText(sampleText, _textBox.Font);
            int newWidth = textSize.Width + 20; // отступы по бокам

            if (newWidth != _lineNumberWidth)
            {
                _lineNumberWidth = newWidth;
                _lineNumberPanel.Width = _lineNumberWidth;
                _textBox.Location = new Point(_lineNumberWidth, 0);
                _textBox.Size = new Size(ClientSize.Width - _lineNumberWidth, ClientSize.Height);
            }
        }

        private int GetFirstVisibleLine()
        {
            return SendMessage(_textBox.Handle, EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero);
        }

        private int GetVisibleLinesCount()
        {
            if (_textBox.Font == null || _textBox.ClientSize.Height <= 0) return 0;

            using (var g = _textBox.CreateGraphics())
            {
                var fontHeight = (int)Math.Ceiling(_textBox.Font.GetHeight(g));
                return (_textBox.ClientSize.Height / fontHeight) + 2; // +2 для частично видимых строк
            }
        }

        // Публичные свойства и методы
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get => _textBox.Text;
            set
            {
                _textBox.Text = value;
                UpdateLineNumberWidth();
                _lineNumberPanel.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font TextFont
        {
            get => _textBox.Font;
            set
            {
                _textBox.Font = value;
                UpdateLineNumberWidth();
                _lineNumberPanel.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool WordWrap
        {
            get => _textBox.WordWrap;
            set => _textBox.WordWrap = value;
        }

        public void SelectAll() => _textBox.SelectAll();
        public void Copy() => _textBox.Copy();
        public void Cut() => _textBox.Cut();
        public void Paste() => _textBox.Paste();
        public void Undo() => _textBox.Undo();
        public void Clear() => _textBox.Clear();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get => _textBox.SelectionStart;
            set => _textBox.SelectionStart = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get => _textBox.SelectionLength;
            set => _textBox.SelectionLength = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText
        {
            get => _textBox.SelectedText;
            set => _textBox.SelectedText = value;
        }

        public string[] Lines => _textBox.Lines;

        public new event EventHandler? TextChanged
        {
            add => _textBox.TextChanged += value;
            remove => _textBox.TextChanged -= value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _textBox?.Dispose();
                _lineNumberPanel?.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}