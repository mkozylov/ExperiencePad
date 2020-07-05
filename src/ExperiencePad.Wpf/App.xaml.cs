using ExperiencePad.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NWrath.Synergy.Common.Extensions;
using ExperiencePad.Logic;

namespace ExperiencePad
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow();

            var injector = ConfigureServices(window);

            injector.BindProperties(window);

            window.DataContext = injector.GetRequiredService<MainDataContext>();
            
            window.ShowDialog();
        }

        private ServiceProvider ConfigureServices(MainWindow window)
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainDataContext>();
            services.AddSingleton(window);
            services.AddSingleton(new StorageDbContext("Data Source=storage.db"));
            services.AddSingleton<DataManager>();

            var injector = services.BuildServiceProvider();

            return injector;
        }
    }
}
