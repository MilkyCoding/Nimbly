using System;
using System.Drawing;
using System.Windows.Forms;

namespace NimblyApp
{
    public partial class PlaceholderComponent : Panel
    {
        private Button? createNewFileButton;
        private Button? openFolderButton;
        private Label? messageLabel;
        private Label? iconLabel;
        private Label? hintLabel;

        public event EventHandler? CreateNewFileClicked;
        public event EventHandler? OpenFolderClicked;

        public PlaceholderComponent()
        {
            InitializeComponent();
            InitializeEvents();
        }
    }
} 