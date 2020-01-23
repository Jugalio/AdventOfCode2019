using AdventOfCode.ViewModels;
using AdventOfCode.Views;
using Extension.Wpf.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AdventOfCode
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static ServiceProvider ServiceProvider;

        /// <summary>
        /// Called on applcation startup to set up the logger and all the
        /// dependencies and configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
            var main = ServiceProvider.GetService<MainWindow>();
            main.Show();
        }

        private void ConfigureServices(IServiceCollection collection)
        {
            collection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Debug);
                loggingBuilder.AddNLog();
            });

            collection.AddSingleton<MainWindow>();
            collection.AddTransient<IDialogService, DefaultDialogs>();
            collection.AddSingleton<MainWindowViewModel>();
        }
    }
}
