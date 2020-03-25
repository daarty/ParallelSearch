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

        public bool DoParallelize { get; set; } = true;

        public bool DoRefreshResultsCollection
        {
            get => doRefreshResultsCollection;
            set
            {
                if (doRefreshResultsCollection != value)
                {
                    doRefreshResultsCollection = value;
                    OnPropertyChanged(nameof(ResultsCollection));
                }
            }
        }

        public bool DoRefreshWordCollection
        {
            get => doRefreshWordCollection;
            set
            {
                if (doRefreshWordCollection != value)
                {
                    doRefreshWordCollection = value;
                    OnPropertyChanged(nameof(WordCollection));
                }
            }
        }

        /// <summary>
        /// Gets the average duration of a trie creation.
        /// </summary>
        public string DurationAverageCreation => PreciseTimeSpan.Average(trieCreationTimes).ToString();

        /// <summary>
        /// Gets the average duration of a search.
        /// </summary>
        public string DurationAverageSearch => PreciseTimeSpan.Average(searchTimes).ToString();

        /// <summary>
        /// Gets the average duration of a search.
        /// </summary>
        public string DurationAverageWordList => PreciseTimeSpan.Average(wordListCreationTimes).ToString();

        /// <summary>
        /// Gets the duration of the last trie creation.
        /// </summary>
        public string DurationLastCreation => trieCreationTimes.LastOrDefault()?.ToString();

        /// <summary>
        /// Gets the duration of the last search.
        /// </summary>
        public string DurationLastSearch => searchTimes.LastOrDefault()?.ToString();

        /// <summary>
        /// Gets the duration of the last trie creation.
        /// </summary>
        public string DurationLastWordList => wordListCreationTimes.LastOrDefault()?.ToString();

        /// <summary>
        /// Gets a value indicating whether the word list contains any words.
        /// </summary>
        public bool IsTrieReady
        {
            get => isTrieReady;
            set
            {
                if (isTrieReady != value)
                {
                    isTrieReady = value;
                    OnPropertyChanged(nameof(IsTrieReady));
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of characters in the words of the wordlist.
        /// </summary>
        public string NumberOfCharacters
        {
            get => numberOfCharacters.ToString();
            set
            {
                int validValue = 0;
                if (string.IsNullOrWhiteSpace(value) ||
                    int.TryParse(value, out validValue))
                {
                    this.numberOfCharacters = validValue;
                    OnPropertyChanged(nameof(NumberOfCharacters));

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

        public string NumberOfResults => ResultsList?.Any() ?? false ? ResultsList.Count.ToString() : "-";

        /// <summary>
        /// Gets the number of the search processes.
        /// </summary>
        public string NumberOfSearches => this.searchTimes.Count.ToString();

        public string NumberOfWords => WordList?.Any() ?? false ? WordList.Count.ToString() : "-";

        /// <summary>
        /// Gets the current word list.
        /// </summary>
        public ObservableCollection<string> ResultsCollection { get; private set; } = new ObservableCollection<string>();

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
                    this.OnPropertyChanged(nameof(TrieAlgorithm));

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
            OnPropertyChanged(nameof(WordCollection));
            IsTrieReady = false;
            OnPropertyChanged(nameof(ResultsCollection));
            OnPropertyChanged(nameof(NumberOfWords));
            OnPropertyChanged(nameof(NumberOfResults));

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
            OnPropertyChanged(nameof(NumberOfWords));

            if (this.DoRefreshWordCollection)
            {
                result.WordList.ForEach(x => this.WordCollection.Add(x));
                OnPropertyChanged(nameof(WordCollection));
            }
            IsTrieReady = true;

            this.wordListCreationTimes.Add(result.CreationTime);
            this.RefreshStatistics();

            this.CreateTrie();
        }

        private async void CreateTrie()
        {
            this.ResultsCollection.Clear();
            this.ResultsList.Clear();
            IsTrieReady = false;
            OnPropertyChanged(nameof(ResultsCollection));
            OnPropertyChanged(nameof(NumberOfResults));

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

            IsTrieReady = true;

            this.trieCreationTimes.Add(trieCreationDuration);
            this.RefreshStatistics();
        }

        private void RefreshStatistics()
        {
            OnPropertyChanged(nameof(DurationLastWordList));
            OnPropertyChanged(nameof(DurationAverageWordList));
            OnPropertyChanged(nameof(DurationLastSearch));
            OnPropertyChanged(nameof(DurationAverageSearch));
            OnPropertyChanged(nameof(NumberOfSearches));
            OnPropertyChanged(nameof(DurationLastCreation));
            OnPropertyChanged(nameof(DurationAverageCreation));
            OnPropertyChanged(nameof(NumberOfCreations));
            OnPropertyChanged(nameof(NumberOfWords));
            OnPropertyChanged(nameof(NumberOfResults));
        }

        private async void StartSearch()
        {
            ResultsCollection.Clear();
            ResultsList.Clear();
            OnPropertyChanged(nameof(NumberOfResults));
            OnPropertyChanged(nameof(ResultsCollection));

            SearchResult result =
                await Task.Run(() => result = this.TrieManager.Search(this.SearchString, this.WordList));

            this.ResultsList = result.ResultList;
            OnPropertyChanged(nameof(NumberOfResults));

            if (this.DoRefreshResultsCollection)
            {
                this.ResultsList.ForEach(x => ResultsCollection.Add(x));
                OnPropertyChanged(nameof(ResultsCollection));
            }

            this.searchTimes.Add(result.SearchTime);
            this.RefreshStatistics();
        }
    }
}