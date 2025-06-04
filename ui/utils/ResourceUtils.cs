using System.Drawing;
using System.Reflection;

namespace NimblyApp
{
    public static class ResourceUtils
    {
        public static Image? GetIcon(string resourceName)
        {
            try
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"NimblyApp.{resourceName}");
                return stream != null ? Image.FromStream(stream) : null;
            }
            catch
            {
                return null;
            }
        }
    }
} 