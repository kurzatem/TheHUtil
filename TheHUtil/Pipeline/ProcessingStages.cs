namespace TheHUtil.Pipeline
{
    /// <summary>
    /// Defines a set of processing stages that are used internally to determine the appropriate action for data as it moves through the pipeline.
    /// </summary>
    public enum ProcessingStages
    {
        /// <summary>
        /// The processing has not begun.
        /// </summary>
        /// <remarks>This means that the step located at (0,0) is to be used.</remarks>
        Unstarted,

        /// <summary>
        /// The processing is underway.
        /// </summary>
        /// <remarks>This means that the next step should be determined by the method used for determining the next step. This particular detail will be worked out better in future versions.</remarks>
        Processing,

        /// <summary>
        /// The processing has been completed.
        /// </summary>
        /// <remarks>This means that the data is to be placed in the output queue.</remarks>
        Completed,

        /// <summary>
        /// The processing has been halted with the intention of being resumed.
        /// </summary>
        /// <remarks>This means that the data should be stored until the processing resumes. Upon resuming, the pipeline should discover what the next step is to be.</remarks>
        Paused,

        /// <summary>
        /// The processing has been cancelled.
        /// </summary>
        /// <remarks>No effort should be made to preserve data that is in processing or the current step location. All pieces of data that have been placed in the output queue are to remain untouched.</remarks>
        Cancelled
    }
}
