using System.Reflection;
using System.Diagnostics;

namespace NimblyApp
{
    public static class ResourceUtils
    {
        public static Image GetIcon(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullResourceName = $"NimblyApp.{resourceName}";
            
            // Debug: List all available resources
            var resources = assembly.GetManifestResourceNames();
            Trace.WriteLine("Available resources:");
            foreach (var resource in resources)
            {
                Trace.WriteLine($"- {resource}");
            }
            
            using var stream = assembly.GetManifestResourceStream(fullResourceName);
            if (stream == null)
            {
                var error = $"Resource {fullResourceName} not found. Available resources: {string.Join(", ", resources)}";
                Trace.WriteLine(error);
                throw new Exception(error);
            }
            return Image.FromStream(stream);
        }
    }
} 