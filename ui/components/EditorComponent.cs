using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace NimblyApp
{
    public class EditorComponent : UserControl
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);

        private readonly TextBox _textBox;

        public EditorComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#1e1e1e");
            this.Padding = new Padding(0);

            _textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.None,
                Location = new Point(45, 5),
                Size = new Size(ClientSize.Width - 45, ClientSize.Height - 10),
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


            this.Controls.Add(_textBox);
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get => _textBox.Text;
            set
            {
                _textBox.Text = value;
            }
        }
    }
}