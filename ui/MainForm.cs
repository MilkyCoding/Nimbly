namespace NimblyApp
{
    public class MainForm : Form
    {
        private HeaderComponent header;
        private EditorComponent editor;
        private PlaceholderComponent placeholder;
        private bool hasOpenFile = false;

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
            
            // Создаем компоненты
            header = new HeaderComponent();
            editor = new EditorComponent();
            placeholder = new PlaceholderComponent();

            // Подписываемся на событие создания нового файла
            placeholder.CreateNewFileClicked += OnCreateNewFile;

            // Добавляем компоненты в правильном порядке
            this.Controls.Add(editor);
            this.Controls.Add(placeholder);
            this.Controls.Add(header);

            // По умолчанию показываем плейсхолдер
            UpdateComponentsVisibility();
        }

        private void OnCreateNewFile(object sender, EventArgs e)
        {
            // Здесь будет логика создания нового файла
            hasOpenFile = true;
            UpdateComponentsVisibility();
        }

        private void UpdateComponentsVisibility()
        {
            editor.Visible = hasOpenFile;
            placeholder.Visible = !hasOpenFile;
        }
    }
}
