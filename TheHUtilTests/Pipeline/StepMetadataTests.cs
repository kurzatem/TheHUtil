namespace TheHUtilTests.Pipeline
{
    using System;
    
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TheHUtil.Pipeline;

    [TestClass]
    public class StepMetadataTests
    {
        [TestMethod]
        public void shouldParseUnnamedStepData()
        {
            var data = "{" + typeof(PipelineManagerTests.TestSubject).AssemblyQualifiedName + "; Foo; " + typeof(int).AssemblyQualifiedName + "; (0,0)}";
            var expected = new Tuple<string, Type, string>("Foo", typeof(int), "(0,0)");

            var actual = StepMetadata.Parse(data);

            Assert.AreEqual(expected.Item1, actual.MethodName);
            Assert.AreEqual(expected.Item2, actual.InputType);
            Assert.AreEqual(expected.Item3, actual.Location.ToString());
        }

        [TestMethod]
        public void shouldParseNamedStepData()
        {
            var expected = new StepMetadata("Parse", typeof(int), typeof(string), typeof(int), "(0,0)");
            var data = expected.ToString();

            var actual = StepMetadata.Parse(data);

            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void shouldCheckEqualityOfDelegateToMetadata()
        {
            Func<string, int> data = int.Parse;

            var testee = new StepMetadata("Parse", typeof(int), typeof(string), typeof(int), "");

            Assert.IsTrue(testee.DoesMethodMatch(data));
        }
    }
}
