namespace NimblyApp
{
    public partial class HeaderComponent
    {
        private void CreateWindowControlButtons()
        {
            // Создаем панель для кнопок управления окном (справа)
            var controlButtonsPanel = new Panel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Создаем панель для настроек и тулбара (левее от кнопок управления)
            var toolsPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 80,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 0)
            };

            // Разделитель
            var separator = new Label
            {
                Text = "│",
                AutoSize = false,
                Width = 10,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Consolas", 12),
                ForeColor = ColorTranslator.FromHtml("#3e3e42"),
                BackColor = Color.Transparent,
                Dock = DockStyle.Right,
                Margin = new Padding(0)
            };

            CreateToolButtons(toolsPanel);
            CreateWindowButtons(controlButtonsPanel);

            // Добавляем панели на заголовок
            this.Controls.Add(toolsPanel);
            this.Controls.Add(separator);
            this.Controls.Add(controlButtonsPanel);
        }

        private void CreateToolButtons(Panel toolsPanel)
        {
            // Кнопка настроек
            var settingsButton = CreateHeaderButton("⚙", "Segoe UI Symbol");
            settingsButton.Click += (s, e) => ShowSettingsMenu(settingsButton);

            // Кнопка тулбара
            var toolbarButton = CreateHeaderButton("🔨", "Segoe UI Symbol");
            toolbarButton.Click += (s, e) => ShowToolbarMenu(toolbarButton);

            // Добавляем кнопки на панель инструментов
            toolsPanel.Controls.Add(toolbarButton);
            toolsPanel.Controls.Add(settingsButton);
        }

        private void CreateWindowButtons(Panel controlButtonsPanel)
        {
            var closeButton = CreateHeaderButton("×", "Consolas");
            closeButton.Click += (s, e) =>
            {
                var form = this.FindForm();
                if (form != null)
                {
                    form.Close();
                }
            };

            var maximizeButton = CreateHeaderButton("□", "Consolas");
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

            var minimizeButton = CreateHeaderButton("−", "Consolas");
            minimizeButton.Click += (s, e) =>
            {
                var form = this.FindForm();
                if (form != null)
                {
                    form.WindowState = FormWindowState.Minimized;
                }
            };

            controlButtonsPanel.Controls.Add(minimizeButton);
            controlButtonsPanel.Controls.Add(maximizeButton);
            controlButtonsPanel.Controls.Add(closeButton);
        }

        private Button CreateHeaderButton(string text, string fontFamily)
        {
            return new Button
            {
                Text = text,
                Width = 30,
                Height = 30,
                BackColor = Color.Transparent,
                ForeColor = ThemeColors.LimeColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(fontFamily, 13),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Right,
                Margin = new Padding(0),
                FlatAppearance =
                {
                    BorderSize = 0,
                    MouseOverBackColor = ThemeColors.DarkLightColor,
                    MouseDownBackColor = ThemeColors.DarkLightColor
                }
            };
        }
    }
}