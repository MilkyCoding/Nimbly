using System;
using System.Drawing;
using System.Windows.Forms;

namespace NimblyApp
{
    public partial class PlaceholderComponent : UserControl
    {
        private Label messageLabel;
        private Label iconLabel;
        private Label hintLabel;
        private Button createNewFileButton;
        private Button openFolderButton;

        public event EventHandler? CreateNewFileClicked;
        public event EventHandler? OpenFolderClicked;

        public PlaceholderComponent()
        {
            InitializeComponent();
            InitializeEvents();

            // Подписываемся на изменение цветов
            ThemeColors.ColorsChanged += ThemeColors_ColorsChanged;
        }

        private void ThemeColors_ColorsChanged(object? sender, EventArgs e)
        {
            this.BackColor = ThemeColors.PlaceholderBackground;
            
            if (iconLabel != null)
                iconLabel.ForeColor = ThemeColors.PlaceholderIcon;
            
            if (messageLabel != null)
                messageLabel.ForeColor = ThemeColors.PlaceholderText;
            
            if (hintLabel != null)
                hintLabel.ForeColor = ThemeColors.PlaceholderHint;
            
            if (createNewFileButton != null)
            {
                createNewFileButton.BackColor = ThemeColors.PlaceholderButton;
                createNewFileButton.ForeColor = ThemeColors.WhiteColor;
            }
            
            if (openFolderButton != null)
            {
                openFolderButton.BackColor = ThemeColors.PlaceholderButton;
                openFolderButton.ForeColor = ThemeColors.WhiteColor;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeColors.ColorsChanged -= ThemeColors_ColorsChanged;
            }
            base.Dispose(disposing);
        }
    }
} 