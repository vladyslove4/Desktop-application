using DesktopApp.Command;
using DesktopApp.DAL;
using DesktopApp.Domain;
using DesktopApp.Service;
using DesktopApp.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            IServiceCollection services = new ServiceCollection();

            services.AddDomainServices();

           services.AddDataAccessServices(configuration);

            services.AddTransient<DataSeeder>();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            
            services.AddSingleton<CoursesViewModel>();
            services.AddSingleton<GroupsViewModel>();
            services.AddSingleton<StudentsViewModel>();
            services.AddSingleton<TeachersViewModel>();
            services.AddSingleton<NavigationService, NavigationService>();
            services.AddSingleton<DataService>();
            
            services.AddSingleton((Func<IServiceProvider, Func<Type, ViewModelBase>>)(serviceProvider => viewModelType =>
            (ViewModelBase)serviceProvider.GetRequiredService(viewModelType)));

            _serviceProvider = services.BuildServiceProvider();

            Task.Run(MigrateAndSeedDatabaseAsync).Wait();
        }

        private async Task MigrateAndSeedDatabaseAsync()
        {
            await _serviceProvider.MigrateAndSeedDatabase();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}