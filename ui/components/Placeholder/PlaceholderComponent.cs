using System;
using System.Drawing;
using System.Windows.Forms;

namespace NimblyApp
{
    public partial class PlaceholderComponent : Panel
    {
        private Button createNewFileButton;
        private Label messageLabel;
        private Label iconLabel;
        private Label hintLabel;

        public event EventHandler CreateNewFileClicked;

        public PlaceholderComponent()
        {
            InitializeComponent();
            InitializeEvents();
        }
    }
} 