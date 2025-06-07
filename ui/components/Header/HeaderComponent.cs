using System.Runtime.InteropServices;

namespace NimblyApp
{
    public partial class HeaderComponent : UserControl
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private EditorComponent? _editor;
        private Label _titleLabel;
        private Label _separator;
        private Panel _toolsPanel;

        public void SetEditor(EditorComponent editor)
        {
            _editor = editor;
        }

        public HeaderComponent()
        {
            // Настройка параметров шапки
            this.Dock = DockStyle.Top;
            this.Height = 30;
            this.BackColor = ThemeColors.TabPanel;
            this.BringToFront();

            // Добавляем обработчик мыши для перетаскивания окна
            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var form = this.FindForm();
                    if (form != null)
                    {
                        ReleaseCapture();
                        SendMessage(form.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    }
                }
            };

            // Создаём логотип
            CreateLogo();

            // Создаем кнопки управления окном
            CreateWindowControlButtons();

            // Подписываемся на изменение цветов
            ThemeColors.ColorsChanged += ThemeColors_ColorsChanged;
        }

        private void ThemeColors_ColorsChanged(object? sender, EventArgs e)
        {
            this.BackColor = ThemeColors.TabPanel;
            
            _titleLabel.ForeColor = ThemeColors.WhiteColor;
            _separator.ForeColor = ThemeColors.Separator;

            // Обновляем цвета кнопок в панели инструментов
            foreach (Control control in _toolsPanel.Controls)
            {
                if (control is Button button)
                {
                    button.ForeColor = ThemeColors.LimeColor;
                    button.FlatAppearance.MouseOverBackColor = ThemeColors.DarkLightColor;
                    button.FlatAppearance.MouseDownBackColor = ThemeColors.DarkLightColor;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeColors.ColorsChanged -= ThemeColors_ColorsChanged;
            }
            base.Dispose(disposing);
        }
    }
}