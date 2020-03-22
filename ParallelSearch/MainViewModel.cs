namespace ParallelSearch
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Gma.DataStructures.StringSearch;
    using ParallelSearch.Mvvm;
    using ParallelSearch.Utils;

    /// <summary>
    /// ViewModel class for the MainWindow.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private int numberOfCharacters = 4;
        private string searchString = string.Empty;

        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="listCreator">Injected instance of the <see cref="IListCreator"/>.</param>
        /// <param name="trieManager">Injected instance of the <see cref="ITrieManager"/>.</param>
        public MainViewModel(IListCreator listCreator, ITrieManager trieManager)
        {
            this.ListCreator = listCreator;
            this.TrieManager = trieManager;
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
        public bool IsTrieReady => WordList.Any() && this.Trie != null;

        /// <summary>
        /// Gets or sets the number of characters in the words of the wordlist.
        /// </summary>
        public string NumberOfCharacters
        {
            get => numberOfCharacters.ToString();
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.numberOfCharacters = 0;
                    OnPropertyChanged(nameof(NumberOfCharacters));
                }
                else if (int.TryParse(value, out int validValue))
                {
                    this.numberOfCharacters = validValue;
                    OnPropertyChanged(nameof(NumberOfCharacters));
                }
            }
        }

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

                if (!string.Equals(searchString, value, StringComparison.OrdinalIgnoreCase))
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

        private IListCreator ListCreator { get; }
        private ITrie<string> Trie { get; set; }
        private ITrieManager TrieManager { get; }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async void CreateListCallback()
        {
            this.WordList.Clear();
            this.Results.Clear();
            this.Trie = null;
            OnPropertyChanged(nameof(WordList));
            OnPropertyChanged(nameof(IsTrieReady));
            OnPropertyChanged(nameof(Results));

            var list = new List<string>();
            await Task.Run(() => list = this.ListCreator.CreateWordList(this.numberOfCharacters));

            list.ForEach(x => this.WordList.Add(x));
            OnPropertyChanged(nameof(WordList));
            OnPropertyChanged(nameof(IsTrieReady));

            await Task.Run(() => this.Trie = this.TrieManager.CreateBasicTrie(this.WordList.ToList()));

            OnPropertyChanged(nameof(IsTrieReady));
        }

        private async void StartSearch()
        {
            Results.Clear();
            OnPropertyChanged(nameof(Results));

            var results = new List<string>();
            await Task.Run(() => results = TrieManager.Search(this.Trie, this.SearchString));

            results.ToList().ForEach(x => Results.Add(x));
            OnPropertyChanged(nameof(Results));
        }
    }
}