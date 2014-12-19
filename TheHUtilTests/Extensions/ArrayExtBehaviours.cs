namespace TheHUtilTests.Extensions
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TheHUtil.Extensions;
    using TheHUtil.HelperConstants;

    [TestClass]
    public class ArrayExtBehaviours
    {
        [TestMethod]
        public void shouldPrintContentsToString()
        {
            var strings = new[] { "Bob", "Fred", "Dilbert" };
            var numbers = new[] { 1, 2, 3, 4, 5, 108783491 };
            var separatorStrings = new[] { "\n", ", ", " ", ".", " When the cows come home. " };
            var separatorChars = new[] { '\n', ',', '.', ' ' };

            foreach (var sep in separatorStrings)
            {
                var expected = string.Format("Bob{0}Fred{0}Dilbert", sep);
                var actual = strings.PrintContentsToString(sep);

                Assert.IsTrue(expected == actual, string.Format("{0}Expected:{0}{1}{0}Actual:{0}{2}", sep, expected, actual));
            }

            foreach (var sep in separatorChars)
            {
                var expected = string.Format("Bob{0}Fred{0}Dilbert", sep);
                var actual = strings.PrintContentsToString(sep);

                Assert.IsTrue(expected == actual, string.Format("{0}{1}{0}{2}", '\n', expected, actual));
            }
        }
    }
}
