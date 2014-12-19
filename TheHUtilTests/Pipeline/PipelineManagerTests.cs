namespace TheHUtilTests.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TheHUtil;
    using TheHUtil.Extensions;
    using TheHUtil.Pipeline;
    using TheHUtil.Testing;
    
    [TestClass]
    public class PipelineManagerTests
    {
        private PipelineScheme scheme;

        public PipelineManagerTests()
        {
            var nameOfShort = typeof(short).AssemblyQualifiedName;
            var nameOfInt = typeof(int).AssemblyQualifiedName;
            var nameOfTestSubj = typeof(TestSubject).AssemblyQualifiedName;
            var config =
                "{" + nameOfTestSubj + "; Square; " + nameOfShort + "; " + nameOfInt + "; (0,0)}\n" +
                "{" + nameOfTestSubj + "; Squareroot; " + nameOfInt + "; " + nameOfShort + "; (0,1)}\n" +
                "{" + nameOfTestSubj + "; Add3; " + nameOfShort + "; " + nameOfShort + "; (0,2)}\n" +
                "{" + nameOfTestSubj + "; Square; " + nameOfShort + "; " + nameOfInt + "; (0,3)}\n" +
                "{" + nameOfTestSubj + "; Squareroot; " + nameOfInt + "; " + nameOfShort + "; (0,4)}\n" +
                "{" + nameOfTestSubj + "; Subtract3; " + nameOfShort + "; " + nameOfShort + "; (0,5)}\n";
            this.scheme = PipelineScheme.Parse(config);
        }

        // Eh?
        // Remember that the configuration file is going to specify how many items are in a batch for deserialization.
        // I think this refers to the scheme parser.

        [TestMethod]
        public void shouldInsertPipelineStepDirectly()
        {
            var obj = new TestSubject();
            var insertionScheme = new PipelineScheme(new[] { new StepMetadata("Bar", typeof(TestSubject), typeof(int), typeof(bool), "(0,0)") });

            var insertionTestee = new PipelineManager(insertionScheme);

            var result = insertionTestee.InsertPipelineStep<int, bool>(obj.Bar);

            Assert.IsTrue(result.Outcome == StepBuildResults.Completed);
        }
        
        [TestMethod]
        public void shouldBuildSimplePipeline()
        {
            var testee = new PipelineManager();
            var results = testee.BuildPipeline(this.scheme, new TestSubject());

            for (var index = 0; index < results.Count; index++)
            {
                Assert.IsTrue(results[index].Outcome == StepBuildResults.Completed, results.PrintContentsToString('\n'));
            }
        }

        [TestMethod]
        public void shouldReturnAllMethodsFromScheme()
        {
            var testee = new PipelineManager(this.scheme);

            var expected = this.scheme.SelectMany(lom => lom.Select(m => m));

            var actual = testee.GetMetadataForMissingSteps();

            Assert.IsTrue(expected.SequenceEqual(actual), TestingHelpers.DumpTestSequencesToText(expected, actual));
        }

        [TestMethod]
        public async Task shouldCancelDuringBuild()
        {
            var cancellationTestee = new PipelineManager();
            var canceller = new System.Threading.CancellationTokenSource();
            var progressReporter = new PipelineProgress(canceller.Cancel, 50, false);

            var result = await cancellationTestee.BuildPipelineAsync(this.scheme, canceller.Token, progressReporter, new TestSubject());

            var refinedResults = result.Select(r => r.Outcome).ToArray();

            Assert.IsTrue(refinedResults.Contains(StepBuildResults.Cancelled) && refinedResults.Contains(StepBuildResults.Completed), "Token may have expirec before the build took place.\nResults are:\n" + refinedResults.PrintContentsToString(Environment.NewLine));
        }

        [TestMethod]
        public async Task shouldShowWhereBuildWasCancelled()
        {
            // Similar to "shouldCancelDuringBuild", but this time check to be sure that the build is cancelling at the right spot.
            var cancellationTestee = new PipelineManager();
            var canceller = new System.Threading.CancellationTokenSource();
            var progressReporter = new PipelineProgress(canceller.Cancel, 50, false);
            var expected = new List<StepBuildResults>()
            {
                StepBuildResults.Completed, StepBuildResults.Completed, StepBuildResults.Completed, StepBuildResults.Completed,
                StepBuildResults.Cancelled, StepBuildResults.Cancelled
            };
            
            var result = await cancellationTestee.BuildPipelineAsync(this.scheme, canceller.Token, progressReporter, new TestSubject());

            var actual = result.Select(r => r.Outcome).ToArray();

            Assert.IsTrue(expected.SequenceEqual(actual), TestingHelpers.DumpTestSequencesToText(expected, actual));
        }

        [TestMethod]
        public void shouldProcessDataPiece()
        {
            var expected = (short)2;
            var testee = new PipelineManager(this.scheme);
            testee.BuildPipeline(new TestSubject());

            try
            {
                testee.ExecuteOnData(expected);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail("Methods missing from pipeline:\n" + testee.GetMetadataForMissingSteps().PrintContentsToString('\n'));
            }

            var actual = testee.GetResults<short>();

            Assert.IsTrue(expected.Equals(actual[0]));
        }

        [TestMethod]
        public void shouldProcessDataChunk()
        {
            var expected = new short[] { 2, 3, 4, 5, 10 };
            var testee = new PipelineManager(this.scheme);
            testee.BuildPipeline(new TestSubject());

            try
            {
                testee.ExecuteOnCollection(expected);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail("Methods missing from pipeline:\n" + testee.GetMetadataForMissingSteps().PrintContentsToString('\n'));
            }

            var actual = testee.GetResults<short>();

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void shouldRebuildPipeline()
        {
            // Setup test object and states. (states are represented by the scheme objects)
            var rebuildingTestee = new PipelineManager(this.scheme);
            var nameOfShort = typeof(short).AssemblyQualifiedName;
            var nameOfInt = typeof(int).AssemblyQualifiedName;
            var nameOfTestSubj = typeof(TestSubject).AssemblyQualifiedName;
            var secondConfig =
                "{" + nameOfTestSubj + "; Add3; " + nameOfShort + "; " + nameOfShort + "; (0,2)}\n" +
                "{" + nameOfTestSubj + "; Square; " + nameOfShort + "; " + nameOfInt + "; (0,0)}\n" +
                "{" + nameOfTestSubj + "; Squareroot; " + nameOfInt + "; " + nameOfShort + "; (0,1)}\n" +
                "{" + nameOfTestSubj + "; Subtract3; " + nameOfShort + "; " + nameOfShort + "; (0,5)}\n";

            var secondScheme = PipelineScheme.Parse(secondConfig);
            var buildResults = rebuildingTestee.BuildPipeline(new TestSubject());

            foreach (var entry in buildResults)
            {
                Assert.IsTrue(entry.Outcome == StepBuildResults.Completed, "First build did not succeed\n" + buildResults.PrintContentsToString('\n'));
            }

            buildResults = rebuildingTestee.RebuildPipeline(secondScheme, new TestSubject());

            foreach (var entry in buildResults)
            {
                Assert.IsTrue(entry.Outcome == StepBuildResults.Completed, "Second build did not succeed\n" + buildResults.PrintContentsToString('\n'));
            }
        }

        [TestMethod]
        public void shouldCacheUnusedMethodsAtRebuildUsingRebuild()
        {
            var testee = new PipelineManager(this.scheme);
            testee.BuildPipeline(new TestSubject());

            var nameOfShort = typeof(short).AssemblyQualifiedName;
            var nameOfInt = typeof(int).AssemblyQualifiedName;
            var nameOfTestSubj = typeof(TestSubject).AssemblyQualifiedName;
            var secondConfig =
                "{" + nameOfTestSubj + "; Add3; " + nameOfShort + "; " + nameOfShort + "; (0,2)}\n" +
                "{" + nameOfTestSubj + "; Square; " + nameOfShort + "; " + nameOfInt + "; (0,0)}\n" +
                "{" + nameOfTestSubj + "; Squareroot; " + nameOfInt + "; " + nameOfShort + "; (0,1)}\n" +
                "{" + nameOfTestSubj + "; Subtract3; " + nameOfShort + "; " + nameOfShort + "; (0,5)}\n";

            var secondScheme = PipelineScheme.Parse(secondConfig);

            testee.RebuildPipeline(secondScheme, new TestSubject());

            var cacheCollection = testee.GetType().GetField("unusedSteps", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var actualCollection = (Dictionary<StepMetadata, StepWrapper>)cacheCollection.GetValue(testee);

            Assert.IsTrue(actualCollection.Count > 0, actualCollection.Values.PrintContentsToString('\n'));
        }

        [TestMethod]
        public void shouldNotCacheUnusedMethodsAtRebuildUsingBuild()
        {
            var testee = new PipelineManager(this.scheme);
            testee.BuildPipeline(new TestSubject());

            var nameOfShort = typeof(short).AssemblyQualifiedName;
            var nameOfInt = typeof(int).AssemblyQualifiedName;
            var nameOfTestSubj = typeof(TestSubject).AssemblyQualifiedName;
            var secondConfig =
                "{" + nameOfTestSubj + "; Add3; " + nameOfShort + "; " + nameOfShort + "; (0,2)}\n" +
                "{" + nameOfTestSubj + "; Square; " + nameOfShort + "; " + nameOfInt + "; (0,0)}\n" +
                "{" + nameOfTestSubj + "; Squareroot; " + nameOfInt + "; " + nameOfShort + "; (0,1)}\n" +
                "{" + nameOfTestSubj + "; Subtract3; " + nameOfShort + "; " + nameOfShort + "; (0,5)}\n";

            var secondScheme = PipelineScheme.Parse(secondConfig);

            testee.BuildPipeline(secondScheme, new TestSubject());

            var cacheCollection = testee.GetType().GetField("unusedSteps", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var actualCollection = (Dictionary<StepMetadata, StepWrapper>)cacheCollection.GetValue(testee);

            Assert.IsTrue(actualCollection.IsNull() || actualCollection.Count == 0);
        }

        public class TestSubject
        {
            public void Foo(int amount)
            {
            }

            public bool Bar(int amount)
            {
                return true;
            }

            public int Square(short value)
            {
                return value * value;
            }

            public short Squareroot(int value)
            {
                return (short)Math.Sqrt(value);
            }

            public short Add3(short value)
            {
                return (short)(value + 3);
            }

            public short Subtract3(short value)
            {
                return (short)(value - 3);
            }

            public string ToString(string format)
            {
                return base.ToString();
            }
        }
    }
}
