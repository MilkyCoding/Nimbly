using System.Windows.Forms;
using System.Drawing;

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
                fileMenu.Items.Add("New", null, (s2, e2) => { /* TODO: New file */ });
                fileMenu.Items.Add("Open", null, (s2, e2) => { /* TODO: Open file */ });
                fileMenu.Items.Add("Save", null, (s2, e2) => { /* TODO: Save file */ });
                fileMenu.Items.Add("Save As", null, (s2, e2) => { /* TODO: Save as */ });
                fileMenu.Show(control, new Point(menu.Width, menu.Items[0].Bounds.Y));
            });
            menu.Items.Add("Help", null, (s, e) => { /* TODO: Show help */ });
            menu.Show(control, new Point(0, control.Height));
        }
    }
} 