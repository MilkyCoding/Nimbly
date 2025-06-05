namespace NimblyApp
{
    public class MainForm : Form
    {
        private HeaderComponent header;
        private EditorComponent editor;

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
            
            // Создаем и добавляем компоненты в правильном порядке
            header = new HeaderComponent();
            editor = new EditorComponent();

            // Сначала добавляем редактор
            this.Controls.Add(editor);
            // Затем добавляем заголовок, чтобы он был поверх
            this.Controls.Add(header);
        }
    }
}
