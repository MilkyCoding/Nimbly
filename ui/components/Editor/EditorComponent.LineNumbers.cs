namespace NimblyApp
{
    public partial class EditorComponent
    {
        internal void LineNumberPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var panel = sender as Panel;
            if (panel == null) return;

            // Очищаем фон
            g.Clear(ThemeColors.LineNumberPanel);

            // Рисуем разделительную линию
            using (var pen = new Pen(ThemeColors.Separator, 1))
            {
                g.DrawLine(pen, panel.Width - 1, 0, panel.Width - 1, panel.Height);
            }

            // Настройки шрифта и кисти
            using (var brush = new SolidBrush(ThemeColors.LineNumberText))
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

        internal void UpdateLineNumberWidth()
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

        internal int GetFirstVisibleLine()
        {
            return SendMessage(_textBox.Handle, EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero);
        }

        internal int GetVisibleLinesCount()
        {
            if (_textBox.Font == null || _textBox.ClientSize.Height <= 0) return 0;

            using (var g = _textBox.CreateGraphics())
            {
                var fontHeight = (int)Math.Ceiling(_textBox.Font.GetHeight(g));
                return (_textBox.ClientSize.Height / fontHeight) + 2; // +2 для частично видимых строк
            }
        }
    }
} 