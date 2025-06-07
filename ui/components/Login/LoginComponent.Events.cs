using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NimblyApp
{
    public partial class LoginComponent
    {
        private void InitializeEvents()
        {
            if (loginButton == null || usernameTextBox == null || passwordTextBox == null)
                return;

            this.Resize += LoginComponent_Resize!;

            // Добавляем эффекты при наведении на кнопку логина
            loginButton.MouseEnter += (s, e) => {
                loginButton.BackColor = ThemeColors.PlaceholderButtonHover;
                this.Cursor = Cursors.Hand;
            };
            
            loginButton.MouseLeave += (s, e) => {
                loginButton.BackColor = ThemeColors.PlaceholderButton;
                this.Cursor = Cursors.Default;
            };

            loginButton.Click += (s, e) => {
                AnimateButtonClick(loginButton);
                LoginClicked?.Invoke(this, new LoginEventArgs(usernameTextBox.Text));
            };

            // Добавляем навигацию по Enter
            usernameTextBox.KeyPress += (s, e) => {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    passwordTextBox.Focus();
                }
            };

            passwordTextBox.KeyPress += (s, e) => {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    loginButton.PerformClick();
                }
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

        private void LoginComponent_Resize(object sender, EventArgs e)
        {
            UpdateControlsPosition();
        }

        private void UpdateControlsPosition()
        {
            if (mainPanel == null || usernameContainer == null || passwordContainer == null || 
                loginButton == null || titleLabel == null)
                return;

            int padding = 25;
            int spacing = 15;

            // Обновляем размеры контейнеров
            usernameContainer.Size = new Size(mainPanel.Width - (padding * 2), 50);
            passwordContainer.Size = new Size(mainPanel.Width - (padding * 2), 50);
            loginButton.Size = new Size(mainPanel.Width - (padding * 2), 50);

            // Обновляем позиции
            titleLabel.Location = new Point(padding, padding);
            usernameContainer.Location = new Point(padding, titleLabel.Bottom + spacing);
            passwordContainer.Location = new Point(padding, usernameContainer.Bottom + spacing);
            loginButton.Location = new Point(padding, passwordContainer.Bottom + spacing * 2);
        }
    }

    public class LoginEventArgs : EventArgs
    {
        public string Username { get; }

        public LoginEventArgs(string username)
        {
            Username = username;
        }
    }
} 