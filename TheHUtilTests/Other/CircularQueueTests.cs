namespace TheHUtilTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TheHUtil.Extensions;
    using TheHUtil;

    [TestClass]
    public class CircularQueueTests
    {
        private CircularQueue<Func<int, int, int>> testee;

        public CircularQueueTests()
        {
            var methods = new Func<int, int, int>[]
            {
                (x, y) => x + y,
                (x, y) => x - y,
                (x, y) => x * y,
                (x, y) => x / y
            };

            this.testee = new CircularQueue<Func<int, int, int>>(methods);
        }
        
        [TestMethod]
        public void shouldCopyUsingToArray()
        {
            var testee = new CircularQueue<int>(new[] { 1, 2, 3, 4 });
            testee.Cycle();
            testee.Cycle();
            var expected = new[] { 3, 4, 1, 2 };

            var actual = testee.ToArray();

            Assert.IsTrue(actual.SequenceEqual(expected), actual.PrintContentsToString(", "));
        }

        [TestMethod]
        public void shouldExhaustQueue()
        {
            var outputs = new List<Func<int, int, int>>(4);
            var initialCount = this.testee.Count;
            this.testee.Cycle();
            for (var iteration = 0; iteration < initialCount; iteration++)
            {
                outputs.Add(this.testee.Dequeue());
            }

            try
            {
                var actual = this.testee.Dequeue();
                Assert.IsTrue(false, outputs.Count + " " + initialCount);
            }
            catch (InvalidOperationException)
            {
                // queues are empty.
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void shouldNotExhaustQueue()
        {
            for(var iteration = 0; iteration < this.testee.Count * 2; iteration++)
            {
                var temp = this.testee.Cycle();
            }

            var actual = this.testee.Cycle();

            Assert.IsFalse(actual.IsNull(), "Queue returned a null.");
        }

        [TestMethod]
        public void shouldOnlyCompleteOneCycle()
        {
            var testee = new CircularQueue<int>(new[] { 1, 2, 3, 4, 5 });
            var expected = 15;

            var actual = 0;
            foreach (var num in testee.OneCompleteCycle)
            {
                actual += num;
            }

            Assert.IsTrue(actual.Equals(expected));
        }
    }
}
