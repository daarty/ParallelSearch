namespace ParallelSearchLibrary.MyTrie
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implementation of a base node in the MyTrie implementation.
    /// </summary>
    public class BaseNode
    {
        /// <summary>
        /// Gets the list containing the children nodes.
        /// </summary>
        public List<Node> Children { get; } = new List<Node>();

        /// <summary>
        /// Adds the given word to this node and its children.
        /// </summary>
        /// <param name="word">Word to be added to the node.</param>
        /// <exception cref="ArgumentException">If the word is null or empty.</exception>
        public void Add(char[] word)
        {
            this.CheckWord(word);

            var existingChild = Children.FirstOrDefault(child => child.Character == word[0]);
            if (existingChild != null)
            {
                existingChild.Add(word.Skip(1).ToArray());
            }
            else
            {
                this.Children.Add(new Node(word));
            }
        }

        /// <summary>
        /// Searches for the given word whithin this node and its sub-tree.
        /// </summary>
        /// <param name="word">Word to be searched for.</param>
        /// <param name="resultWord">
        /// The currently built result sub-word that will be added to the results.
        /// </param>
        /// <param name="result">List of all results.</param>
        /// <exception cref="ArgumentNullException">If any argument is null.</exception>
        public void Search(char[] word, string resultWord, List<string> result)
        {
            if (word == null) throw new ArgumentNullException(nameof(word));
            if (resultWord == null) throw new ArgumentNullException(nameof(resultWord));
            if (result == null) throw new ArgumentNullException(nameof(result));

            if (word.Any())
            {
                var existingChild = Children.FirstOrDefault(child => child.Character == word[0]);
                if (existingChild != null)
                {
                    existingChild.Search(word.Skip(1).ToArray(), resultWord + word[0], result);
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

        /// <summary>
        /// Checks whether the passed word is null or empty.
        /// </summary>
        /// <param name="word">Word to be checked.</param>
        /// <exception cref="ArgumentException">If the word is null or empty.</exception>
        protected void CheckWord(char[] word)
        {
            if (word == null || !word.Any()) throw new ArgumentException($"The '{nameof(word)}' char array is null or empty.");
        }

        /// <summary>
        /// Checks the sub nodes recursively, builds their containing words and adds them to the
        /// result list.
        /// </summary>
        /// <param name="resultWord">
        /// The currently built result sub-word that will be added to the results.
        /// </param>
        /// <param name="result">List of all results.</param>
        protected void GetAllWords(string resultWord, List<string> result)
        {
            if (!this.Children.Any())
            {
                result.Add(resultWord);
                return;
            }

            foreach (var child in this.Children)
            {
                child.GetAllWords(resultWord + child.Character, result);
            }
        }
    }
}