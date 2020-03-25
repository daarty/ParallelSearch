namespace ParallelSearchTestTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using log4net;
    using ParallelSearchLibrary.Timer;
    using ParallelSearchLibrary.Trie;
    using ParallelSearchLibrary.Words;

    public class MainMenu
    {
        private const int DangerousNumberOfCharacters = 5;
        private const int DefaultInvalidNumberOfCharacters = 0;
        private const int DefaultInvalidNumberOfRuns = 0;
        private const int DefaultNumberOfCharacters = 4;
        private const int DefaultNumberOfRuns = 10;
        private const double NumberOfCharactersInConsoleLine = 120;
        private static readonly char[] CharactersArray = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static readonly string[] NegativeInput = { "no", "n" };
        private static readonly string[] PositiveInput = { "yes", "y" };

        public MainMenu(IWordCreator wordCreator, ITrieManager trieManager)
        {
            this.WordCreator = wordCreator;
            this.TrieManager = trieManager;
        }

        private ITrieManager TrieManager { get; }
        private IWordCreator WordCreator { get; }

        public void RunAutomaticBenchmark()
        {
            Logger.Info(string.Empty);
            Logger.Info($"Starting the automatic benchmark that tests all algorithms with the default values ...");
            RunBenchmark(DefaultNumberOfCharacters, TrieAlgorithm.Basic, DefaultNumberOfRuns);
            RunBenchmark(DefaultNumberOfCharacters, TrieAlgorithm.Concurrent, DefaultNumberOfRuns);
            RunBenchmark(DefaultNumberOfCharacters, TrieAlgorithm.Patricia, DefaultNumberOfRuns);
            RunBenchmark(DefaultNumberOfCharacters, TrieAlgorithm.Ukkonen, DefaultNumberOfRuns);
        }

        public void StartManualMode()
        {
            var numberOfCharacters = this.GetNumberOfCharacters();
            var trieAlgorithm = this.GetTrieAlgorithm();
            var numberOfRuns = this.GetNumberOfRuns();
            this.RunBenchmark(numberOfCharacters, trieAlgorithm, numberOfRuns);
        }

        private int GetNumberOfCharacters()
        {
            bool isValidInput;
            var numberOfCharacters = DefaultInvalidNumberOfCharacters;
            while (numberOfCharacters <= DefaultInvalidNumberOfCharacters)
            {
                Logger.Info(string.Empty);
                Logger.Info($"How many letters should the trie have? (default: {DefaultNumberOfCharacters})");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    numberOfCharacters = DefaultNumberOfCharacters;
                }
                else if (int.TryParse(input, out numberOfCharacters))
                {
                    if (numberOfCharacters >= DangerousNumberOfCharacters)
                    {
                        isValidInput = false;
                        while (!isValidInput)
                        {
                            Logger.Info(string.Empty);
                            Logger.Info($"This is a high value and the computation might take very long. Are you sure? [y, n] (default: n)");
                            input = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(input) || NegativeInput.Contains(input))
                            {
                                isValidInput = true;
                                numberOfCharacters = DefaultInvalidNumberOfCharacters;
                            }
                            else if (PositiveInput.Contains(input))
                            {
                                isValidInput = true;
                            }
                            else
                            {
                                Logger.Warn($"'{input}' is an invalid input.");
                            }
                        }
                    }
                }
                else
                {
                    Logger.Warn($"'{input}' is an invalid input.");
                }
            }

            return numberOfCharacters;
        }

        private int GetNumberOfRuns()
        {
            var numberOfRuns = DefaultInvalidNumberOfRuns;
            while (numberOfRuns <= DefaultInvalidNumberOfRuns)
            {
                Logger.Info(string.Empty);
                Logger.Info($"How many searches should be run? (default: {DefaultNumberOfRuns})");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    numberOfRuns = DefaultNumberOfRuns;
                }
                else if (!int.TryParse(input, out numberOfRuns))
                {
                    Logger.Warn($"'{input}' is not a valid number.");
                }
            }

            return numberOfRuns;
        }

        private TrieAlgorithm GetTrieAlgorithm()
        {
            var selectedAlgorithm = TrieAlgorithm.Basic;

            var isValidInput = false;
            while (!isValidInput)
            {
                Logger.Info(string.Empty);
                Logger.Info($"What algorithm should be used? (default: {(int)selectedAlgorithm} - {selectedAlgorithm})");
                foreach (var algorithm in Enum.GetValues(typeof(TrieAlgorithm)))
                {
                    Logger.Info($" -{(int)algorithm,2} {algorithm,12}");
                }

                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    isValidInput = true;
                }
                else if (int.TryParse(input, out int algorithmSelection))
                {
                    foreach (var algorithm in Enum.GetValues(typeof(TrieAlgorithm)))
                    {
                        if (algorithmSelection == (int)algorithm)
                        {
                            selectedAlgorithm = (TrieAlgorithm)algorithmSelection;
                            isValidInput = true;
                        }
                    }

                    if (!isValidInput)
                    {
                        Logger.Warn($"'{input}' is not a valid algorithm choice.");
                    }
                }
                else
                {
                    Logger.Warn($"'{input}' is not a valid algorithm choice.");
                }
            }

            return selectedAlgorithm;
        }

        private void RunBenchmark(int numberOfCharacters, TrieAlgorithm trieAlgorithm, int numberOfRuns)
        {
            Logger.Info(string.Empty);
            Logger.Info("Start running benchmark with:");
            Logger.Info($"- a word list of words with '{numberOfCharacters}' characters");
            Logger.Info($"- a trie with the '{trieAlgorithm}' algorithm");
            Logger.Info($"- the average result of '{numberOfRuns}' executions of the test");
            Logger.Info(string.Empty);

            var wordListCreationResult = this.WordCreator.CreateWordList(numberOfCharacters);

            // TODO wordlist creation time?
            var creationTimes = new List<PreciseTimeSpan>();
            var searchTimes = new List<PreciseTimeSpan>();

            var alreadyDrawnProgressDots = 0;
            this.TrieManager.TrieAlgorithm = trieAlgorithm;

            for (int i = 0; i < numberOfRuns; i++)
            {
                var creationResult = this.TrieManager.CreateTrie(wordListCreationResult.WordList);
                creationTimes.Add(creationResult);

                var randomSearchWord = this.WordCreator.GetRandomWord(numberOfCharacters);

                var searchResult = this.TrieManager.Search(randomSearchWord, wordListCreationResult.WordList);
                searchTimes.Add(searchResult.SearchTime);

                // Draw progress line
                var numberOfProgressDotsToDraw = Math.Round(NumberOfCharactersInConsoleLine / numberOfRuns * (i + 1));
                for (int dots = alreadyDrawnProgressDots; dots < numberOfProgressDotsToDraw; dots++)
                {
                    Console.Write(".");
                    alreadyDrawnProgressDots++;
                }
            }

            Console.WriteLine(string.Empty);
            Logger.Info(string.Empty);
            Logger.Info($"Finished benchmark of '{trieAlgorithm}' Trie with '{numberOfCharacters}' characters:");
            Logger.Info($" Average Trie Creation time: {PreciseTimeSpan.Average(creationTimes),10}");
            Logger.Info($" Average Trie Search time:   {PreciseTimeSpan.Average(searchTimes),10}");
            Logger.Info(string.Empty);
            Logger.Info("-----------------------------------------------------------------------------------------------------------------------");
        }
    }
}