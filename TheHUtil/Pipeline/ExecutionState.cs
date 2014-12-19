namespace TheHUtil.Pipeline
{
    /// <summary>
    /// Defines the <see cref="ExecutionState"/> generic class.
    /// </summary>
    /// <typeparam name="T">The type of objects being processed by the pipeline.</typeparam>
    internal class ExecutionState<T> : State
    {
        /// <summary>
        /// The collection of objects that are/were being processed.
        /// </summary>
        public T[] Collection { get; set; }
    }
}
