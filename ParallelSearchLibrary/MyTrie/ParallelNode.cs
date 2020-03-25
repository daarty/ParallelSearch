namespace ParallelSearchLibrary.MyTrie
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementation of a node in the MyParallelTrie implementation.
    /// </summary>
    public class ParallelNode
    {
        /// <summary>
        /// Creates a new instance of <see cref="ParallelNode"/>.
        /// </summary>
        /// <remarks>This basic constructor is needed for the root element.</remarks>
        public ParallelNode()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ParallelNode"/>.
        /// </summary>
        /// <param name="word">Word to be added to the node.</param>
        public ParallelNode(char[] word)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));

            if (!word.Any())
            {
                return;
            }

            this.Children.GetOrAdd(word[0], new ParallelNode(word.Skip(1).ToArray()));
        }

        /// <summary>
        /// Gets the concurrent dictionary containing the children nodes.
        /// </summary>
        public ConcurrentDictionary<char, ParallelNode> Children { get; } = new ConcurrentDictionary<char, ParallelNode>();

        /// <summary>
        /// Adds the given word to this node and its children.
        /// </summary>
        /// <param name="word">Word to be added to the node.</param>
        /// <exception cref="ArgumentException">If the word is null or empty.</exception>
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

        /// <summary>
        /// Searches for the given word whithin this node and its sub-tree.
        /// </summary>
        /// <param name="word">Word to be searched for.</param>
        /// <param name="resultWord">
        /// The currently built result sub-word that will be added to the results.
        /// </param>
        /// <param name="result">Concurrent set of all results.</param>
        /// <exception cref="ArgumentNullException">If any argument is null.</exception>
        public void Search(char[] word, string resultWord, ConcurrentBag<string> result)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));
            if (resultWord == null) throw new ArgumentNullException(nameof(resultWord));
            if (result == null) throw new ArgumentNullException(nameof(result));

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
            else
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