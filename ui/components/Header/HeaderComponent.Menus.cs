namespace NimblyApp
{
    public partial class HeaderComponent
    {
        private void ShowSettingsMenu(Control control)
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Editor Settings", null, (s, e) => { /* TODO: Show editor settings */ });
            menu.Items.Add("Color Theme", null, (s, e) => { /* TODO: Show theme settings */ });
            menu.Show(control, new Point(0, control.Height));
        }

        private void ShowToolbarMenu(Control control)
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("File", null, (s, e) =>
            {
                var fileMenu = new ContextMenuStrip();
                fileMenu.Items.Add("New", null, (s2, e2) => { _editor?.NewFile(); });
                fileMenu.Items.Add("Open", null, (s2, e2) => 
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                        openFileDialog.FilterIndex = 1;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            _editor?.OpenFile(openFileDialog.FileName);
                        }
                    }
                });
                fileMenu.Items.Add("Save", null, (s2, e2) => { _editor?.SaveFile(); });
                fileMenu.Items.Add("Save As", null, (s2, e2) => { _editor?.SaveFileAs(); });
                fileMenu.Show(control, new Point(menu.Width, menu.Items[0].Bounds.Y));
            });
            menu.Items.Add("Help", null, (s, e) => { /* TODO: Show help */ });
            menu.Show(control, new Point(0, control.Height));
        }
    }
} 