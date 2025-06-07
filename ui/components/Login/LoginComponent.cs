using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NimblyApp
{
    public partial class LoginComponent : UserControl
    {
        private Panel mainPanel;
        private Panel usernameContainer;
        private Panel passwordContainer;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button loginButton;
        private Label titleLabel;
        private Label messageLabel;

        public event EventHandler<LoginEventArgs>? LoginClicked;

        public LoginComponent()
        {
            InitializeComponent();
            InitializeEvents();

            // Подписываемся на изменение цветов
            ThemeColors.ColorsChanged += ThemeColors_ColorsChanged;
        }

        private void ThemeColors_ColorsChanged(object? sender, EventArgs e)
        {
            if (mainPanel != null)
                mainPanel.BackColor = ThemeColors.MainBackground;

            if (loginButton != null)
            {
                loginButton.BackColor = ThemeColors.PlaceholderButton;
                loginButton.ForeColor = ThemeColors.WhiteColor;
            }

            if (titleLabel != null)
                titleLabel.ForeColor = ThemeColors.WhiteColor;

            if (messageLabel != null)
                messageLabel.ForeColor = ThemeColors.PlaceholderText;

            // Style textboxes
            foreach (var textBox in new[] { usernameTextBox, passwordTextBox })
            {
                textBox.BackColor = ThemeColors.MainBackground;
                textBox.ForeColor = ThemeColors.WhiteColor;
            }
            
            // Style button
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.FlatAppearance.MouseOverBackColor = ThemeColors.PlaceholderButtonHover;
            loginButton.FlatAppearance.MouseDownBackColor = ThemeColors.PlaceholderButtonActive;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeColors.ColorsChanged -= ThemeColors_ColorsChanged;
            }
            base.Dispose(disposing);
        }

        private static class NativeWinAPI
        {
            [DllImport("user32.dll")]
            public static extern bool HideCaret(IntPtr hWnd);
        }
    }
} 