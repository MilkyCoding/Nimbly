using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NimblyApp
{
    public partial class PlaceholderComponent
    {
        private Color buttonDefaultColor = ThemeColors.PlaceholderButton;
        private Color buttonHoverColor = ThemeColors.PlaceholderButtonHover;

        private void InitializeEvents()
        {
            this.Resize += PlaceholderComponent_Resize;
            
            // Добавляем эффекты при наведении на кнопку
            createNewFileButton.MouseEnter += (s, e) => {
                createNewFileButton.BackColor = buttonHoverColor;
                this.Cursor = Cursors.Hand;
            };
            
            createNewFileButton.MouseLeave += (s, e) => {
                createNewFileButton.BackColor = buttonDefaultColor;
                this.Cursor = Cursors.Default;
            };

            createNewFileButton.Click += (s, e) => {
                AnimateButtonClick();
                CreateNewFileClicked?.Invoke(this, EventArgs.Empty);
            };
        }

        private void AnimateButtonClick()
        {
            var originalColor = createNewFileButton.BackColor;
            createNewFileButton.BackColor = ThemeColors.PlaceholderButtonActive;
            
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += (s, e) => {
                createNewFileButton.BackColor = originalColor;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        private void PlaceholderComponent_Resize(object sender, EventArgs e)
        {
            UpdateControlsPosition();
        }

        private void UpdateControlsPosition()
        {
            int centerY = this.Height / 2;
            int spacing = 20;

            // Позиционируем иконку
            iconLabel.Size = new Size(120, 120);
            iconLabel.Location = new Point(
                (this.Width - iconLabel.Width) / 2,
                centerY - 160
            );

            // Позиционируем основной текст
            messageLabel.Size = new Size(this.Width, 40);
            messageLabel.Location = new Point(
                0,
                iconLabel.Bottom + spacing
            );

            // Позиционируем текст подсказки
            hintLabel.Size = new Size(this.Width, 30);
            hintLabel.Location = new Point(
                0,
                messageLabel.Bottom + spacing / 2
            );

            // Позиционируем кнопку
            createNewFileButton.Location = new Point(
                (this.Width - createNewFileButton.Width) / 2,
                hintLabel.Bottom + spacing * 2
            );
        }
    }
} 