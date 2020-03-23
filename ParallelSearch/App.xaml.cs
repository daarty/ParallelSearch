namespace ParallelSearch
{
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using log4net;
    using log4net.Config;
    using ParallelSearchLibrary.List;
    using ParallelSearchLibrary.Trie;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(App));

        /// <summary>
        /// Creates a new instance of <see cref="App"/>.
        /// </summary>
        public App()
        {
            // Prepare dependency injection for the MainWindow and its MainViewModel.
            var listCreator = new ListCreator();
            var trieManager = new TrieManager();

            var mainViewModel = new MainViewModel(listCreator, trieManager);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();

            Logger.Debug("ParallelSearch started.");
        }

        private void AddLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4netConfig.xml"));
            Logger.Debug("Starting ParallelSearch...");
        }
    }
}