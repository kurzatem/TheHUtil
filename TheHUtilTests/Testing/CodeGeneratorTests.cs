namespace TheHUtilTests.Testing
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TheHUtil.Extensions;
    using TheHUtil.Testing;

    [TestClass]
    public class CodeGeneratorTests
    {
        [TestMethod]
        public void shouldGetAttributesForEachParameterInTestSubjectMethodInstance()
        {
            var testee = new TestSubject();
            var method = testee.GetType().GetMethod("Method");

            var expected = new[]
                {
                    new TestGeneratorParameterCasesAttribute(0, 1, 2),
                    new TestGeneratorParameterCasesAttribute('a', 'b', 2)
                };

            var actual = CodeGenerator.GetTestGeneratorAttributesForMethod(method);

            Assert.IsTrue(expected.SequenceEqual(actual), TestingHelpers.DumpTestSequencesToText(expected, actual));
        }

        [TestMethod]
        public void shouldWriteExpectedTestCode()
        {
            var testee = new TestSubject();
            var methodInfo = testee.GetType().GetMethod("Add2And2");

            var expected = 
@"var expected = 4;

var actual = testee.Add2And2();

Assert.IsTrue(expected.Equals(actual));";

            var actual = CodeGenerator.CreateTestMethodCodeFor(methodInfo, null, 4);

            Assert.IsTrue(expected.Equals(actual, StringComparison.InvariantCultureIgnoreCase));
        }

        private class TestSubject
        {
            public void Method([TestGeneratorParameterCases(0, 1, 2)]int foo, [TestGeneratorParameterCases('a', 'b', 2)]char bar)
            {
            }

            [return: TestGeneratorReturnValueCases(4, 4)]
            public int Add2And2()
            {                
                return 4;
            }
        }
    }
}
