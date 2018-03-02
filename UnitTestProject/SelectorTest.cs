using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskClasses;

namespace UnitTestProject
{
    [TestClass]
    public class SelectorTest
    {
        [TestMethod]
        public void Selector_Test()
        {
            var actual = Selector.Select(new int[] { 1, 1, 2, 1, 1, 0, 1 }, 2);
            var expected = new int[][] { new int[] { 0, 2 }, new int[] { 1, 1 }, new int[] { 1, 1 }};

            Assert.AreEqual(expected.Length, actual.Length, "Size of the result does not match");

            for (int i = 0; i < expected.Length; i++ )
            {
                CollectionAssert.AreEquivalent(expected[i], actual[i]);
            }
        }
    }
}
