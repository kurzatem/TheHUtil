using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheHUtil.Extensions;

namespace TheHUtilTests.Converters
{
    [TestClass]
    public class XElementExtBehaviours
    {
        [TestMethod]
        public void shouldHaveChildValues()
        {
            var data = XElement.Parse("<parent><child>10</child><child>20</child></parent>");

            var result = data.ChildrenHaveContents();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void shouldTrimChildrenValuesReturningCurrentNodeValue()
        {
            var data = XElement.Parse("<Root>This is <b />the parent content</Root>");
            var expectedBool = false;
            var actualBool = data.ChildrenHaveContents();
            var expectedString = "This is the parent content";
            var actualString = data.TrimChildrenValues();

            Assert.IsTrue(actualBool == expectedBool);
            Assert.IsTrue(expectedString == actualString, "\n" + expectedString + "stuff\n" + actualString + "stuff");

            data = XElement.Parse("<Root>This is <b>Children First!</b>the parent content</Root>");
            expectedBool = true;
            actualBool = data.ChildrenHaveContents();
            actualString = data.TrimChildrenValues();

            Assert.IsTrue(actualBool == expectedBool);
            Assert.IsTrue(expectedString == actualString, "\n" + expectedString + "stuff\n" + actualString + "stuff");
        }

        [TestMethod]
        public void shouldTryUsingPreExistingParseMethod()
        {
            var data = XElement.Parse("<root>123</root>");
            var expected = 123;
            int actual = data.ParseValue<int>();

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void shouldParseCollectionFromValue()
        {
            var data = XElement.Parse("<root>109</root>");
            var expectedNumber = 109;
            var actualNumber = data.ParseValue(int.Parse);

            Assert.IsTrue(expectedNumber == actualNumber);

            data = XElement.Parse("<root>100, 101, 102, 103</root>");
            var expectedArray = new[] { 100, 101, 102, 103 };
            var actualArray = data.ValueToCollection(int.Parse, new[] { ", " });

            Assert.IsTrue(expectedArray.SequenceEqual(actualArray));
        }
    }
}
