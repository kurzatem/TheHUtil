namespace TheHUtil.Pipeline
{
    using System;

    /// <summary>
    /// Defines the <see cref="PipelineProgress"/> class. This class implements the <see cref="IProgress"/> generic interface using the type parameter of <see cref="Int32"/>.
    /// </summary>
    /// <remarks>This is for reporting the progress of the pipeline. It is not necessary to use this specific class in the asynchronous methods, but it provides functionality that allows the pipeline to be cancelled at a specific point of completion.</remarks>
    public class PipelineProgress : IProgress<int>
    {
        /// <summary>
        /// The progress at which the provided delegate is to be called.
        /// </summary>
        private int threshold;

        /// <summary>
        /// The optional parameter for the delegate when it is called.
        /// </summary>
        private bool? parameter;

        /// <summary>
        /// A <see cref="Delegate"/> instance that is to be an <see cref="Action"/> delegate that may or may not accept a <see cref="Boolean"/> parameter.
        /// </summary>
        private Delegate onThresholdExceeded;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineProgress"/> class.
        /// </summary>
        /// <param name="threshold">The progress percentage at which the delegate is to be called.</param>
        /// <param name="toDo">The action to take when percentage of completion has surpassed the threshold.</param>
        private PipelineProgress(int threshold, Delegate toDo)
        {
            this.threshold = threshold;
            this.onThresholdExceeded = toDo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineProgress"/> class.
        /// </summary>
        /// <param name="toDoWhenThresholdExceeded">The parameterless action to take when percentage of completion has surpassed the threshold.</param>
        /// <param name="threshold">The progress percentage at which the delegate is to be called.</param>
        public PipelineProgress(Action toDoWhenThresholdExceeded, int threshold) :
            this(threshold, toDoWhenThresholdExceeded)
        {
            this.parameter = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineProgress"/> class.
        /// </summary>
        /// <param name="toDoWhenThresholdExceeded">The parameterless action to take when percentage of completion has surpassed the threshold.</param>
        /// <param name="threshold">The progress percentage at which the delegate is to be called.</param>
        /// <param name="parameterValue">The value that should be passed into the action when it is called.</param>
        public PipelineProgress(Action<bool> toDoWhenThresholdExceeded, int threshold, bool parameterValue) :
            this(threshold, toDoWhenThresholdExceeded)
        {
            this.parameter = parameterValue;
        }
        
        /// <summary>
        /// Reports a progress update.
        /// </summary>
        /// <param name="value">The value to report.</param>
        public void Report(int value)
        {
            if (this.threshold <= value)
            {
                if (this.parameter.HasValue)
                {
                    var operation = (Action<bool>)this.onThresholdExceeded;
                    operation(this.parameter.Value);
                }
                else
                {
                    var operation = (Action)this.onThresholdExceeded;
                    operation();
                }
            }
        }
    }
}
