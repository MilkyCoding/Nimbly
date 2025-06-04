using System.Windows.Forms;
using NimblyApp;
using System.Drawing;

namespace NimblyApp
{
    public class MainForm : Form
    {
        public MainForm()
        {
            // Настройка параметров окна
            this.Text = "Nimbly";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 800;
            this.Height = 600;
            this.Icon = new Icon("ui/assets/app.ico");

            // Убираем стандартный тайтл бар
            this.FormBorderStyle = FormBorderStyle.None;
            
            // Добавляем шапку (HeaderComponent) сверху окна
            var header = new HeaderComponent();
            this.Controls.Add(header);

            // Добавляем редактор
            var editor = new EditorComponent();
            this.Controls.Add(editor);
        }
    }
}
