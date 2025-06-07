using System.Drawing;
using System.Windows.Forms;

namespace NimblyApp.Forms
{
    public partial class ThemeSettingsForm : Form
    {
        private readonly ColorDialog colorDialog;
        private readonly TableLayoutPanel colorTable;
        private readonly Button resetButton;
        private readonly Button applyButton;
        private readonly Button cancelButton;

        public ThemeSettingsForm()
        {
            InitializeComponent();
            this.Text = "Theme Settings";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            colorDialog = new ColorDialog();

            // Create main layout
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(10)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));

            // Create color table
            colorTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 0,
                AutoScroll = true,
                Padding = new Padding(5)
            };
            colorTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            colorTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            colorTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            // Add color rows
            AddColorRow("Main Background", "MainBackground");
            AddColorRow("Dark Gray", "DarkGrayColor");
            AddColorRow("Dark Light", "DarkLightColor");
            AddColorRow("Tab Panel", "TabPanel");
            AddColorRow("Tab Inactive", "TabInactive");
            AddColorRow("Tab Active", "TabActive");
            AddColorRow("Separator", "Separator");
            AddColorRow("Line Number Panel", "LineNumberPanel");
            AddColorRow("Line Number Text", "LineNumberText");
            AddColorRow("Placeholder Background", "PlaceholderBackground");
            AddColorRow("Placeholder Icon", "PlaceholderIcon");
            AddColorRow("Placeholder Text", "PlaceholderText");
            AddColorRow("Placeholder Hint", "PlaceholderHint");
            AddColorRow("Placeholder Button", "PlaceholderButton");
            AddColorRow("Placeholder Button Hover", "PlaceholderButtonHover");
            AddColorRow("Placeholder Button Active", "PlaceholderButtonActive");
            AddColorRow("Footer Background", "FooterBackground");
            AddColorRow("Footer Text", "FooterText");

            // Create button panel
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(5)
            };

            // Create buttons
            resetButton = new Button
            {
                Text = "Reset to Defaults",
                Width = 120,
                Height = 30
            };
            resetButton.Click += ResetButton_Click;

            applyButton = new Button
            {
                Text = "Apply",
                Width = 100,
                Height = 30
            };
            applyButton.Click += ApplyButton_Click;

            cancelButton = new Button
            {
                Text = "Cancel",
                Width = 100,
                Height = 30
            };
            cancelButton.Click += CancelButton_Click;

            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(applyButton);
            buttonPanel.Controls.Add(resetButton);

            // Add controls to main layout
            mainLayout.Controls.Add(colorTable, 0, 0);
            mainLayout.Controls.Add(buttonPanel, 0, 1);

            this.Controls.Add(mainLayout);

            // Subscribe to color changes
            NimblyApp.ThemeColors.ColorsChanged += ThemeColors_ColorsChanged;
        }

        private void AddColorRow(string label, string colorPropertyName)
        {
            var row = colorTable.RowCount++;
            colorTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            var labelControl = new Label
            {
                Text = label,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0)
            };

            var previewPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            var button = new Button
            {
                Text = "Change",
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                Tag = colorPropertyName
            };
            button.Click += ColorButton_Click;

            colorTable.Controls.Add(labelControl, 0, row);
            colorTable.Controls.Add(previewPanel, 1, row);
            colorTable.Controls.Add(button, 2, row);

            // Set initial color
            var property = typeof(NimblyApp.ThemeColors).GetProperty(colorPropertyName);
            if (property != null)
            {
                var color = (Color)property.GetValue(null);
                previewPanel.BackColor = color;
            }
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is string colorPropertyName)
            {
                var property = typeof(NimblyApp.ThemeColors).GetProperty(colorPropertyName);
                if (property != null)
                {
                    var currentColor = (Color)property.GetValue(null);
                    colorDialog.Color = currentColor;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        NimblyApp.ThemeColors.SetColor(colorPropertyName, colorDialog.Color);
                    }
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            NimblyApp.ThemeColors.ResetToDefaults();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NimblyApp.ThemeColors.ResetToDefaults();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ThemeColors_ColorsChanged(object sender, EventArgs e)
        {
            // Update all color previews
            foreach (Control control in colorTable.Controls)
            {
                if (control is Panel previewPanel)
                {
                    var row = colorTable.GetRow(previewPanel);
                    var button = colorTable.GetControlFromPosition(2, row) as Button;
                    if (button?.Tag is string colorPropertyName)
                    {
                        var property = typeof(NimblyApp.ThemeColors).GetProperty(colorPropertyName);
                        if (property != null)
                        {
                            var color = (Color)property.GetValue(null);
                            previewPanel.BackColor = color;
                        }
                    }
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            NimblyApp.ThemeColors.ColorsChanged -= ThemeColors_ColorsChanged;
        }
    }
} 