using AudioPlayer.Services;
using AudioPlayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AudioPlayer
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            services.AddSingleton<IMusicPlayer, MusicPlayer>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();

            Services = services.BuildServiceProvider();
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}