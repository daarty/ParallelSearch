namespace ParallelSearchLibrary.MyTrie
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;

    public class ParallelNode
    {
        public ParallelNode()
        {
        }

        public ParallelNode(char[] word)
        {
            if (word == null) throw new ArgumentNullException($"The '{nameof(word)}' is null.");

            if (!word.Any())
            {
                return;
            }

            if (!this.Children.TryAdd(word[0], new ParallelNode(word.Skip(1).ToArray())))
            {
                throw new NotImplementedException();
            }
        }

        public ConcurrentDictionary<char, ParallelNode> Children { get; } = new ConcurrentDictionary<char, ParallelNode>();

        public void Add(char[] word)
        {
            this.CheckWord(word);

            if (this.Children.ContainsKey(word[0]))
            {
                this.Children[word[0]].Add(word.Skip(1).ToArray());
            }
            else
            {
                this.Children.AddOrUpdate(
                    word[0],
                    new ParallelNode(word.Skip(1).ToArray()),
                    (character, oldChild) => UpdateChildNode(character, oldChild, word));
            }
        }

        public void Search(char[] word, string resultWord, ConcurrentBag<string> result)
        {
            if (word == null) throw new ArgumentNullException($"The '{nameof(word)}' is null.");
            if (resultWord == null) throw new ArgumentNullException($"The '{nameof(resultWord)}' is null.");

            if (word.Any())
            {
                if (this.Children.ContainsKey(word[0]))
                {
                    this.Children[word[0]].Search(word.Skip(1).ToArray(), resultWord + word[0], result);
                }
                else
                {
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(resultWord))
            {
                this.GetAllWords(resultWord, result);
            }
        }

        private void CheckWord(char[] word)
        {
            if (word == null || !word.Any()) throw new ArgumentException($"The '{nameof(word)}' char array is null or empty.");
        }

        private void GetAllWords(string resultWord, ConcurrentBag<string> result)
        {
            if (!this.Children.Any())
            {
                result.Add(resultWord);
                return;
            }

            Parallel.ForEach(this.Children, child => child.Value.GetAllWords(resultWord + child.Key, result));
        }

        private ParallelNode UpdateChildNode(char _, ParallelNode exisingChild, char[] word)
        {
            exisingChild.Add(word.Skip(1).ToArray());
            return exisingChild;
        }
    }
}