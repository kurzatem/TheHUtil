namespace TheHUtil.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using TheHUtil.Extensions;
    using TheHUtil.XmlAndJson;

    /// <summary>
    /// Defines the <see cref="PipelineManager"/> class.
    /// </summary>
    /// <remarks>
    /// This is a dynamic pipeline that can be modified at any point during execution. 
    /// For more control over the flow of data as it's being processed, simply derive from this class and override the protected <see cref="GetNextStepLocation"/> method. 
    /// Generally speaking, to use this class, you must follow a few procedures, which should be intuitive. 
    /// 1) You must provide an instance of the <see cref="PipelineScheme"/> class to guide the manager. 
    /// 2) Build the pipeline by doing one of the following. 
    /// 2a) For the collection of objects technique, remember that there is a fair amount of reflection involved, so less typing now == more cpu cycles at runtime. 
    /// 2b) For the "delegate at a time", it takes more code, but doesn't require all of the reflection. This is only done synchronouly.
    /// 2.1) For synchronous initialization, use <see cref="BuildPipeline"/> for a collection of objects, or <see cref="InsertPipelineStep"/> for direct delegate insertion. 
    /// For asynchronous initialization, use <see cref="BuildPipelineAsync"/> to run asynchronously. 
    /// 3) Once the pipeline is built, you may then execute the pipeline on a single or a collection of objects by calling any of the "Execute..." methods. 
    /// </remarks>
    [Serializable]
    public class PipelineManager
    {
        // HACK: Look at me!
        // BUG: Important idea for the pipeline!
        // REFACTOR: Read carefully as this has some details.
        // IDEA: Configurable/reconfigurable pipeline idea in the comment below.
        /* The KingISS/xml/json file should specify:
         * the fully qualified type name (namespace(s) and class),
         * the type of step,
         * and location via a coordinate notation
         * 
         * Internally, the pipeline could keep a dictionary of wrapped steps that would be:
         * TKey: Type,
         * TValue: step wrapper with object type name inside.
         * 
         * Then the configuration data could tell the pipeline where the steps go using the ANC.
         * I could easily write a behavior that would accept the config data and recreate itself using the start to finish building model.
         * I think for the ANC it wouldn't matter which came first as I doubt there would be more than 26 steps used from beginning to end.
         * Regardless, the logic needed to determine if the next char is a letter is trivial.
         */

        // Only thing left is the KingISS/JSON/XML parser for this.
        
        /// <summary>
        /// The current scheme that the pipeline is based on. This cannot be changed between the building of the pipeline and the execution of it.
        /// </summary>
        private PipelineScheme currentScheme;
        
        /// <summary>
        /// Determines whether the pipeline has been finalized in preparation for execution.
        /// </summary>
        private bool isFinalized;

        /// <summary>
        /// The queue that holds all the results from the pipeline's execution.
        /// </summary>
        private Queue<object> output;
        
        /// <summary>
        /// The collection of step metadata and wrappers that represent the steps of a pipeline.
        /// </summary>
        private Dictionary<StepMetadata, StepWrapper> pipelineSteps;

        /// <summary>
        /// The collection that indexes all of the steps of a pipeline.
        /// </summary>
        private Dictionary<StepLocation, StepMetadata> pipelineStepLocationIndex;

        /// <summary>
        /// The stage at which this pipeline manager is currently at.
        /// </summary>
        private ProcessingStages ProcessingStage;

        /// <summary>
        /// The collection of unused steps. The contents of this collection are dependant upon the value of <see cref="PipelineManager.shouldCacheMethods"/>.
        /// </summary>
        private Dictionary<StepMetadata, StepWrapper> unusedSteps;

        /// <summary>
        /// Gets or sets the current location. This is to be used in the overriding of <see cref="PipelineManager.GetNextStep"/>.
        /// </summary>
        protected StepLocation CurrentLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether the pipeline is ready for finalization before execution.
        /// </summary>
        public bool IsReady
        {
            get
            {
                return !this.currentScheme.IsNull() && this.currentScheme.IsEmpty && this.pipelineSteps.Count != 0;
            }
        }

        /// <summary>
        /// Initializes the static <see cref="PipelineManager"/>.
        /// </summary>
        static PipelineManager()
        {
            // TODO: build the pipeline manager KingISS/xml/json deserializer.

            // data file should place all steps as a single dimensional array
            // the location string will provide the location data.
            // could be serialized listing all branches in a tier before moving on to the next tier.
            // for visualization purposes:

            /*          0,0
             *         /   \
             *      0,1     1,1
             *     /   \       \
             *  0,2     1,2     2,2
             *  
             * would be stored as: ...(0,0) ...(0,1) ...(1,1) ...(0,2) ...(1,2) ...(2,2)
             * the assumption is that " ..." represents the rest of the data.
             */
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineManager"/> class.
        /// </summary>
        public PipelineManager()
        {
            this.isFinalized = false;
            this.pipelineSteps = new Dictionary<StepMetadata, StepWrapper>();
            this.pipelineStepLocationIndex = new Dictionary<StepLocation, StepMetadata>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineManager"/> class.
        /// </summary>
        /// <param name="scheme">The configuration schematic for the pipeline.</param>
        public PipelineManager(PipelineScheme scheme) :
            this()
        {
            this.InsertPipelineScheme(scheme);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineManager"/> class that is ready for execution.
        /// </summary>
        /// <param name="scheme">The configuration schematic for the pipeline.</param>
        /// <param name="objects">A collection of objects that are needed to build the pipeline.</param>
        public PipelineManager(PipelineScheme scheme, params object[] objects) :
            this(scheme)
        {
            this.BuildPipeline(objects);
        }
        
        /// <summary>
        /// Gets the method that matches the metadata provided.
        /// </summary>
        /// <param name="methods">A collection of <see cref="MethodInfo"/> instances.</param>
        /// <param name="meta">The metadata to match from the collection of <see cref="MethodInfo"/>.</param>
        /// <returns>The <see cref="MethodInfo"/> that matches the metadata provided.</returns>
        private static MethodInfo GetMethodThatMatchesMetadata(MethodInfo[] methods, StepMetadata meta)
        {
            var methodsWithCorrectNames = methods.Where(m => m.Name.Equals(meta.MethodName)).ToArray();
            for (var index = 0; index < methodsWithCorrectNames.Length; index++)
            {
                var parameters = methodsWithCorrectNames[index].GetParameters();
                if (parameters.Length != 1)
                {
                    throw new ArgumentException("The method: " + methodsWithCorrectNames[index].Name + " is not valid. Methods can only have 1 input parameter.");
                }

                if (parameters[0].ParameterType.Equals(meta.InputType) && methodsWithCorrectNames[index].ReturnType.Equals(meta.OutputType))
                {
                    return methodsWithCorrectNames[index];
                }
            }

            return null;
        }

        /// <summary>
        /// Increments and reports the progress of the pipeline manager's operation.
        /// </summary>
        /// <param name="progressIncrement">The amount to increment the progress.</param>
        /// <param name="progressAmount">The un-incremented progress.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> generic interface.</param>
        /// <returns>The incremented progress of an operation.</returns>
        private static float IncrementAndReportProgress(float progressIncrement, float progressAmount, IProgress<int> progress)
        {
            if (!progress.IsNull())
            {
                progressAmount = progressAmount + progressIncrement;
                progress.Report((int)progressAmount);
            }

            return progressAmount;
        }

        /// <summary>
        /// Creates instances of the <see cref="StepInsertionResult"/> class for every metadata provided.
        /// </summary>
        /// <param name="metas">A metadata collection to create <see cref="StepInsertionResult"/> instances for.</param>
        /// <param name="results">A collection of <see cref="StepInsertionResult"/> instances to add to.</param>
        /// <param name="buildResult">The result that the <see cref="StepInsertionResult"/> instances will utilize.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances.</returns>
        private static List<StepInsertionResult> MakeResultsForMetas(List<StepMetadata> metas, List<StepInsertionResult> results, StepBuildResults buildResult)
        {
            for (var index = 0; index < metas.Count; index++)
            {
                // compare the results and see if there are metas that don't have any matches.
                bool shouldAdd = true;
                for (var innerIndex = 0; innerIndex < results.Count; innerIndex++)
                {
                    if (metas[index].Equals(results[innerIndex].Metadata))
                    {
                        shouldAdd = false;
                    }
                }

                if (shouldAdd)
                {
                    results.Add(new StepInsertionResult(metas[0], buildResult));
                }
            }

            return results;
        }

        /// <summary>
        /// Collects and stores all the methods that the configuration schematic calls for use in the pipeline.
        /// </summary>
        /// <typeparam name="T">The type of object that is passed in.</typeparam>
        /// <param name="obj">The object to retrieve the method(s) from.</param>
        /// <param name="shouldCheckCache">Determines if the wrapper cache should be checked before extracting the method from an object.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances that tell what happened during the execution of this method.</returns>
        private List<StepInsertionResult> CompileStepsUsingObject<T>(T obj, bool shouldCheckCache)
        {
            var typeOfObj = obj.GetType();
            var methods = typeOfObj.GetMethods();
            List<StepMetadata> metas;
            var results = new List<StepInsertionResult>();
            if (this.currentScheme.TryRemove(typeOfObj, out metas))
            {
                results.Capacity = metas.Count;
                for (var index = 0; index < metas.Count; index++)
                {
                    results.Add(this.ExtractMethodAndWrapMethod<T>(obj, shouldCheckCache, methods, metas[index]));
                }
            }
            else
            {
                results.Add(new StepInsertionResult(typeOfObj, StepBuildResults.ObjectNotRequired));
            }

            return results;
        }

        /// <summary>
        /// Compiles all the needed steps for a pipeline as specified by the current configuration.
        /// </summary>
        /// <typeparam name="T">The type of object to extract the methods from.</typeparam>
        /// <param name="obj">The object that all instance methods will bind to.</param>
        /// <param name="shouldCheckCache">Determines if the wrapper cache should be checked before extracting the method from an object.</param>
        /// <param name="progressIncrement">The amount that the progress will be incremented by.</param>
        /// <param name="progressAmount">The total or estimated progress amount.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> instance that can serve a cancellation notice.</param>
        /// <param name="progress">The progress reporter to report progress to.</param>
        /// <returns>A <see cref="Task"/> instance that embodies the work done in this method.</returns>
        private Task<List<StepInsertionResult>> CompileStepsUsingObjectAsync<T>(T obj, bool shouldCheckCache, float progressIncrement, ref float progressAmount, CancellationToken cancellationToken, IProgress<int> progress)
        {
            var result = new TaskCompletionSource<List<StepInsertionResult>>();
            var typeOfObj = obj.GetType();
            var methods = typeOfObj.GetMethods();
            List<StepMetadata> metas;
            progressAmount = PipelineManager.IncrementAndReportProgress(progressIncrement, progressAmount, progress);
            if (this.currentScheme.TryRemove(typeOfObj, out metas))
            {
                var results = new List<StepInsertionResult>(metas.Count);
                for (var index = 0; index < metas.Count; index++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        result.SetResult(PipelineManager.MakeResultsForMetas(metas, results, StepBuildResults.Cancelled));
                        break;
                    }
                    else
                    {
                        results.Add(this.ExtractMethodAndWrapMethod(obj, shouldCheckCache, methods, metas[index]));
                    }

                    progressAmount = PipelineManager.IncrementAndReportProgress(progressIncrement, progressAmount, progress);
                }
            }
            else
            {
                result.SetResult(new List<StepInsertionResult>() { new StepInsertionResult(typeOfObj, StepBuildResults.ObjectNotRequired) });
            }

            return result.Task;
        }

        /// <summary>
        /// Inserts a wrapped method that has either been retrieved from the cache or extracted and wrapped from the given object.
        /// </summary>
        /// <typeparam name="T">The type of object that the methods are to be extracted from.</typeparam>
        /// <param name="obj">The object that the instance methods will be bound to.</param>
        /// <param name="shouldCheckCache">Determines if the wrapper cache should be checked before extracting the method from an object.</param>
        /// <param name="methods">The collection of all public, instance methods from the given object type.</param>
        /// <param name="meta">The <see cref="StepMetadata"/> that embodies a step in a pipeline.</param>
        /// <returns>An instance of the <see cref="StepInsertionResult"/> class that holds the results of this operation.</returns>
        private StepInsertionResult ExtractMethodAndWrapMethod<T>(T obj, bool shouldCheckCache, MethodInfo[] methods, StepMetadata meta)
        {
            StepWrapper step = null;
            if (shouldCheckCache && !this.unusedSteps.IsNull() && this.unusedSteps.TryGetValue(meta, out step))
            {
                this.unusedSteps.Remove(meta);
            }
            else
            {
                var method = PipelineManager.GetMethodThatMatchesMetadata(methods, meta);
                if (!method.IsNull())
                {
                    step = StepWrapper.CreateWrapper(obj, method);
                }
            }

            if (!step.IsNull())
            {
                this.pipelineSteps.Add(meta, step);
                return new StepInsertionResult(meta, StepBuildResults.Completed);
            }
            else
            {
                return new StepInsertionResult(meta, StepBuildResults.MethodNotFound);
            }
        }

        /// <summary>
        /// Prepares the cache for use.
        /// </summary>
        private void PrepareCache()
        {
            if (this.unusedSteps.IsNull())
            {
                this.unusedSteps = new Dictionary<StepMetadata, StepWrapper>(this.pipelineSteps);
            }
            else
            {
                foreach (var step in this.pipelineSteps)
                {
                    if (this.unusedSteps.ContainsKey(step.Key))
                    {
                        this.unusedSteps.Add(step.Key, step.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Prepares the pipeline for execution.
        /// </summary>
        private void PreparePipelineForExecution()
        {
            string exceptionMessage;
            if (this.pipelineSteps.Count == 0)
            {
                exceptionMessage = "Pipeline has not been built";
                if (this.currentScheme.IsNull() || this.currentScheme.IsEmpty)
                {
                    exceptionMessage = string.Concat(exceptionMessage, " and no configuration has been inserted.");
                }
                else
                {
                    exceptionMessage = string.Concat(exceptionMessage, ".");
                }

                throw new InvalidOperationException(exceptionMessage);
            }
            else
            {
                if (!this.isFinalized)
                {
                    foreach (var step in this.pipelineSteps.Keys)
                    {
                        this.pipelineStepLocationIndex.Add(step.Location, step);
                    }

                    this.output = new Queue<object>();
                    this.isFinalized = true;
                    this.ProcessingStage = ProcessingStages.Unstarted;
                    this.CurrentLocation = new StepLocation(0, 0);
                }
            }
        }

        /// <summary>
        /// Processes a piece of data through a method defined by the metadata passed in.
        /// </summary>
        /// <param name="piece">The data to process.</param>
        /// <param name="step">The metadata that represents the method to call.</param>
        /// <returns>The piece of data that has been modified.</returns>
        private object ProcessPiece(object piece, StepMetadata step)
        {
            var executionStep = this.pipelineSteps[step];
            var outputFromStep = executionStep.Execute(piece);
            this.GetNextStepLocation();
            if (this.CurrentLocation.Equals(StepLocation.InvalidLocation))
            {
                this.output.Enqueue(outputFromStep);
                this.ProcessingStage = ProcessingStages.Completed;
            }
            else
            {
                this.ProcessingStage = ProcessingStages.Processing;
            }

            return outputFromStep;
        }

        /// <summary>
        /// Checks if the configuration schematic is null and throws an exception if it is.
        /// </summary>
        private void ThrowIfSchemeIsNull()
        {
            if (this.currentScheme.IsNull())
            {
                throw new InvalidOperationException("Cannot build a pipeline without a scheme. Insert one before, or with build call.");
            }
        }

        /// <summary>
        /// Performs the work for asynchronous building and rebuilding of the pipeline.
        /// </summary>
        /// <param name="shouldCheckCache">Determines if the wrapper cache should be checked before extracting the method from an object.</param>
        /// <param name="cancellationToken">The token that cancels the execution of the pipeline.</param>
        /// <param name="progress">The instance that implements the <see cref="IProgress"/> generic interface for reporting progress.</param>
        /// <param name="objects">A collection of objects that will be used in the building of the pipeline.</param>
        /// <returns>A <see cref="Task>"/> that embodies the work done in this method.</returns>
        private async Task<IList<StepInsertionResult>> WorkerForAsyncBuildAndRebuild(bool shouldCheckCache, CancellationToken cancellationToken, IProgress<int> progress, object[] objects)
        {
            this.ThrowIfSchemeIsNull();
            await Task.Yield();
            if (this.pipelineSteps.Count > 0)
            {
                this.pipelineSteps.Clear();
            }

            var results = new List<StepInsertionResult>(objects.Length);
            if (!cancellationToken.IsCancellationRequested)
            {
                var progressIncrement = (float)(this.currentScheme.Count * 2) / 1f;
                var progressAmount = 0f;
                for (var index = 0; index < objects.Length; index++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        results.AddRange(this.WriteCancellationLogs(results));
                        break;
                    }
                    else
                    {
                        results.AddRange(await this.CompileStepsUsingObjectAsync(objects[index], shouldCheckCache, progressIncrement, ref progressAmount, cancellationToken, progress));
                    }
                }
            }
            else
            {
                results = this.WriteCancellationLogs(null);
            }

            return results;
        }

        /// <summary>
        /// Performs the work for asynchronous execution of the pipeline.
        /// </summary>
        /// <typeparam name="T">The type of data that is to be processed.</typeparam>
        /// <param name="data">The object to process.</param>
        /// <param name="progressIncrement">The amount to increment progress reporting.</param>
        /// <param name="progressAmount">The amount of progress prior to calling this method.</param>
        /// <param name="cancellationToken">The token that cancels the execution of the pipeline.</param>
        /// <param name="progress">The instance that implements the <see cref="IProgress"/> generic interface for reporting progress.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done by this method.</returns>
        private Task<float> WorkerForAsyncExecution<T>
            (
                T data,
                float progressIncrement,
                float progressAmount,
                CancellationToken cancellationToken,
                IProgress<int> progress
            )
        {
            var result = new TaskCompletionSource<float>();
            if (!cancellationToken.IsCancellationRequested)
            {
                this.PreparePipelineForExecution();
                object piece = data;
                this.ProcessingStage = ProcessingStages.Processing;
                while (!this.ProcessingStage.Equals(ProcessingStages.Completed) || cancellationToken.IsCancellationRequested)
                {
                    piece = this.ProcessPiece(piece, this.pipelineStepLocationIndex[this.CurrentLocation]);
                    result.SetResult(PipelineManager.IncrementAndReportProgress(progressIncrement, progressAmount, progress));
                }
            }

            return result.Task;
        }

        /// <summary>
        /// Creates a collection of <see cref"StepInsertionResult"/> instances that all reflect where the build was cancelled.
        /// </summary>
        /// <param name="results">An optional collection of <see cref="StepInsertionResult"/> instances to either add to or create.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances.</returns>
        private List<StepInsertionResult> WriteCancellationLogs(List<StepInsertionResult> results = null)
        {
            if (results.IsNull())
            {
                results = new List<StepInsertionResult>(this.currentScheme.Count);
            }

            foreach (var metas in this.currentScheme)
            {
                results.AddRange(PipelineManager.MakeResultsForMetas(metas, results, StepBuildResults.Cancelled));
            }

            return results;
        }

        /// <summary>
        /// Gets the next location of the next step in the pipeline. The default behavior is to advance to the next tier of the same branch.
        /// </summary>
        /// <remarks>To override, be sure to set the protected property <see cref="CurrentLocation"/> to either a valid location or <see cref="StepLocation.InvalidLocation"/> to indicate processing is complete.</remarks>
        protected virtual void GetNextStepLocation()
        {
            var nextLocation = new StepLocation(this.CurrentLocation.Branch, (short)(this.CurrentLocation.Tier + 1));
            if (!this.pipelineStepLocationIndex.ContainsKey(nextLocation))
            {
                this.CurrentLocation = StepLocation.InvalidLocation;
            }
            else
            {
                this.CurrentLocation = nextLocation;
            }
        }

        /// <summary>
        /// Builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public IList<StepInsertionResult> BuildPipeline(params object[] objects)
        {
            this.ThrowIfSchemeIsNull();
            if (this.pipelineSteps.Count > 0)
            {
                this.pipelineSteps.Clear();
            }

            var results = new List<StepInsertionResult>(objects.Length);
            for (var index = 0; index < objects.Length; index++)
            {
                results.AddRange(this.CompileStepsUsingObject(objects[index], false));
            }

            return results;
        }

        /// <summary>
        /// Builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="scheme">The configuration schematic to build the pipeline to.</param>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public IList<StepInsertionResult> BuildPipeline(PipelineScheme scheme, params object[] objects)
        {
            this.InsertPipelineScheme(scheme);
            return this.BuildPipeline(objects);
        }

        /// <summary>
        /// Asynchronously builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public async Task<IList<StepInsertionResult>> BuildPipelineAsync(params object[] objects)
        {
            return await this.BuildPipelineAsync(CancellationToken.None, null, objects);
        }

        /// <summary>
        /// Asynchronously builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="cancellationToken">The token that signals that the build process should be cancelled. This can be accomplished without throwing an exception.</param>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public async Task<IList<StepInsertionResult>> BuildPipelineAsync(CancellationToken cancellationToken, params object[] objects)
        {
            return await this.BuildPipelineAsync(cancellationToken, null, objects);
        }

        /// <summary>
        /// Asynchronously builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public async Task<IList<StepInsertionResult>> BuildPipelineAsync(IProgress<int> progress, params object[] objects)
        {
            return await this.BuildPipelineAsync(CancellationToken.None, progress, objects);
        }

        // The following overload does the work

        /// <summary>
        /// Asynchronously builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="cancellationToken">The token that signals that the build process should be cancelled. This can be accomplished without throwing an exception.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public async Task<IList<StepInsertionResult>> BuildPipelineAsync
            (
                CancellationToken cancellationToken,
                IProgress<int> progress,
                params object[] objects
            )
        {
            var results = await this.WorkerForAsyncBuildAndRebuild(false, cancellationToken, progress, objects);

            if (!progress.IsNull())
            {
                progress.Report(100);
            }

            return results;
        }

        /// <summary>
        /// Asynchronously builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="scheme">The configuration schematic to build the pipeline to.</param>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public async Task<IList<StepInsertionResult>> BuildPipelineAsync(PipelineScheme scheme, params object[] objects)
        {
            return await this.BuildPipelineAsync(scheme, CancellationToken.None, null, objects);
        }

        /// <summary>
        /// Asynchronously builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="scheme">The configuration schematic to build the pipeline to.</param>
        /// <param name="cancellationToken">The token that signals that the build process should be cancelled. This can be accomplished without throwing an exception.</param>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public async Task<IList<StepInsertionResult>> BuildPipelineAsync(PipelineScheme scheme, CancellationToken cancellationToken, params object[] objects)
        {
            return await this.BuildPipelineAsync(scheme, CancellationToken.None, null, objects);
        }

        /// <summary>
        /// Asynchronously builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="scheme">The configuration schematic to build the pipeline to.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public async Task<IList<StepInsertionResult>> BuildPipelineAsync(PipelineScheme scheme, IProgress<int> progress, params object[] objects)
        {
            return await this.BuildPipelineAsync(scheme, CancellationToken.None, progress, objects);
        }

        /// <summary>
        /// Asynchronously builds, or rebuilds without caching, a pipeline using the provided objects to the specification of the configuration schematic.
        /// </summary>
        /// <param name="scheme">The configuration schematic to build the pipeline to.</param>
        /// <param name="cancellationToken">The token that signals that the build process should be cancelled. This can be accomplished without throwing an exception.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <param name="objects">The objects that house the methods that make up the pipeline.</param>
        /// <returns>A collection of <see cref="StepInsertionResult"/> instances detailing the outcome of every step of the way.</returns>
        public async Task<IList<StepInsertionResult>> BuildPipelineAsync(PipelineScheme scheme, CancellationToken cancellationToken, IProgress<int> progress, params object[] objects)
        {
            this.InsertPipelineScheme(scheme);
            return await this.BuildPipelineAsync(cancellationToken, progress, objects);
        }
        
        /// <summary>
        /// Gets the metadatas that were not supplied with matching methods. This is primarily for debugging purposes.
        /// </summary>
        /// <param name="throwIfEmpty">Throws an exception if there are no metadatas without methods.</param>
        /// <returns>A collection of <see cref="StepMetadata"/> instances that specify which methods have not been supplied to the <see cref="PipelineManager"/> instance.</returns>
        public IEnumerable<StepMetadata> GetMetadataForMissingSteps(bool throwIfEmpty = false)
        {
            if (!this.currentScheme.IsNull() && !this.currentScheme.IsEmpty)
            {
                foreach (var metaCollection in this.currentScheme)
                {
                    foreach (var meta in metaCollection)
                    {
                        yield return meta;
                    }
                }
            }
            else
            {
                if (throwIfEmpty)
                {
                    throw new InvalidOperationException("Configuration schematic is empty or null.");
                }
                else
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// Gets the results of the pipeline execution.
        /// </summary>
        /// <typeparam name="TOut">The type of results to retrieve.</typeparam>
        /// <returns>An indexable collection of results.</returns>
        public IList<TOut> GetResults<TOut>()
        {
            return this.output.Where(op => op is TOut).Select(op => (TOut)op).ToArray();
        }

        /// <summary>
        /// Executes the pipeline on a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of object to execute the pipeline on.</typeparam>
        /// <param name="collection">The collection of objects to execute the pipeline on.</param>
        public void ExecuteOnCollection<T>(IEnumerable<T> collection)
        {
            if (collection is IList<T>)
            {
                var chunk = collection as IList<T>;
                for (var index = 0; index < chunk.Count; index++)
                {
                    this.CurrentLocation = new StepLocation(0, 0);
                    this.ExecuteOnData(chunk[index]);
                }
            }
            else
            {
                foreach (var data in collection)
                {
                    this.ExecuteOnData(data);
                }
            }
        }

        // Work in async execution methods is done in a private worker method.

        /// <summary>
        /// Asynchronously executes the pipeline on a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of object to execute the pipeline on.</typeparam>
        /// <param name="collection">The collection of objects to execute the pipeline on.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done here.</returns>
        public async Task ExecuteOnCollectionAsync<T>(IEnumerable<T> collection)
        {
            await this.ExecuteOnCollectionAsync(collection, CancellationToken.None, null);
        }

        /// <summary>
        /// Asynchronously executes the pipeline on a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of object to execute the pipeline on.</typeparam>
        /// <param name="collection">The collection of objects to execute the pipeline on.</param>
        /// <param name="cancellationToken">The token that signals that the build process should be cancelled. This can be accomplished without throwing an exception.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done here.</returns>
        public async Task ExecuteOnCollectionAsync<T>(IEnumerable<T> collection, CancellationToken cancellationToken)
        {
            await this.ExecuteOnCollectionAsync(collection, cancellationToken, null);
        }

        /// <summary>
        /// Asynchronously executes the pipeline on a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of object to execute the pipeline on.</typeparam>
        /// <param name="collection">The collection of objects to execute the pipeline on.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done here.</returns>
        public async Task ExecuteOnCollectionAsync<T>(IEnumerable<T> collection, IProgress<int> progress)
        {
            await this.ExecuteOnCollectionAsync(collection, CancellationToken.None, progress);
        }

        /// <summary>
        /// Asynchronously executes the pipeline on a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of object to execute the pipeline on.</typeparam>
        /// <param name="collection">The collection of objects to execute the pipeline on.</param>
        /// <param name="cancellationToken">The token that signals that the build process should be cancelled. This can be accomplished without throwing an exception.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done here.</returns>
        public async Task ExecuteOnCollectionAsync<T>(IEnumerable<T> collection, CancellationToken cancellationToken, IProgress<int> progress)
        {
            await Task.Yield();
            if (!cancellationToken.IsCancellationRequested)
            {
                float progressIncrement, progressAmount = 0f;
                if (collection is IList<T>)
                {
                    var chunk = collection as IList<T>;
                    progressIncrement = ((float)chunk.Count * (float)this.pipelineSteps.Count) / 1f;
                    for (var index = 0; index < chunk.Count; index++)
                    {
                        progressAmount = await this.WorkerForAsyncExecution(chunk[index], progressIncrement, progressAmount, cancellationToken, progress);
                    }
                }
                else
                {
                    int collectionCount = 1;
                    foreach (var data in collection)
                    {
                        collectionCount++;
                        progressIncrement = ((float)collectionCount * (float)this.pipelineSteps.Count) / 1f;
                        progressAmount = await this.WorkerForAsyncExecution(data, progressIncrement, progressAmount, cancellationToken, progress);
                    }
                }
            }

            progress.Report(100);
        }

        /// <summary>
        /// Executes the pipeline on the data provided.
        /// </summary>
        /// <typeparam name="T">The type of data to execute the pipeline on.</typeparam>
        /// <param name="data">The data to execute the pipeline on.</param>
        public void ExecuteOnData<T>(T data)
        {
            this.PreparePipelineForExecution();
            object piece = data;
            this.ProcessingStage = ProcessingStages.Processing;
            while (!this.ProcessingStage.Equals(ProcessingStages.Completed))
            {
                piece = this.ProcessPiece(piece, this.pipelineStepLocationIndex[this.CurrentLocation]);
            }
        }

        // Work in async execution methods is done in a private worker method.

        /// <summary>
        /// Asynchronously executes the pipeline on the data provided.
        /// </summary>
        /// <typeparam name="T">The type of data to execute the pipeline on.</typeparam>
        /// <param name="data">The data to execute the pipeline on.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done here.</returns>
        public async Task ExecuteOnDataAsync<T>(T data)
        {
            await this.ExecuteOnDataAsync(data, CancellationToken.None, null);
        }

        /// <summary>
        /// Asynchronously executes the pipeline on the data provided.
        /// </summary>
        /// <typeparam name="T">The type of data to execute the pipeline on.</typeparam>
        /// <param name="data">The data to execute the pipeline on.</param>
        /// <param name="cancellationToken">The token that signals that the build process should be cancelled. This can be accomplished without throwing an exception.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done here.</returns>
        public async Task ExecuteOnDataAsync<T>(T data, CancellationToken cancellationToken)
        {
            await this.ExecuteOnDataAsync(data, cancellationToken, null);
        }

        /// <summary>
        /// Asynchronously executes the pipeline on the data provided.
        /// </summary>
        /// <typeparam name="T">The type of data to execute the pipeline on.</typeparam>
        /// <param name="data">The data to execute the pipeline on.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done here.</returns>
        public async Task ExecuteOnDataAsync<T>(T data, IProgress<int> progress)
        {
            await this.ExecuteOnDataAsync(data, CancellationToken.None, progress);
        }
        
        /// <summary>
        /// Asynchronously executes the pipeline on the data provided.
        /// </summary>
        /// <typeparam name="T">The type of data to execute the pipeline on.</typeparam>
        /// <param name="data">The data to execute the pipeline on.</param>
        /// <param name="cancellationToken">The token that signals that the build process should be cancelled. This can be accomplished without throwing an exception.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <returns>A <see cref="Task"/> that embodies the work done here.</returns>
        public async Task ExecuteOnDataAsync<T>(T data, CancellationToken cancellationToken, IProgress<int> progress)
        {
            await Task.Yield();
            await this.WorkerForAsyncExecution(data, (float)this.pipelineSteps.Count / 1f, 0f, cancellationToken, progress);
        }

        /// <summary>
        /// Inserts a configuration schematic into the pipeline.
        /// </summary>
        /// <remarks>The pipeline must be rebuilt, by calling either of the build methods.</remarks>
        /// <param name="scheme">The configuration schematic.</param>
        public void InsertPipelineScheme(PipelineScheme scheme)
        {
            if (scheme.IsNull())
            {
                throw new ArgumentNullException("scheme");
            }
            else
            {
                this.currentScheme = scheme.Clone();
                this.isFinalized = false;
            }
        }

        /// <summary>
        /// Inserts a step into the pipeline. This should be used if desiring to insert static methods.
        /// </summary>
        /// <typeparam name="TIn">The input type of the method.</typeparam>
        /// <typeparam name="TOut">The return type of the method.</typeparam>
        /// <param name="pipelineStep">The method to insert into the pipeline.</param>
        /// <returns>A <see cref="StepInsertionResult"/> that describes the outcome of the process.</returns>
        public StepInsertionResult InsertPipelineStep<TIn, TOut>(Func<TIn, TOut> pipelineStep)
        {
            var typeOfObjectMethodIsFrom = pipelineStep.Target.GetType();
            List<StepMetadata> metas;
            if (this.currentScheme.TryRemove(typeOfObjectMethodIsFrom, out metas))
            {
                // Possible optimization: cache this for shorter repeats.
                // Compile step into pipeline.
                StepWrapper step = null;
                for (var index = 0; index < metas.Count; index++)
                {
                    if (metas[index].DoesMethodMatch(pipelineStep))
                    {
                        step = new StepWrapper<TIn, TOut>() { method = pipelineStep };
                        this.pipelineSteps.Add(metas[index], step);
                        this.currentScheme.TryAdd(typeOfObjectMethodIsFrom, metas);
                        return new StepInsertionResult(metas[index], StepBuildResults.Completed);
                    }
                }

                this.currentScheme.TryAdd(typeOfObjectMethodIsFrom, metas);
            }
            
            return new StepInsertionResult(typeOfObjectMethodIsFrom, pipelineStep.Method, StepBuildResults.ObjectNotRequired);
        }

        /// <summary>
        /// Builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public IList<StepInsertionResult> RebuildPipeline(params object[] objects)
        {
            this.PrepareCache();
            return this.BuildPipeline(objects);
        }

        /// <summary>
        /// Builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="newScheme">An instance of the <see cref="PipelineScheme"/> class that represents the configuration of a pipeline.</param>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public IList<StepInsertionResult> RebuildPipeline(PipelineScheme newScheme, params object[] objects)
        {
            this.InsertPipelineScheme(newScheme);
            return this.RebuildPipeline(objects);
        }

        /// <summary>
        /// Asynchronously builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A <see cref="Task"/> that represents a numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public async Task<IList<StepInsertionResult>> RebuildPipelineAsync(params object[] objects)
        {
            return await this.RebuildPipelineAsync(CancellationToken.None, null, objects);
        }

        /// <summary>
        /// Asynchronously builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that serves notice of cancellation of the build process.</param>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A <see cref="Task"/> that represents a numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public async Task<IList<StepInsertionResult>> RebuildPipelineAsync(CancellationToken cancellationToken, params object[] objects)
        {
            return await this.RebuildPipelineAsync(cancellationToken, null, objects);
        }

        /// <summary>
        /// Asynchronously builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A <see cref="Task"/> that represents a numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public async Task<IList<StepInsertionResult>> RebuildPipelineAsync(IProgress<int> progress, params object[] objects)
        {
            return await this.RebuildPipelineAsync(CancellationToken.None, progress, objects);
        }

        // The following overload does all the work.

        /// <summary>
        /// Asynchronously builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that serves notice of cancellation of the build process.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A <see cref="Task"/> that represents a numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public async Task<IList<StepInsertionResult>> RebuildPipelineAsync(CancellationToken cancellationToken, IProgress<int> progress, params object[] objects)
        {
            this.PrepareCache();
            var results = await this.WorkerForAsyncBuildAndRebuild(true, cancellationToken, progress, objects);

            if (!progress.IsNull())
            {
                progress.Report(100);
            }

            return results;
        }

        /// <summary>
        /// Asynchronously builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="newScheme">An instance of the <see cref="PipelineScheme"/> class that represents the configuration of a pipeline.</param>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A <see cref="Task"/> that represents a numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public async Task<IList<StepInsertionResult>> RebuildPipelineAsync(PipelineScheme newScheme, params object[] objects)
        {
            return await this.RebuildPipelineAsync(newScheme, CancellationToken.None, null, objects);
        }

        /// <summary>
        /// Asynchronously builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="newScheme">An instance of the <see cref="PipelineScheme"/> class that represents the configuration of a pipeline.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that serves notice of cancellation of the build process.</param>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A <see cref="Task"/> that represents a numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public async Task<IList<StepInsertionResult>> RebuildPipelineAsync(PipelineScheme newScheme, CancellationToken cancellationToken, params object[] objects)
        {
            return await this.RebuildPipelineAsync(newScheme, cancellationToken, null, objects);
        }

        /// <summary>
        /// Asynchronously builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="newScheme">An instance of the <see cref="PipelineScheme"/> class that represents the configuration of a pipeline.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A <see cref="Task"/> that represents a numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public async Task<IList<StepInsertionResult>> RebuildPipelineAsync(PipelineScheme newScheme, IProgress<int> progress, params object[] objects)
        {
            return await this.RebuildPipelineAsync(newScheme, CancellationToken.None, progress, objects);
        }

        /// <summary>
        /// Asynchronously builds or rebuilds a pipeline to a new configuration. When rebuilding, if any wrappers are no longer used, it is cached for faster retrieval in the future.
        /// </summary>
        /// <param name="newScheme">An instance of the <see cref="PipelineScheme"/> class that represents the configuration of a pipeline.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that serves notice of cancellation of the build process.</param>
        /// <param name="progress">An instance that implements the <see cref="IProgress"/> interface that the build progress will be reported to.</param>
        /// <param name="objects">A collection of objects that the instance methods will be bound to.</param>
        /// <returns>A <see cref="Task"/> that represents a numerically indexed collection of <see cref="StepInsertionResult"/> objects that detail the outcomes of the building process.</returns>
        public async Task<IList<StepInsertionResult>> RebuildPipelineAsync
            (
                PipelineScheme newScheme,
                CancellationToken cancellationToken,
                IProgress<int> progress,
                params object[] objects
            )
        {
            this.InsertPipelineScheme(newScheme);
            return await this.RebuildPipelineAsync(cancellationToken, progress, objects);
        }
    }
}
