﻿namespace ParallelSearch
{
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using log4net;
    using log4net.Config;
    using ParallelSearch.Utils;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ILog logger = LogManager.GetLogger(typeof(App));

        /// <summary>
        /// Creates a new instance of <see cref="App"/>.
        /// </summary>
        public App()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4netConfig.xml"));
            logger.Debug("Starting ParallelSearch...");

            // Prepare dependency injection for the MainWindow and its MainViewModel.
            var listCreator = new ListCreator();

            var mainViewModel = new MainViewModel(listCreator);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();

            logger.Debug("ParallelSearch started.");
        }
    }
}