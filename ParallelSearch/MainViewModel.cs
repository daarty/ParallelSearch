namespace ParallelSearch
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using log4net;
    using ParallelSearch.Mvvm;
    using ParallelSearchLibrary.Result;
    using ParallelSearchLibrary.Timer;
    using ParallelSearchLibrary.Trie;
    using ParallelSearchLibrary.Words;

    /// <summary>
    /// ViewModel class for the MainWindow.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainViewModel));

        private bool doRefreshResultsCollection;
        private bool doRefreshWordCollection;
        private bool isTrieReady;
        private int numberOfCharacters = 4;
        private string searchString = string.Empty;
        private List<PreciseTimeSpan> searchTimes = new List<PreciseTimeSpan>();
        private List<PreciseTimeSpan> trieCreationTimes = new List<PreciseTimeSpan>();
        private List<PreciseTimeSpan> wordListCreationTimes = new List<PreciseTimeSpan>();

        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="wordCreator">Injected instance of the <see cref="IWordCreator"/>.</param>
        /// <param name="trieManager">Injected instance of the <see cref="ITrieManager"/>.</param>
        public MainViewModel(IWordCreator wordCreator, ITrieManager trieManager)
        {
            this.ListCreator = wordCreator;
            this.TrieManager = trieManager;
            this.CreateListCommand = new DelegateCommand(this.CreateListCallback);
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
        /// Gets or sets a value indicating whether the word list creation should be parallelized.
        /// </summary>
        public bool DoParallelize { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether result collection should be refreshed.
        /// </summary>
        public bool DoRefreshResultsCollection
        {
            get => this.doRefreshResultsCollection;
            set
            {
                if (this.doRefreshResultsCollection != value)
                {
                    this.doRefreshResultsCollection = value;
                    this.OnPropertyChanged(nameof(this.ResultsCollection));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether word list collection should be refreshed.
        /// </summary>
        public bool DoRefreshWordCollection
        {
            get => this.doRefreshWordCollection;
            set
            {
                if (this.doRefreshWordCollection != value)
                {
                    this.doRefreshWordCollection = value;
                    this.OnPropertyChanged(nameof(this.WordCollection));
                }
            }
        }

        /// <summary>
        /// Gets the average duration of a trie creation.
        /// </summary>
        public string DurationAverageCreation => PreciseTimeSpan.Average(this.trieCreationTimes).ToString();

        /// <summary>
        /// Gets the average duration of a search.
        /// </summary>
        public string DurationAverageSearch => PreciseTimeSpan.Average(this.searchTimes).ToString();

        /// <summary>
        /// Gets the average duration of a search.
        /// </summary>
        public string DurationAverageWordList => PreciseTimeSpan.Average(this.wordListCreationTimes).ToString();

        /// <summary>
        /// Gets the duration of the last trie creation.
        /// </summary>
        public string DurationLastCreation => this.trieCreationTimes.LastOrDefault()?.ToString();

        /// <summary>
        /// Gets the duration of the last search.
        /// </summary>
        public string DurationLastSearch => this.searchTimes.LastOrDefault()?.ToString();

        /// <summary>
        /// Gets the duration of the last trie creation.
        /// </summary>
        public string DurationLastWordList => this.wordListCreationTimes.LastOrDefault()?.ToString();

        /// <summary>
        /// Gets a value indicating whether the word list contains any words.
        /// </summary>
        public bool IsTrieReady
        {
            get => this.isTrieReady;
            set
            {
                if (this.isTrieReady != value)
                {
                    this.isTrieReady = value;
                    this.OnPropertyChanged(nameof(this.IsTrieReady));
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of characters in the words of the wordlist.
        /// </summary>
        public string NumberOfCharacters
        {
            get => this.numberOfCharacters.ToString();
            set
            {
                int validValue = 0;
                if (string.IsNullOrWhiteSpace(value) ||
                    int.TryParse(value, out validValue))
                {
                    this.numberOfCharacters = validValue;
                    this.OnPropertyChanged(nameof(this.NumberOfCharacters));

                    this.wordListCreationTimes.Clear();
                    this.trieCreationTimes.Clear();
                    this.searchTimes.Clear();
                    this.RefreshStatistics();
                }
            }
        }

        /// <summary>
        /// Gets the number of the trie creation processes.
        /// </summary>
        public string NumberOfCreations => this.trieCreationTimes.Count.ToString();

        /// <summary>
        /// Gets the number of the word search results.
        /// </summary>
        public string NumberOfResults => this.ResultsList?.Any() ?? false ? this.ResultsList.Count.ToString() : "-";

        /// <summary>
        /// Gets the number of the search processes.
        /// </summary>
        public string NumberOfSearches => this.searchTimes.Count.ToString();

        /// <summary>
        /// Gets the number of created words .
        /// </summary>
        public string NumberOfWords => this.WordList?.Any() ?? false ? this.WordList.Count.ToString() : "-";

        /// <summary>
        /// Gets the current word list.
        /// </summary>
        public ObservableCollection<string> ResultsCollection { get; private set; } = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the current search string.
        /// </summary>
        public string SearchString
        {
            get => this.searchString;
            set
            {
                if (value == null)
                {
                    return;
                }

                if (!string.Equals(this.searchString, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.searchString = value.ToUpper();
                    this.OnPropertyChanged(nameof(this.SearchString));

                    this.StartSearch();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Trie algorithm to use.
        /// </summary>
        public TrieAlgorithm TrieAlgorithm
        {
            get => this.TrieManager.TrieAlgorithm;
            set
            {
                if (this.TrieManager.TrieAlgorithm != value)
                {
                    this.TrieManager.TrieAlgorithm = value;
                    this.OnPropertyChanged(nameof(this.TrieAlgorithm));

                    this.trieCreationTimes.Clear();
                    this.searchTimes.Clear();
                    this.RefreshStatistics();
                    this.CreateTrie();
                }
            }
        }

        /// <summary>
        /// Gets the list of available Trie algorithms.
        /// </summary>
        public List<TrieAlgorithm> TrieAlgorithmList => Enum.GetValues(typeof(TrieAlgorithm)).Cast<TrieAlgorithm>().ToList();

        /// <summary>
        /// Gets the current word list.
        /// </summary>
        public ObservableCollection<string> WordCollection { get; private set; } = new ObservableCollection<string>();

        private IWordCreator ListCreator { get; }
        private List<string> ResultsList { get; set; } = new List<string>();
        private ITrieManager TrieManager { get; }
        private List<string> WordList { get; set; } = new List<string>();

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async void CreateListCallback()
        {
            this.WordCollection.Clear();
            this.WordList.Clear();
            this.ResultsCollection.Clear();
            this.ResultsList.Clear();
            this.OnPropertyChanged(nameof(this.WordCollection));
            this.IsTrieReady = false;
            this.OnPropertyChanged(nameof(this.ResultsCollection));
            this.OnPropertyChanged(nameof(this.NumberOfWords));
            this.OnPropertyChanged(nameof(this.NumberOfResults));

            WordListResult result = null;
            await Task.Run(() =>
            {
                if (this.DoParallelize)
                {
                    result = this.ListCreator.CreateWordListParallel(this.numberOfCharacters);
                }
                else
                {
                    result = this.ListCreator.CreateWordList(this.numberOfCharacters);
                }
            });

            if (result == null)
            {
                Logger.Warn("WordList creation result is null.");
                return;
            }

            this.WordList = result.WordList;
            this.OnPropertyChanged(nameof(this.NumberOfWords));

            if (this.DoRefreshWordCollection)
            {
                result.WordList.ForEach(x => this.WordCollection.Add(x));
                this.OnPropertyChanged(nameof(this.WordCollection));
            }
            this.IsTrieReady = true;

            this.wordListCreationTimes.Add(result.CreationTime);
            this.RefreshStatistics();

            this.CreateTrie();
        }

        private async void CreateTrie()
        {
            this.ResultsCollection.Clear();
            this.ResultsList.Clear();
            this.IsTrieReady = false;
            this.OnPropertyChanged(nameof(this.ResultsCollection));
            this.OnPropertyChanged(nameof(this.NumberOfResults));

            if (!this.WordList.Any())
            {
                return;
            }

            PreciseTimeSpan trieCreationDuration = null;
            await Task.Run(() => trieCreationDuration = this.TrieManager.CreateTrie(this.WordList.ToList()));

            if (trieCreationDuration == null)
            {
                Logger.Warn("Trie creation result is null.");
                return;
            }

            this.IsTrieReady = true;

            this.trieCreationTimes.Add(trieCreationDuration);
            this.RefreshStatistics();
        }

        private void RefreshStatistics()
        {
            this.OnPropertyChanged(nameof(this.DurationLastWordList));
            this.OnPropertyChanged(nameof(this.DurationAverageWordList));
            this.OnPropertyChanged(nameof(this.DurationLastSearch));
            this.OnPropertyChanged(nameof(this.DurationAverageSearch));
            this.OnPropertyChanged(nameof(this.NumberOfSearches));
            this.OnPropertyChanged(nameof(this.DurationLastCreation));
            this.OnPropertyChanged(nameof(this.DurationAverageCreation));
            this.OnPropertyChanged(nameof(this.NumberOfCreations));
            this.OnPropertyChanged(nameof(this.NumberOfWords));
            this.OnPropertyChanged(nameof(this.NumberOfResults));
        }

        private async void StartSearch()
        {
            this.ResultsCollection.Clear();
            this.ResultsList.Clear();
            this.OnPropertyChanged(nameof(this.NumberOfResults));
            this.OnPropertyChanged(nameof(this.ResultsCollection));

            SearchResult result =
                await Task.Run(() => result = this.TrieManager.Search(this.SearchString, this.WordList));

            this.ResultsList = result.ResultList;
            this.OnPropertyChanged(nameof(this.NumberOfResults));

            if (this.DoRefreshResultsCollection)
            {
                this.ResultsList.ForEach(x => this.ResultsCollection.Add(x));
                this.OnPropertyChanged(nameof(this.ResultsCollection));
            }

            this.searchTimes.Add(result.SearchTime);
            this.RefreshStatistics();
        }
    }
}