﻿namespace ParallelSearchTestTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using log4net;
    using ParallelSearchLibrary.List;
    using ParallelSearchLibrary.Timer;
    using ParallelSearchLibrary.Trie;

    public class MainMenu
    {
        private const int DangerousNumberOfCharacters = 5;
        private const int DefaultInvalidNumberOfCharacters = 0;
        private const int DefaultInvalidNumberOfRuns = 0;
        private const int DefaultNumberOfCharacters = 4;
        private const int DefaultNumberOfRuns = 1000;
        private const int NumberOfCharactersInConsoleLine = 120;
        private static readonly char[] charactersArray = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static readonly string[] NegativeInput = { "no", "n" };
        private static readonly string[] PositiveInput = { "yes", "y" };

        public MainMenu(IListCreator listCreator, ITrieManager trieManager)
        {
            this.ListCreator = listCreator;
            this.TrieManager = trieManager;
        }

        private IListCreator ListCreator { get; }

        private ITrieManager TrieManager { get; }

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

        private string GetRandomSearchWord(int maxNumberOfCharacters)
        {
            var random = new Random();
            var numberOfCharacters = random.Next(maxNumberOfCharacters) + 1;

            var charArray = new char[numberOfCharacters];
            for (int i = 0; i < numberOfCharacters; i++)
            {
                charArray[i] = charactersArray[random.Next(charactersArray.Length)];
            }

            return new string(charArray);
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

            var wordList = this.ListCreator.CreateWordList(numberOfCharacters);

            var creationTimes = new List<PreciseTimeSpan>();
            var searchTimes = new List<PreciseTimeSpan>();

            var alreadyDrawnProgressDots = 0;

            for (int i = 0; i < numberOfRuns; i++)
            {
                var numberOfProgressDotsToDraw = Math.Round(((double)NumberOfCharactersInConsoleLine / (double)numberOfRuns) * (double)i);
                for (int dots = alreadyDrawnProgressDots; dots < numberOfProgressDotsToDraw; dots++)
                {
                    alreadyDrawnProgressDots++;
                    Console.Write(".");
                }

                var creationResult = this.TrieManager.CreateTrie(trieAlgorithm, wordList);
                creationTimes.Add(creationResult.CreationTime);

                var randomSearchWord = GetRandomSearchWord(numberOfCharacters);

                var searchResult = this.TrieManager.Search(creationResult.Trie, randomSearchWord);
                searchTimes.Add(searchResult.SearchTime);
            }

            Console.WriteLine(string.Empty);
            Logger.Info(string.Empty);
            Logger.Info($"Finished benchmark of '{trieAlgorithm}' Trie with '{numberOfCharacters}' characters");
            Logger.Info($" Average Trie Creation time: {PreciseTimeSpan.Average(creationTimes),10}");
            Logger.Info($" Average Trie Search time:   {PreciseTimeSpan.Average(searchTimes),10}");
        }
    }
}