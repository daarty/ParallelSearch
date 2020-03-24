namespace ParallelSearchLibrary.List
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using log4net;
    using ParallelSearchLibrary.Result;
    using Timer;

    /// <summary>
    /// Implementation of the <see cref="IListCreator"/> interface.
    /// </summary>
    public class ListCreator : IListCreator
    {
        private static readonly char[] CharacterArray = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ListCreator));

        /// </inheritdoc>
        public WordListResult CreateWordList(int wordLength)
        {
            var wordList = new List<string>();

            if (wordLength == 0)
            {
                return new WordListResult { WordList = wordList, CreationTime = new PreciseTimeSpan() };
            }

            int numberOfWords = Convert.ToInt32(Math.Pow(CharacterArray.Length, wordLength));
            var wordsArray = new string[numberOfWords];

            Logger.Debug($"Creating Word List with wordLength '{wordLength}'...");
            var timer = new PreciseTimer();
            timer.Start();

            this.CreateWordArrayRecursive(wordsArray, 0, new char[wordLength], 0);

            var timeSpanCreation = timer.Stop();
            Logger.Debug($"Successfully created Word List with '{wordList.Count}' elements in '{timeSpanCreation}'.");

            var random = new Random();
            Logger.Debug($"Permutating Word List with '{wordList.Count}' elements...");
            timer.Start();

            wordList = wordsArray.OrderBy(x => random.Next()).ToList();

            var timeSpanShuffle = timer.Stop();
            Logger.Debug($"Successfully permutated Word List with '{wordList.Count}' elements in '{timeSpanShuffle}'.");

            return new WordListResult { WordList = wordList, CreationTime = new PreciseTimeSpan(timeSpanCreation.TotalMicroseconds + timeSpanShuffle.TotalMicroseconds) };
        }

        private int CreateWordArrayRecursive(string[] wordArray, int previousWordIndex, char[] currentWord, int currentCharIndex)
        {
            int currentWordIndex = previousWordIndex;
            foreach (var character in CharacterArray)
            {
                currentWord[currentCharIndex] = character;
                currentCharIndex++;

                if (currentWord.Length == currentCharIndex)
                {
                    wordArray[currentWordIndex] = new string(currentWord);
                    currentWordIndex++;
                    currentCharIndex--;
                }
                else
                {
                    currentWordIndex = CreateWordArrayRecursive(wordArray, currentWordIndex, currentWord, currentCharIndex);
                    currentCharIndex--;
                }
            }

            return currentWordIndex;
        }

        private void CreateWordBagRecursive(ConcurrentBag<string> wordBag, char[] currentWord, int currentCharIndex)
        {
            if (currentWord.Length == currentCharIndex)
            {
                wordBag.Add(new string(currentWord));
                return;
            }

            foreach (var character in CharacterArray)
            {
                currentWord[currentCharIndex] = character;
                currentCharIndex++;

                if (currentWord.Length == currentCharIndex)
                {
                    wordBag.Add(new string(currentWord));
                    currentCharIndex--;
                }
                else
                {
                    CreateWordBagRecursive(wordBag, currentWord, currentCharIndex);
                    currentCharIndex--;
                }
            }

            return;
        }

        /// </inheritdoc>
        public WordListResult CreateWordListParallel(int wordLength)
        {
            var wordList = new List<string>();

            if (wordLength == 0)
            {
                return new WordListResult { WordList = wordList, CreationTime = new PreciseTimeSpan() };
            }

            int numberOfWords = Convert.ToInt32(Math.Pow(CharacterArray.Length, wordLength));
            var wordBag = new ConcurrentBag<string>();

            Logger.Debug($"Creating Word List with wordLength '{wordLength}'...");
            var timer = new PreciseTimer();
            timer.Start();

            // Not optimal parallelism but at least that's 26 threads.
            Parallel.ForEach(
                CharacterArray,
                character =>
                {
                    var newWord = new char[wordLength];
                    newWord[0] = character;
                    CreateWordBagRecursive(wordBag, newWord, 1);
                });

            var timeSpanCreation = timer.Stop();
            Logger.Debug($"Successfully created Word List with '{wordList.Count}' elements in '{timeSpanCreation}'.");

            var random = new Random();
            Logger.Debug($"Permutating Word List with '{wordList.Count}' elements...");
            timer.Start();

            wordList = wordBag.OrderBy(x => random.Next()).ToList();

            var timeSpanShuffle = timer.Stop();
            Logger.Debug($"Successfully permutated Word List with '{wordList.Count}' elements in '{timeSpanShuffle}'.");

            return new WordListResult { WordList = wordList, CreationTime = new PreciseTimeSpan(timeSpanCreation.TotalMicroseconds + timeSpanShuffle.TotalMicroseconds) };
        }

        // TODO put this here and rename the class to WordManager
        private string GetRandomWord(int numberOfCharacters)
        {
            var random = new Random();

            var charArray = new char[numberOfCharacters];
            for (int i = 0; i < numberOfCharacters; i++)
            {
                charArray[i] = CharacterArray[random.Next(CharacterArray.Length)];
            }

            return new string(charArray);
        }
    }
}