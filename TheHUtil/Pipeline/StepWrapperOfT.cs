namespace TheHUtil.Pipeline
{
    using System;

    /// <summary>
    /// Defines the <see cref="StepWrapper"/> generic class.
    /// </summary>
    /// <typeparam name="TIn">The input type of the internally stored delegate.</typeparam>
    /// <typeparam name="TOut">The output type of the internally stored delegate.</typeparam>
    internal class StepWrapper<TIn, TOut> : StepWrapper
    {
        /// <summary>
        /// Executes the internally stored method on the object passed in.
        /// </summary>
        /// <param name="input">The data to process using the internally stored delegate.</param>
        /// <returns>The output of the internally stored delegate.</returns>
        public override object Execute(object input)
        {
            if (!(input is TIn))
            {
                throw new ArgumentException("Input parameter's type is incorrect. Expected type is: " + typeof(TIn).FullName + ". Actual type is: " + input.GetType().FullName);
            }

            var methodAsFunc = base.method as Func<TIn, TOut>;
            return methodAsFunc((TIn)input);
        }

        /// <summary>
        /// Prints the contents of this instance to a formatted string. For use primarily as a debugging tool.
        /// </summary>
        /// <returns>A formatted string that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("StepWrapper<{0}, {1}>", typeof(TIn).FullName, typeof(TOut).FullName); 
        }
    }
}
