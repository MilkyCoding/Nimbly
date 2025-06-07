using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace NimblyApp
{
    public partial class LoginComponent
    {

        private class TransparentTextBox : TextBox
        {
            public TransparentTextBox()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;
                BorderStyle = BorderStyle.None;
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                // Не рисуем фон, чтобы сохранить прозрачность
                if (BackColor == Color.Transparent)
                {
                    e.Graphics.Clear(Parent?.BackColor ?? SystemColors.Control);
                }
                else
                {
                    base.OnPaintBackground(e);
                }
            }
        }

        private void InitializeComponent()
        {

            this.Dock = DockStyle.Fill;
            this.BackColor = ThemeColors.MainBackground;

            // Создаем основную панель
            mainPanel = new Panel
            {
                Size = new Size(400, 600),
                BackColor = ThemeColors.DarkGrayColor,
                Anchor = AnchorStyles.None,
                Padding = new Padding(0)
            };

            // Создаем FlowLayoutPanel для вертикального расположения групп
            var flowLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 40, 0, 0),
                BackColor = Color.Transparent
            };

            // 1. Группа с логотипом
            var logoPanel = new Panel
            {
                Width = mainPanel.Width,
                Height = 120,
                Margin = new Padding(0, 0, 0, 40),
                BackColor = Color.Transparent
            };

            var logoBox = new PictureBox
            {
                Size = new Size(120, 120),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = ResourceUtils.GetIcon("ui.assets.logo.png"),
                BackColor = Color.Transparent
            };
            logoBox.Location = new Point((logoPanel.Width - logoBox.Width) / 2, 0);
            logoPanel.Controls.Add(logoBox);

            // 2. Группа с заголовком и подзаголовком
            var headerPanel = new Panel
            {
                Width = mainPanel.Width,
                Height = 80,
                Margin = new Padding(0, 0, 0, 40),
                BackColor = Color.Transparent
            };

            titleLabel = new Label
            {
                Text = "Nimbly - Log in",
                Font = new Font("Consolas", 24, FontStyle.Bold),
                ForeColor = ThemeColors.WhiteColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            var subtitleLabel = new Label
            {
                Text = "Войдите, для продолжения",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = ThemeColors.PlaceholderText,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 30
            };

            headerPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel });

            // 3. Группа с полями ввода
            var inputsPanel = new Panel
            {
                Width = mainPanel.Width - 80,
                Height = 115,
                Margin = new Padding(40, 0, 40, 40),
                BackColor = Color.Transparent
            };

            usernameContainer = CreateTextBoxContainer("Username", false);
            usernameContainer.Dock = DockStyle.Top;
            usernameTextBox = (TextBox)usernameContainer.Controls[0];

            passwordContainer = CreateTextBoxContainer("Password", true);
            passwordContainer.Dock = DockStyle.Bottom;
            passwordTextBox = (TextBox)passwordContainer.Controls[0];

            inputsPanel.Controls.AddRange(new Control[] { usernameContainer, passwordContainer });

            // 4. Группа с кнопкой и ссылкой
            var buttonsPanel = new Panel
            {
                Width = mainPanel.Width - 40,
                Height = 85,
                Margin = new Padding(20, 0, 20, 0),
                BackColor = Color.Transparent
            };

            loginButton = new Button
            {
                Text = "Войти",
                FlatStyle = FlatStyle.Flat,
                BackColor = ThemeColors.PlaceholderButton,
                ForeColor = ThemeColors.WhiteColor,
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Regular),
                Size = new Size(mainPanel.Width - 40, 50),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Top
            };

            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Region = CreateRoundRectRegion(loginButton.Width, loginButton.Height, 8);

            var registerLink = new LinkLabel
            {
                Text = "Зарегистрироваться",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 25,
                Dock = DockStyle.Bottom,
                LinkColor = ThemeColors.PlaceholderButton,
                ActiveLinkColor = ThemeColors.PlaceholderButtonHover,
                VisitedLinkColor = ThemeColors.PlaceholderButton
            };

            registerLink.Click += (s, e) =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://nimbly.one/register",
                    UseShellExecute = true
                });
            };

            buttonsPanel.Controls.AddRange(new Control[] { loginButton, registerLink });

            // Добавляем все группы в FlowLayoutPanel
            flowLayout.Controls.AddRange(new Control[] {
                logoPanel,
                headerPanel,
                inputsPanel,
                buttonsPanel
            });

            // Добавляем FlowLayoutPanel в основную панель
            mainPanel.Controls.Add(flowLayout);

            // Добавляем основную панель на форму и центрируем её
            this.Controls.Add(mainPanel);
            CenterMainPanel();

            // Обработчик изменения размера формы
            this.Resize += (s, e) => CenterMainPanel();
        }

        private void CenterMainPanel()
        {
            if (mainPanel != null)
            {
                mainPanel.Location = new Point(
                    (this.ClientSize.Width - mainPanel.Width) / 2,
                    (this.ClientSize.Height - mainPanel.Height) / 2
                );
            }
        }

        private Panel CreateTextBoxContainer(string labelText, bool isPassword = false)
        {
            var container = new Panel
            {
                Size = new Size(mainPanel.Width - 50, 50),
                BackColor = ThemeColors.DarkGrayColor,
                Padding = new Padding(15, 0, 15, 0),
                Margin = new Padding(15, 0, 15, 0)
            };

            // Label for the input field
            var label = new Label
            {
                Text = labelText.ToUpper(),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = ThemeColors.PlaceholderText,
                Location = new Point(0, 8),
                AutoSize = true
            };

            var textBox = new TransparentTextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(container.Width, 24),
                Location = new Point(0, 22),
                BorderStyle = BorderStyle.None,
                PlaceholderText = "",
                UseSystemPasswordChar = isPassword,
                BackColor = ThemeColors.DarkGrayColor,
                ForeColor = ThemeColors.WhiteColor
            };

            // Отключаем стандартное выделение при фокусе
            textBox.GotFocus += (s, e) =>
            {
                NativeWinAPI.HideCaret(textBox.Handle);
                container.BackColor = ThemeColors.DarkLightColor;
                textBox.BackColor = ThemeColors.DarkLightColor;
                container.Invalidate();
            };

            textBox.LostFocus += (s, e) =>
            {
                container.BackColor = ThemeColors.DarkGrayColor;
                textBox.BackColor = ThemeColors.DarkGrayColor;
                container.Invalidate();
            };

            // Custom border with rounded corners
            container.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw container background
                using (var path = new GraphicsPath())
                {
                    int radius = 8;
                    var bounds = container.ClientRectangle;
                    path.AddArc(bounds.X, bounds.Y, radius * 2, radius * 2, 180, 90);
                    path.AddArc(bounds.Right - radius * 2, bounds.Y, radius * 2, radius * 2, 270, 90);
                    path.AddArc(bounds.Right - radius * 2, bounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(bounds.X, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();

                    // Draw border
                    using (var pen = new Pen(ThemeColors.Border, 1))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }

                // Draw bottom line
                int lineY = textBox.Bottom + 2;
                using (var pen = new Pen(label.ForeColor, 1))
                {
                    e.Graphics.DrawLine(pen, 15, lineY, container.Width - 15, lineY);
                }
            };

            container.Controls.Add(textBox);
            container.Controls.Add(label);

            return container;
        }

        private Region CreateRoundRectRegion(int width, int height, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddArc(width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
            path.AddArc(width - radius * 2, height - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(0, height - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return new Region(path);
        }
    }
}