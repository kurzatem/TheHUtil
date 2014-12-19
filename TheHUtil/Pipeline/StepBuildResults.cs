namespace TheHUtil.Pipeline
{
    /// <summary>
    /// Defines the <see cref="StepBuildResults"/> enum.
    /// </summary>
    public enum StepBuildResults
    {
        /// <summary>
        /// Step building was successfully completed.
        /// </summary>
        Completed,

        /// <summary>
        /// Pipeline scheme requires an object not provided during the build process.
        /// </summary>
        MissingObject,

        /// <summary>
        /// Object provided does not contain any methods utilizing the <see cref="PipelineMethodAttribute"/> attribute.
        /// </summary>
        ObjectMissingMethodsWithAttribute,

        /// <summary>
        /// Method provided does not fully match metadata provided in scheme.
        /// </summary>
        MethodAndMetadataPartialMismatch,

        /// <summary>
        /// Pipeline scheme requires a method that is not found in the provided object.
        /// </summary>
        MethodNotFound,

        /// <summary>
        /// Object provided is not required in scheme.
        /// </summary>
        ObjectNotRequired,
            
        /// <summary>
        /// Pipeline building operation was cancelled.
        /// </summary>
        Cancelled,

    }
}
