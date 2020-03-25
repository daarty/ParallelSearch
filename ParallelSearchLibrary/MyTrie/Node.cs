namespace ParallelSearchLibrary.MyTrie
{
    using System.Linq;

    /// <summary>
    /// Implementation of a non-root node.
    /// </summary>
    public class Node : BaseNode
    {
        /// <summary>
        /// Creates a new instance of <see cref="MyParallelTrie"/>.
        /// </summary>
        /// <param name="word">Word to be added to the node.</param>
        public Node(char[] word)
        {
            this.CheckWord(word);

            this.Character = word[0];
            if (word.Length == 1)
            {
                return;
            }

            this.Children.Add(new Node(word.Skip(1).ToArray()));
        }

        /// <summary>
        /// Gets the character that this node is representing.
        /// </summary>
        public char Character { get; }
    }
}