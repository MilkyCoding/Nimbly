using NimblyApp.Services;

namespace NimblyApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Initialize Discord RPC
            DiscordRPCService.Initialize();
            DiscordRPCService.UpdatePresence("Idling", "Drink a cup of coffee.");
            
            Application.Run(new MainForm());
            
            // Cleanup Discord RPC when application closes
            DiscordRPCService.Shutdown();
        }
    }
} 