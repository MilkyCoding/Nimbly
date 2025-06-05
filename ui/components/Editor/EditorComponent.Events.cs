using System;
using System.Windows.Forms;
using System.Drawing;

namespace NimblyApp
{
    public partial class EditorComponent
    {
        internal void TextBox_TextChanged(object? sender, EventArgs e)
        {
            UpdateLineNumberWidth();
            _lineNumberPanel.Invalidate();
            IsModified = true;
        }

        internal void TextBox_FontChanged(object? sender, EventArgs e)
        {
            UpdateLineNumberWidth();
            _lineNumberPanel.Invalidate();
        }

        internal void TextBox_Resize(object? sender, EventArgs e)
        {
            _lineNumberPanel.Invalidate();
        }

        internal void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Control && !e.Alt && !e.Shift)
            {
                // Отменяем стандартное поведение Tab
                e.SuppressKeyPress = true;

                // Вставляем 4 пробелов вместо символа табуляции
                _textBox.SelectedText = "    "; // 4 пробелов
            }
        }
    }
} 