using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ParallelSearch.Mvvm;
using ParallelSearch.Utils;

namespace ParallelSearch
{
    /// <summary>
    /// ViewModel class for the MainWindow.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private string searchString = string.Empty;

        private Regex validWordRegex = new Regex("^[a-zA-Z]{0,4}$");

        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="listCreator">Injected instance of the ListCreator.</param>
        public MainViewModel(IListCreator listCreator)
        {
            this.listCreator = listCreator;
            CreateListCommand = new DelegateCommand(CreateListCallback);
        }

        /// <summary>
        /// Event handler for the 'property changed' event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the command that loads the file with the data points.
        /// </summary>
        public ICommand CreateListCommand { get; }

        /// <summary>
        /// Gets the average duration of a search.
        /// </summary>
        public string DurationAverage { get; private set; }

        /// <summary>
        /// Gets the duration of the last search.
        /// </summary>
        public string DurationLastSearch { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the word list contains any words.
        /// </summary>
        public bool IsWordListFilled => WordList.Any();

        /// <summary>
        /// Gets the number of the search processes.
        /// </summary>
        public string NumberOfSearches { get; private set; }

        /// <summary>
        /// Gets the current word list.
        /// </summary>
        public ObservableCollection<string> Results { get; private set; } = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the current search string.
        /// </summary>
        public string SearchString
        {
            get => searchString;
            set
            {
                if (value == null)
                {
                    return;
                }

                if (validWordRegex.IsMatch(value) &&
                    !string.Equals(searchString, value, StringComparison.OrdinalIgnoreCase))
                {
                    searchString = value.ToUpper();
                    OnPropertyChanged(nameof(SearchString));

                    StartSearch();
                }
            }
        }

        /// <summary>
        /// Gets the current word list.
        /// </summary>
        public ObservableCollection<string> WordList { get; private set; } = new ObservableCollection<string>();

        private IListCreator listCreator { get; }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void CreateListCallback()
        {
            WordList.Clear();
            listCreator.CreateWordList().ForEach(x => WordList.Add(x));
            OnPropertyChanged(nameof(WordList));
            OnPropertyChanged(nameof(IsWordListFilled));
        }

        private void StartSearch()
        {
            // TODO initiate search

            Results.Clear();
            WordList.ToList().ForEach(x => Results.Add(x));
            OnPropertyChanged(nameof(Results));
        }
    }
}