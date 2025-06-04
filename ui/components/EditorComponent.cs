using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace NimblyApp
{
    public class EditorComponent : UserControl
    {
        private TextBox textBox;

        public EditorComponent()
        {
            // Настройка компонента
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#1e1e1e");

            // Создаем текстовое поле
            textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#1e1e1e"),
                ForeColor = ThemeColors.WhiteColor,
                Font = new Font("Consolas", 12),
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.None,
                ReadOnly = false,
                AcceptsReturn = true,
                AcceptsTab = true,
                WordWrap = false
            };

            // Добавляем отступы для текста
            textBox.Margin = new Padding(10);
            textBox.Padding = new Padding(10);

            this.Controls.Add(textBox);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get => textBox.Text;
            set => textBox.Text = value;
        }
    }
}
