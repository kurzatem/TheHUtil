namespace TheHUtil.Pipeline
{
    using System;
    using System.Collections.Generic;

    using TheHUtil.Extensions;

    /// <summary>
    /// Defines the <see cref="StepInsertionResult"/> class.
    /// </summary>
    [Serializable]
    public class StepInsertionResult
    {
        /// <summary>
        /// Stores all the values in the <see cref="StepBuildResults"/> enum and the names as strings. This is for performance reasons.
        /// </summary>
        private static IReadOnlyDictionary<StepBuildResults, string> BuildResultNames = EnumConverters.ConvertToDictionary<StepBuildResults>();

        /// <summary>
        /// Gets an instance of the <see cref="StepMetadata"/> class for this instance.
        /// </summary>
        public StepMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the outcome of the insertion procedure.
        /// </summary>
        public StepBuildResults Outcome { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepInsertionResult"/> class.
        /// </summary>
        /// <param name="type">The type that the result pertains to.</param>
        /// <param name="result">The result of the insertion process as defined by the <see cref="StepBuildResults"/> enum.</param>
        internal StepInsertionResult(Type type, StepBuildResults result)
        {
            this.Metadata = new StepMetadata(null, type, null, null, null);
            this.Outcome = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepInsertionResult"/> class.
        /// </summary>
        /// <param name="metadata">The instance of the <see cref="StepMetadata"/> class that this insertion result pertains to.</param>
        /// <param name="result">The result of the insertion process as defined by the <see cref="StepBuildResults"/> enum.</param>
        internal StepInsertionResult(StepMetadata metadata, StepBuildResults result)
        {
            this.Metadata = metadata;
            this.Outcome = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepInsertionResult"/> class.
        /// </summary>
        /// <param name="type">The type of object used to build the metadata from.</param>
        /// <param name="method">The <see cref="MethodInfo"/> that represents a method in a class or struct.</param>
        /// <param name="result">The result of the insertion process as defined by the <see cref="StepBuildResults"/> enum.</param>
        internal StepInsertionResult(Type type, System.Reflection.MethodInfo method, StepBuildResults result)
        {
            this.Metadata = new StepMetadata(method.Name, type, null, null, null);
            this.Outcome = result;
        }

        /// <summary>
        /// Returns the internal values in a formatted string.
        /// </summary>
        /// <returns>The internal values as a string.</returns>
        public override string ToString()
        {
            return string.Format("Metadata: {0} Outcome: {1}", this.Metadata.ToString(), BuildResultNames[this.Outcome]);
        }
    }
}
