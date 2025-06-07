using NimblyApp.Services;
using System.Reflection;

namespace NimblyApp
{
    public class MainForm : Form
    {
        private HeaderComponent header;
        private EditorComponent editor;
        private PlaceholderComponent placeholder;
        private FooterComponent footer;
        private bool hasOpenFolder = false;

        public MainForm()
        {
            // Настройка параметров окна
            this.Text = "Nimbly";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 800;
            this.Height = 600;

            // Загружаем иконку из встроенных ресурсов
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NimblyApp.ui.assets.app.ico"))
            {
                if (stream != null)
                {
                    this.Icon = new Icon(stream);
                }
            }

            // Убираем стандартный тайтл бар
            this.FormBorderStyle = FormBorderStyle.None;
            
            // Создаем компоненты
            header = new HeaderComponent();
            editor = new EditorComponent();
            placeholder = new PlaceholderComponent();
            footer = new FooterComponent();

            // Связываем компоненты
            header.SetEditor(editor);

            // Подписываемся на событие создания нового файла
            placeholder.CreateNewFileClicked += OnCreateNewFile;
            // Подписываемся на событие открытия папки
            placeholder.OpenFolderClicked += OnOpenFolder;

            // Подписываемся на событие смены файла в редакторе
            editor.FileNameChanged += OnEditorFileNameChanged;

            // Подписываемся на изменение цветов
            ThemeColors.ColorsChanged += ThemeColors_ColorsChanged;

            // Добавляем компоненты в правильном порядке
            this.Controls.Add(editor);
            this.Controls.Add(placeholder);
            this.Controls.Add(header);
            this.Controls.Add(footer);

            // По умолчанию показываем плейсхолдер
            UpdateComponentsVisibility();
            UpdateDiscordPresence();

            // Инициализируем вкладки после создания всех компонентов
            editor.InitializeTabs();
        }

        private void ThemeColors_ColorsChanged(object? sender, EventArgs e)
        {
            // Обновляем цвета всех компонентов
            this.BackColor = ThemeColors.MainBackground;
            header.Refresh();
            editor.Refresh();
            placeholder.Refresh();
            footer.Refresh();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            ThemeColors.ColorsChanged -= ThemeColors_ColorsChanged;
        }

        private void OnCreateNewFile(object? sender, EventArgs e)
        {
            hasOpenFolder = true;
            UpdateComponentsVisibility();
            UpdateDiscordPresence();
        }

        private void UpdateComponentsVisibility()
        {
            editor.Visible = hasOpenFolder;
            placeholder.Visible = !hasOpenFolder;
        }

        private void UpdateDiscordPresence()
        {
            if (!hasOpenFolder)
            {
                DiscordRPCService.UpdatePresence("Idle", "Ready to code");
            }
        }

        private void OnEditorFileNameChanged(object? sender, EventArgs e)
        {
            string fileName = editor.CurrentFileName;
            string ext = string.Empty;
            if (!string.IsNullOrEmpty(fileName) && fileName.Contains('.'))
            {
                ext = fileName.Substring(fileName.LastIndexOf('.') + 1);
            }
            footer.SetExtension(ext);
        }

        private void OnOpenFolder(object? sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку для открытия";
                folderDialog.UseDescriptionForTitle = true;
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    hasOpenFolder = true;
                    UpdateComponentsVisibility();
                    // Загружаем содержимое папки в дерево файлов
                    editor.LoadFolder(folderDialog.SelectedPath);
                    UpdateDiscordPresence();
                }
            }
        }
    }
}
