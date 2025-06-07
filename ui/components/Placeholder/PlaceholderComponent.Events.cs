using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NimblyApp
{
    public partial class PlaceholderComponent
    {
        private void InitializeEvents()
        {
            if (createNewFileButton == null || openFolderButton == null || 
                messageLabel == null || iconLabel == null || hintLabel == null)
                return;

            this.Resize += PlaceholderComponent_Resize!;
            
            // Добавляем эффекты при наведении на кнопку нового файла
            createNewFileButton.MouseEnter += (s, e) => {
                createNewFileButton.BackColor = ThemeColors.PlaceholderButtonHover;
                this.Cursor = Cursors.Hand;
            };
            
            createNewFileButton.MouseLeave += (s, e) => {
                createNewFileButton.BackColor = ThemeColors.PlaceholderButton;
                this.Cursor = Cursors.Default;
            };

            createNewFileButton.Click += (s, e) => {
                AnimateButtonClick(createNewFileButton);
                CreateNewFileClicked?.Invoke(this, EventArgs.Empty);
            };

            // Добавляем эффекты при наведении на кнопку открытия папки
            openFolderButton.MouseEnter += (s, e) => {
                openFolderButton.BackColor = ThemeColors.PlaceholderButtonHover;
                this.Cursor = Cursors.Hand;
            };
            
            openFolderButton.MouseLeave += (s, e) => {
                openFolderButton.BackColor = ThemeColors.PlaceholderButton;
                this.Cursor = Cursors.Default;
            };

            openFolderButton.Click += (s, e) => {
                AnimateButtonClick(openFolderButton);
                OpenFolderClicked?.Invoke(this, EventArgs.Empty);
            };
        }

        private void AnimateButtonClick(Button button)
        {
            button.BackColor = ThemeColors.PlaceholderButtonActive;
            
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += (s, e) => {
                button.BackColor = ThemeColors.PlaceholderButton;
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
            if (createNewFileButton == null || openFolderButton == null || 
                messageLabel == null || iconLabel == null || hintLabel == null)
                return;

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

            // Позиционируем кнопку нового файла
            createNewFileButton.Location = new Point(
                (this.Width - createNewFileButton.Width) / 2,
                hintLabel.Bottom + spacing
            );

            // Позиционируем кнопку открытия папки
            openFolderButton.Location = new Point(
                (this.Width - openFolderButton.Width) / 2,
                createNewFileButton.Bottom + spacing / 2
            );
        }
    }
} 