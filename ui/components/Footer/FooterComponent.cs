using System.Windows.Forms;
using System.Drawing;

namespace NimblyApp
{
    public class FooterComponent : UserControl
    {
        public FooterComponent()
        {
            this.Dock = DockStyle.Bottom;
            this.Height = 24;
            this.BackColor = ColorTranslator.FromHtml("#232323");

            var versionLabel = new Label
            {
                Text = "Nimbly v1.0.0",
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Width = 120,
                Padding = new Padding(10, 5, 10, 5)
            };
            this.Controls.Add(versionLabel);
        }
    }
} 