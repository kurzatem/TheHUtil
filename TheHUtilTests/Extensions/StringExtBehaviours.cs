using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheHUtil.Extensions;
using TheHUtil.HelperConstants;

namespace TheHUtilTests.Extensions
{
    [TestClass]
    public class StringExtBehaviours
    {
        char[][] limiters;

        public StringExtBehaviours()
        {
            var staticProperties = typeof(CharConsts).GetProperties(BindingFlags.Public | BindingFlags.Static);
            this.limiters = new char[staticProperties.Length][];
            int index = 0;
            foreach (var property in staticProperties)
            {
                limiters[index] = property.GetGetMethod().Invoke(null, Type.EmptyTypes) as char[];
                index++;
            }

        }

        [TestMethod]
        public void shouldContainAnyNumbers()
        {
            var data = "hfdjks784932";

            var actual = data.ContainsAny(CharConsts.Numbers);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void shouldContainAllNumbers()
        {
            var data = "1234567890dhasfjkoyu";

            var actual = data.ContainsAll(new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void shouldRemoveAllLetters()
        {
            var data = "jfuierwoyt8290ytugrehvjfrbewuig5o4y27t";
            var expected = "82905427";

            var actual = data.RemoveAll(CharConsts.Letters);

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void shouldPreserveAllLetters()
        {
            var data = "5j4k32l1;6h6h6jk478vhjkvytrueofuda;dls  d,o";
            var expected = "jklhhjkvhjkvytrueofudadlsdo";

            var actual = data.Preserve(CharConsts.Letters);

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void shouldParseUsingGenericParser()
        {
            // Setup and test in this order: int, float, and bool.
            var dataInt = "90";
            var dataFloat = "90.01";
            var dataBool = bool.FalseString;

            var expectedInt = 90;
            var expectedFloat = 90.01f;
            var expectedBool = false;

            var actualInt = dataInt.ParseTo<int>();
            var actualFloat = dataFloat.ParseTo<float>();
            var actualBool = dataBool.ParseTo<bool>();

            Assert.IsTrue(actualInt.Equals(expectedInt));
            Assert.IsTrue(actualFloat - expectedFloat < float.Epsilon);
            Assert.IsTrue(actualBool.Equals(expectedBool));
        }
    }
}
