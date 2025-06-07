namespace NimblyApp
{
    public static class ThemeColors
    {
        // Private backing fields
        private static Color _mainBackground = ColorTranslator.FromHtml("#1e1e1e");
        private static Color _darkGrayColor = ColorTranslator.FromHtml("#2d2d2d");
        private static Color _darkLightColor = ColorTranslator.FromHtml("#5c5c5c");
        private static Color _tabPanel = ColorTranslator.FromHtml("#2d2d2d");
        private static Color _tabInactive = ColorTranslator.FromHtml("#3d3d3d");
        private static Color _tabActive = ColorTranslator.FromHtml("#4d4d4d");
        private static Color _separator = ColorTranslator.FromHtml("#3e3e42");
        private static Color _lineNumberPanel = ColorTranslator.FromHtml("#2d2d30");
        private static Color _lineNumberText = ColorTranslator.FromHtml("#858585");
        private static Color _placeholderBackground = Color.FromArgb(18, 18, 18);
        private static Color _placeholderIcon = Color.FromArgb(80, 80, 80);
        private static Color _placeholderText = Color.FromArgb(180, 180, 180);
        private static Color _placeholderHint = Color.FromArgb(100, 100, 100);
        private static Color _placeholderButton = Color.FromArgb(0, 122, 204);
        private static Color _placeholderButtonHover = Color.FromArgb(0, 142, 234);
        private static Color _placeholderButtonActive = Color.FromArgb(0, 102, 184);
        private static Color _whiteColor = Color.White;
        private static Color _limeColor = ColorTranslator.FromHtml("#90EE90");
        private static Color _footerBackground = ColorTranslator.FromHtml("#232323");
        private static Color _footerText = Color.FromArgb(180, 180, 180);
        private static Color _border = Color.FromArgb(200, 200, 200);

        // Public properties
        public static Color MainBackground { get => _mainBackground; set => _mainBackground = value; }
        public static Color DarkGrayColor { get => _darkGrayColor; set => _darkGrayColor = value; }
        public static Color DarkLightColor { get => _darkLightColor; set => _darkLightColor = value; }
        public static Color TabPanel { get => _tabPanel; set => _tabPanel = value; }
        public static Color TabInactive { get => _tabInactive; set => _tabInactive = value; }
        public static Color TabActive { get => _tabActive; set => _tabActive = value; }
        public static Color Separator { get => _separator; set => _separator = value; }
        public static Color LineNumberPanel { get => _lineNumberPanel; set => _lineNumberPanel = value; }
        public static Color LineNumberText { get => _lineNumberText; set => _lineNumberText = value; }
        public static Color PlaceholderBackground { get => _placeholderBackground; set => _placeholderBackground = value; }
        public static Color PlaceholderIcon { get => _placeholderIcon; set => _placeholderIcon = value; }
        public static Color PlaceholderText { get => _placeholderText; set => _placeholderText = value; }
        public static Color PlaceholderHint { get => _placeholderHint; set => _placeholderHint = value; }
        public static Color PlaceholderButton { get => _placeholderButton; set => _placeholderButton = value; }
        public static Color PlaceholderButtonHover { get => _placeholderButtonHover; set => _placeholderButtonHover = value; }
        public static Color PlaceholderButtonActive { get => _placeholderButtonActive; set => _placeholderButtonActive = value; }
        public static Color WhiteColor { get => _whiteColor; set => _whiteColor = value; }
        public static Color LimeColor { get => _limeColor; set => _limeColor = value; }
        public static Color FooterBackground { get => _footerBackground; set => _footerBackground = value; }
        public static Color FooterText { get => _footerText; set => _footerText = value; }
        public static Color Border { get => _border; set => _border = value; }

        // Event for notifying when colors change
        public static event EventHandler? ColorsChanged;

        // Method to set color by name
        public static bool SetColor(string colorName, Color newColor)
        {
            var property = typeof(ThemeColors).GetProperty(colorName);
            if (property != null)
            {
                property.SetValue(null, newColor);
                ColorsChanged?.Invoke(null, EventArgs.Empty);
                return true;
            }
            return false;
        }

        // Method to set color by hex string
        public static bool SetColorFromHex(string colorName, string hexColor)
        {
            try
            {
                var color = ColorTranslator.FromHtml(hexColor);
                return SetColor(colorName, color);
            }
            catch
            {
                return false;
            }
        }

        // Method to reset all colors to default values
        public static void ResetToDefaults()
        {
            _mainBackground = ColorTranslator.FromHtml("#1e1e1e");
            _darkGrayColor = ColorTranslator.FromHtml("#2d2d2d");
            _darkLightColor = ColorTranslator.FromHtml("#5c5c5c");
            _tabPanel = ColorTranslator.FromHtml("#2d2d2d");
            _tabInactive = ColorTranslator.FromHtml("#3d3d3d");
            _tabActive = ColorTranslator.FromHtml("#4d4d4d");
            _separator = ColorTranslator.FromHtml("#3e3e42");
            _lineNumberPanel = ColorTranslator.FromHtml("#2d2d30");
            _lineNumberText = ColorTranslator.FromHtml("#858585");
            _placeholderBackground = Color.FromArgb(18, 18, 18);
            _placeholderIcon = Color.FromArgb(80, 80, 80);
            _placeholderText = Color.FromArgb(180, 180, 180);
            _placeholderHint = Color.FromArgb(100, 100, 100);
            _placeholderButton = Color.FromArgb(0, 122, 204);
            _placeholderButtonHover = Color.FromArgb(0, 142, 234);
            _placeholderButtonActive = Color.FromArgb(0, 102, 184);
            _whiteColor = Color.White;
            _limeColor = ColorTranslator.FromHtml("#90EE90");
            _footerBackground = ColorTranslator.FromHtml("#232323");
            _footerText = Color.FromArgb(180, 180, 180);
            _border = Color.FromArgb(200, 200, 200);

            ColorsChanged?.Invoke(null, EventArgs.Empty);
        }
    }
} 