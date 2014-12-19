namespace TheHUtil.Pipeline
{
    /// <summary>
    /// Defines the <see cref="BuildState"/> class.
    /// </summary>
    /// <remarks>This is used primarily in allowing the <see cref="PipelineManager"/> to perform it's work using the Task.Run asynchronous initialization.</remarks>
    internal class BuildState : State
    {
        /// <summary>
        /// The collection of objects that the pipeline manager uses to extract the methods from.
        /// </summary>
        public object[] BuildObjects { get; set; }
    }
}
