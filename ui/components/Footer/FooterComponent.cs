using System.Windows.Forms;
using System.Drawing;

namespace NimblyApp
{
    public class FooterComponent : UserControl
    {
        private readonly Label extensionLabel;

        public FooterComponent()
        {
            this.Dock = DockStyle.Bottom;
            this.Height = 24;
            this.BackColor = ThemeColors.FooterBackground;

            var versionLabel = new Label
            {
                Text = "Nimbly v1.0.0",
                ForeColor = ThemeColors.FooterText,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Width = 120,
                Padding = new Padding(10, 5, 10, 5)
            };
            this.Controls.Add(versionLabel);

            extensionLabel = new Label
            {
                Text = string.Empty,
                ForeColor = ThemeColors.FooterText,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Width = 80,
                Padding = new Padding(0, 5, 10, 5)
            };
            this.Controls.Add(extensionLabel);
        }

        public void SetExtension(string ext)
        {
            extensionLabel.Text = string.IsNullOrEmpty(ext) ? string.Empty : $".{ext.ToLower()} file";
        }
    }
} 