using NUnit.Framework;
using NUnitTest;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PassingTest()
        {
            int a = 2;
            int b = 2;
            int expectedSum = 4;
            int actualSum = MathUtilities.Add(a, b);
            Assert.AreEqual(expectedSum, actualSum);
        }

        [Test]
        public void FailingTest()
        {
            int a = 2;
            int b = 2;
            int expectedSum = 5;
            int actualSum = MathUtilities.Add(a, b);
            Assert.AreEqual(expectedSum, actualSum);
        }
    }
}