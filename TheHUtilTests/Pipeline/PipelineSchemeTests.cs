namespace TheHUtilTests.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TheHUtil.Extensions;
    using TheHUtil.Pipeline;

    [TestClass]
    public class PipelineSchemeTests
    {
        private PipelineScheme testee;

        private Dictionary<Type, string> typeNames;

        public PipelineSchemeTests()
        {
            this.typeNames = new Dictionary<Type, string>()
            {
                {typeof(int), typeof(int).AssemblyQualifiedName},
                {typeof(bool), typeof(bool).AssemblyQualifiedName},
                {typeof(short), typeof(short).AssemblyQualifiedName},
                {typeof(float), typeof(float).AssemblyQualifiedName},
                {typeof(string), typeof(string).AssemblyQualifiedName}
            };

            var intTypeName = this.typeNames[typeof(int)];
            var boolTypeName = this.typeNames[typeof(bool)];
            var shortTypeName = this.typeNames[typeof(short)];
            var floatTypeName = this.typeNames[typeof(float)];
            var stringTypeName = this.typeNames[typeof(string)];
            var parseMethodName = "Parse";
            var toStringMethodName = "ToString";
            var equalsMethodName = "Equals";
            this.testee = PipelineScheme.Parse
                (
                    "{" + intTypeName + "; " + parseMethodName + "; " + stringTypeName + "; " + intTypeName + "; (0,0)}\n" +
                    "{" + intTypeName + "; " + toStringMethodName + "; " + stringTypeName + "; " + stringTypeName + "; (0,0)}\n" +
                    "{" + intTypeName + "; " + equalsMethodName + "; " + intTypeName + "; " + boolTypeName + "; (0,0)}\n" +
                    "{" + shortTypeName + "; " + parseMethodName + "; " + stringTypeName + "; " + shortTypeName + "; (0,0)}\n" +
                    "{" + shortTypeName + "; " + toStringMethodName + "; " + stringTypeName + "; " + stringTypeName + "; (0,0)}\n" +
                    "{" + shortTypeName + "; " + equalsMethodName + "; " + shortTypeName + "; " + boolTypeName + "; (0,0)}\n" +
                    "{" + floatTypeName + "; " + parseMethodName + "; " + stringTypeName + "; " + floatTypeName + "; (0,0)}\n" +
                    "{" + floatTypeName + "; " + toStringMethodName + "; " + stringTypeName + "; " + stringTypeName + "; (0,0)}\n" +
                    "{" + floatTypeName + "; " + equalsMethodName + "; " + floatTypeName + "; " + boolTypeName + "; (0,0)}\n"
                );
        }

        [TestMethod]
        public void shouldCloneScheme()
        {            
            var data = new List<StepMetadata>()
            {
                new StepMetadata("Parse", typeof(int), typeof(string), typeof(int), "(0,0)"),
                new StepMetadata("ToString", typeof(int), typeof(string), typeof(string), "(0,0)"),
                new StepMetadata("Equals", typeof(int), typeof(int), typeof(bool), "(0,0)")
            };

            var cloningTestee = new PipelineScheme(data);

            var actual = cloningTestee.Clone();

            Assert.IsFalse(cloningTestee.ReferenceEquals(actual));
            Assert.IsTrue(cloningTestee.SequenceEqual(actual));
        }

        [TestMethod]
        public void shouldDesignSchemeParser()
        {
        }


    }
}
