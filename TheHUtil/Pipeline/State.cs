namespace TheHUtil.Pipeline
{
    using System;
    using System.Threading;
    
    /// <summary>
    /// Defines the <see cref="State"/> abstract class.
    /// </summary>
    internal abstract class State
    {
        /// <summary>
        /// The cancellation token that causes the pipeline to cancel it's current asynchornous operation.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// The progress reporter that reports the pipeline manager's progress.
        /// </summary>
        public IProgress<int> ProgressReporter { get; set; }
    }
}
