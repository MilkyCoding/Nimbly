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

        public HeaderComponent()
        {
            // Настройка параметров шапки
            this.Dock = DockStyle.Top;
            this.Height = 30;
            this.BackColor = ColorTranslator.FromHtml("#2d2d2d");
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
        }
    }
}