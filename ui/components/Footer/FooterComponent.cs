using System.Windows.Forms;
using System.Drawing;

namespace NimblyApp
{
    public class FooterComponent : UserControl
    {
        private readonly Label extensionLabel;
        private readonly Label versionLabel;

        public FooterComponent()
        {
            this.Dock = DockStyle.Bottom;
            this.Height = 24;
            this.BackColor = ThemeColors.FooterBackground;

            versionLabel = new Label
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

            // Подписываемся на изменение цветов
            ThemeColors.ColorsChanged += ThemeColors_ColorsChanged;
        }

        private void ThemeColors_ColorsChanged(object? sender, EventArgs e)
        {
            this.BackColor = ThemeColors.FooterBackground;
            extensionLabel.ForeColor = ThemeColors.FooterText;
            versionLabel.ForeColor = ThemeColors.FooterText;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeColors.ColorsChanged -= ThemeColors_ColorsChanged;
            }
            base.Dispose(disposing);
        }

        public void SetExtension(string ext)
        {
            extensionLabel.Text = string.IsNullOrEmpty(ext) ? string.Empty : $".{ext.ToLower()} file";
        }
    }
} 