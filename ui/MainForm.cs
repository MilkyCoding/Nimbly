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
        private LoginComponent login;
        private bool hasOpenFolder = false;
        private bool isAuthenticated = false;

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
            login = new LoginComponent();

            // Связываем компоненты
            header.SetEditor(editor);

            // Подписываемся на событие создания нового файла
            placeholder.CreateNewFileClicked += OnCreateNewFile;
            // Подписываемся на событие открытия папки
            placeholder.OpenFolderClicked += OnOpenFolder;

            // Подписываемся на событие смены файла в редакторе
            editor.FileNameChanged += OnEditorFileNameChanged;

            // Подписываемся на событие успешной авторизации
            login.LoginClicked += LoginComponent_LoginSuccessful;

            // Подписываемся на изменение цветов
            ThemeColors.ColorsChanged += ThemeColors_ColorsChanged;

            // Добавляем компоненты в правильном порядке
            this.Controls.Add(editor);
            this.Controls.Add(placeholder);
            this.Controls.Add(login);
            this.Controls.Add(header);
            this.Controls.Add(footer);

            // По умолчанию показываем форму авторизации
            UpdateComponentsVisibility();
            UpdateDiscordPresence();

            // Инициализируем вкладки после создания всех компонентов
            editor.InitializeTabs();
        }

        private void LoginComponent_LoginSuccessful(object? sender, LoginEventArgs e)
        {
            isAuthenticated = true;
            UpdateComponentsVisibility();
            UpdateDiscordPresence();
        }

        private void ThemeColors_ColorsChanged(object? sender, EventArgs e)
        {
            // Обновляем цвета всех компонентов
            this.BackColor = ThemeColors.MainBackground;
            header.Refresh();
            editor.Refresh();
            placeholder.Refresh();
            footer.Refresh();
            login.Refresh();
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
            // Показываем компонент авторизации, если пользователь не авторизован
            login.Visible = !isAuthenticated;
            
            // Остальные компоненты показываем только после авторизации
            bool showMainComponents = isAuthenticated;
            header.Visible = showMainComponents;
            footer.Visible = showMainComponents;
            
            // Редактор или плейсхолдер показываем в зависимости от состояния
            if (showMainComponents)
            {
                editor.Visible = hasOpenFolder;
                placeholder.Visible = !hasOpenFolder;
            }
            else
            {
                editor.Visible = false;
                placeholder.Visible = false;
            }
        }

        private void UpdateDiscordPresence()
        {
            if (!isAuthenticated)
            {
                DiscordRPCService.UpdatePresence("Login", "Authenticating...");
            }
            else if (!hasOpenFolder)
            {
                DiscordRPCService.UpdatePresence("Idle", "Ready to code");
            }
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

        private void OnEditorFileNameChanged(object? sender, EventArgs e)
        {
            footer.SetExtension(Path.GetExtension(editor.CurrentFileName));
            UpdateDiscordPresence();
        }
    }
}
