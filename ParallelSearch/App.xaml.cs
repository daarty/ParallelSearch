using System.Windows;
using ParallelSearch.Utils;

namespace ParallelSearch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Creates a new instance of <see cref="App"/>.
        /// </summary>
        public App()
        {
            // Prepare dependency injection for the MainWindow and its MainViewModel.
            var listCreator = new ListCreator();

            var mainViewModel = new MainViewModel(listCreator);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}