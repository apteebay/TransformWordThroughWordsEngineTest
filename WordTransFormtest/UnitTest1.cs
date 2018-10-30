using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransformWordInterface;

namespace WordTransFormtest
{
    /// <summary>
    /// Test the Mock Transformer. This should always work as 
    /// the correct result is hard coded into it
    /// </summary>
    [TestClass]
    public class UnitTestMock
    {
        [TestMethod]
        public void TestMethod1()
        {
            IWordTransformer wordTransformer = new WordTransformMock.WordTransformer();
            Assert.AreEqual(wordTransformer.Transforms.Count(), 3);
            Assert.IsTrue(wordTransformer.IsValid);
            Assert.AreEqual(wordTransformer.Transforms.Count(), 3);
            Assert.AreEqual(wordTransformer.Transforms[0], "Spin");
            Assert.AreEqual(wordTransformer.Transforms[1], "Spit");
            Assert.AreEqual(wordTransformer.Transforms[2], "Spot");
        }
    }

    /// <summary>
    /// Test the default Text File dictionary with full dictionary and char scan.
    /// </summary>
    [TestClass]
    public class UnitTestTxtDict
    {
        [TestMethod]
        public void TestMethod1()
        {
            IWordTransformer wordTransformer = new WordTransformTxtDict.WordTransformer(@"C:\Users\andrew.NEWLAND\Documents\Visual Studio 2017\Projects\TransformWordThroughWordsEngineTest\TransformWordThroughWordsEngineTest\Data\words-english1.txt");
            wordTransformer.StartWord = "Spin";
            wordTransformer.EndWord = "Spot";
            Assert.IsTrue(wordTransformer.IsValid);
            Assert.AreEqual(wordTransformer.Transforms.Count(), 3);
            Assert.AreEqual(wordTransformer.Transforms[0], "Spin");
            Assert.AreEqual(wordTransformer.Transforms[1], "Spit");
            Assert.AreEqual(wordTransformer.Transforms[2], "Spot");

        }
    }

    [TestClass]
    public class UnitTestFastenshtein
    {
        /// <summary>
        /// Test Text File Dictionary with Levenshtein scan using
        /// WordTransformFastenshtein Derived Class from WordTransformTxtDict
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            IWordTransformer wordTransformer = new WordTransformFastenshtein.WordTransformer(@"C:\Users\andrew.NEWLAND\Documents\Visual Studio 2017\Projects\TransformWordThroughWordsEngineTest\TransformWordThroughWordsEngineTest\Data\words-english1.txt");
            wordTransformer.StartWord = "Spin";
            wordTransformer.EndWord = "Spot";
            Assert.IsTrue(wordTransformer.IsValid);
            Assert.AreEqual(wordTransformer.Transforms.Count(), 3);
            Assert.AreEqual(wordTransformer.Transforms[0], "Spin");
            Assert.AreEqual(wordTransformer.Transforms[1], "Spit");
            Assert.AreEqual(wordTransformer.Transforms[2], "Spot");

        }

        /// <summary>
        /// Test Text File Dictionary with  Levenshtein scan using
        /// WordTransformTxtDict and injecting the WordTransformFastenshtein Engine
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            IWordTransformer wordTransformer = new WordTransformTxtDict.WordTransformer(@"C:\Users\andrew.NEWLAND\Documents\Visual Studio 2017\Projects\TransformWordThroughWordsEngineTest\TransformWordThroughWordsEngineTest\Data\words-english1.txt");
            wordTransformer.Engine = new WordTransformFastenshtein.Engine();
            wordTransformer.StartWord = "Spin";
            wordTransformer.EndWord = "Spot";
            Assert.IsTrue(wordTransformer.IsValid);
            Assert.AreEqual(wordTransformer.Transforms.Count(), 3);
            Assert.AreEqual(wordTransformer.Transforms[0], "Spin");
            Assert.AreEqual(wordTransformer.Transforms[1], "Spit");
            Assert.AreEqual(wordTransformer.Transforms[2], "Spot");

        }
    }
}
