namespace ParallelSearchLibraryTests.MyTrie
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using lib = ParallelSearchLibrary.MyTrie;

    [TestClass]
    public class MyTrieTest
    {
        private List<string> sampleWordList = new List<string> { "hallo", "hi", "hey", "hallöchen", "holla", "grüß gott" };

        [TestMethod]
        public void ConstructorTest()
        {
            // Execute the test
            var trie = new lib.MyTrie(this.sampleWordList);

            // Verify the results
            Assert.IsNotNull(trie);
            Assert.IsNotNull(trie.RootNode);
            Assert.AreEqual(2, trie.RootNode.Children.Count);
        }

        [DataTestMethod]
        [DataRow("", 6)]
        [DataRow("h", 5)]
        [DataRow("ha", 2)]
        [DataRow("hal", 2)]
        [DataRow("hall", 2)]
        [DataRow("hallo", 1)]
        [DataRow("hallö", 1)]
        [DataRow("hallodri", 0)]
        [DataRow("g", 1)]
        [DataRow("a", 0)]
        public void SearchTest(string searchTerm, int numberOfResults)
        {
            // Setup the environment
            var trie = new lib.MyTrie(this.sampleWordList);

            // Execute the test
            var result = trie.Search(searchTerm);

            // Verify the results
            Assert.AreEqual(numberOfResults, result.Count);
        }
    }
}