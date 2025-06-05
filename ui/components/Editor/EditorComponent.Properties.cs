using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace NimblyApp
{
    public partial class EditorComponent
    {
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
    }
} 