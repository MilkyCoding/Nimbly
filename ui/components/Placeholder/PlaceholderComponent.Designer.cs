using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NimblyApp
{
    public partial class PlaceholderComponent
    {
        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ThemeColors.PlaceholderBackground; // Более темный фон для контраста

            // Создаем и настраиваем иконку
            var iconLabel = new Label
            {
                Text = "📁",
                ForeColor = ThemeColors.PlaceholderIcon,
                Font = new Font("Segoe UI", 72, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(120, 120),
                Dock = DockStyle.None
            };

            // Создаем и настраиваем сообщение
            messageLabel = new Label
            {
                Text = "Файл не найден",
                ForeColor = ThemeColors.PlaceholderText,
                Font = new Font("Segoe UI Light", 26, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Dock = DockStyle.None
            };

            // Создаем и настраиваем подсказку
            var hintLabel = new Label
            {
                Text = "Создайте новый файл, чтобы начать работу",
                ForeColor = ThemeColors.PlaceholderHint,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Dock = DockStyle.None
            };

            // Создаем и настраиваем кнопку нового файла
            createNewFileButton = new Button
            {
                Text = "Создать новый файл",
                FlatStyle = FlatStyle.Flat,
                BackColor = ThemeColors.PlaceholderButton,
                ForeColor = ThemeColors.WhiteColor,
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Size = new Size(280, 50)
            };

            // Создаем и настраиваем кнопку открытия папки
            openFolderButton = new Button
            {
                Text = "Открыть папку",
                FlatStyle = FlatStyle.Flat,
                BackColor = ThemeColors.PlaceholderButton,
                ForeColor = ThemeColors.WhiteColor,
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Size = new Size(280, 50)
            };

            // Настраиваем внешний вид кнопок
            createNewFileButton.FlatAppearance.BorderSize = 0;
            createNewFileButton.Region = CreateRoundRectRegion(createNewFileButton.Width, createNewFileButton.Height, 8);
            
            openFolderButton.FlatAppearance.BorderSize = 0;
            openFolderButton.Region = CreateRoundRectRegion(openFolderButton.Width, openFolderButton.Height, 8);

            // Добавляем все элементы на панель
            this.Controls.Add(iconLabel);
            this.Controls.Add(messageLabel);
            this.Controls.Add(hintLabel);
            this.Controls.Add(createNewFileButton);
            this.Controls.Add(openFolderButton);

            // Сохраняем ссылки на дополнительные элементы для позиционирования
            this.iconLabel = iconLabel;
            this.hintLabel = hintLabel;

            UpdateControlsPosition();
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