namespace NimblyApp
{
    public partial class HeaderComponent
    {
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
    }
} 