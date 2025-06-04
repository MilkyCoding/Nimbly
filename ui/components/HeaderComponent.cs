using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;

namespace NimblyApp
{
    public class HeaderComponent : UserControl
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
            this.Dock = DockStyle.Top; // Прикрепляем шапку к верхней части окна
            this.Height = 30; // Высота шапки
            this.BackColor = ColorTranslator.FromHtml("#2d2d2d"); // Цвет фона шапки
            this.BringToFront(); // Гарантируем, что шапка всегда будет поверх других компонентов

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
            this.CreateLogo();

            // Создаем кнопки управления окном
            this.CreateWindowControlButtons();
        }

        private void CreateLogo()
        {
            // Создаем контейнер для логотипа и текста
            var logoContainer = new Panel
            {
                Dock = DockStyle.Left,
                Height = 30,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Добавляем иконку
            var iconBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(24, 24),
                Location = new Point(8, 4),
                BackColor = Color.Transparent
            };

            // Загружаем иконку
            iconBox.Image = ResourceUtils.GetIcon("ui.assets.logo.png");
            logoContainer.Controls.Add(iconBox);

            // Добавляем текст
            var titleLabel = new Label
            {
                Text = "Nimbly",
                ForeColor = ThemeColors.WhiteColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Consolas", 13, FontStyle.Regular),
                Location = new Point(30, 5),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            logoContainer.Controls.Add(titleLabel);

            this.Controls.Add(logoContainer);
        }

        private void CreateWindowControlButtons()
        {
            // Создаем кнопку минимизации окна
            var minimizeButton = new Button
            {
                Text = "−",
                Dock = DockStyle.Right,
                Width = 30,
                BackColor = Color.Transparent,
                ForeColor = ThemeColors.LimeColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 13, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                FlatAppearance =
                {
                    BorderSize = 0,
                    MouseOverBackColor = ThemeColors.DarkLightColor,
                    MouseDownBackColor = ThemeColors.DarkLightColor
                }
            };

            minimizeButton.Click += (s, e) =>
            {
                var form = this.FindForm();
                if (form != null)
                {
                    form.WindowState = FormWindowState.Minimized;
                }
            };

            this.Controls.Add(minimizeButton);

            // Создаем кнопку максимизации окна
            var maximizeButton = new Button
            {
                Text = "□",
                Dock = DockStyle.Right,
                Width = 30,
                BackColor = Color.Transparent,
                ForeColor = ThemeColors.LimeColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 13, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                FlatAppearance =
                {
                    BorderSize = 0,
                    MouseOverBackColor = ThemeColors.DarkLightColor,
                    MouseDownBackColor = ThemeColors.DarkLightColor
                }
            };

            // Переключение между максимизированным и нормальным состоянием
            maximizeButton.Click += (s, e) =>
            {
                var form = this.FindForm();
                if (form != null)
                {
                    if (form.WindowState == FormWindowState.Maximized)
                    {
                        form.WindowState = FormWindowState.Normal;
                        form.Bounds = new Rectangle(100, 100, 800, 600);
                    }
                    else
                    {
                        form.WindowState = FormWindowState.Maximized;
                    }
                }
            };

            this.Controls.Add(maximizeButton);

            // Создаем кнопку закрытия
            var closeButton = new Button
            {
                Text = "×",
                Dock = DockStyle.Right,
                Width = 30,
                BackColor = Color.Transparent,
                ForeColor = ThemeColors.LimeColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 13, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                FlatAppearance =
                {
                    BorderSize = 0,
                    MouseOverBackColor = ThemeColors.DarkLightColor,
                    MouseDownBackColor = ThemeColors.DarkLightColor
                }
            };

            closeButton.Click += (s, e) =>
            {
                var form = this.FindForm();
                if (form != null)
                {
                    form.Close();
                }
            };

            this.Controls.Add(closeButton);
        }
    }
}