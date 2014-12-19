namespace TheHUtil.Pipeline
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="StepWrapper"/> class.
    /// </summary>
    /// <remarks>A "step" is not the same as a delegate or method. Although the implementation is using a delegate, the concept of a step is that it is a method or delegate that has a role in processing data via a pipeline.</remarks>
    internal abstract class StepWrapper
    {
        /// <summary>
        /// Gets or sets the delegate that is part of a pipeline.
        /// </summary>
        internal Delegate method { get; set; }

        /// <summary>
        /// Creates an instance of the <see cref="StepWrapper"/> class.
        /// </summary>
        /// <typeparam name="T">The type of object that is provided.</typeparam>
        /// <param name="obj">The object that the provided <see cref="MethodInfo"/> points to.</param>
        /// <param name="method">The method, as embodied in a <see cref="MethodInfo"/> instance, to create the delegate from.</param>
        /// <returns>An instance of the <see cref="StepWrapper"/> class to be used in a pipeline.</returns>
        public static StepWrapper CreateWrapper<T>(T obj, MethodInfo method)
        {
            var args = method.GetParameters();
            var expParams = args.Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
            var objAsExpression = Expression.Constant(obj);
            var expression = Expression.Call(objAsExpression, method, expParams);
            var func = Expression.Lambda(expression, expParams).Compile();
            Type[] genericTypeParams;
            if (method.ReturnType.Equals(typeof(void)))
            {
                genericTypeParams = new[] { args[0].ParameterType };
            }
            else
            {
                genericTypeParams = new[] { args[0].ParameterType, method.ReturnType };
            }

            var result = Activator.CreateInstance(typeof(StepWrapper<,>).MakeGenericType(genericTypeParams)) as StepWrapper;
            result.method = func;
            return result;
        }

        /// <summary>
        /// Executes the internally stored method on the object passed in.
        /// </summary>
        /// <param name="input">The data to process using the internally stored delegate.</param>
        /// <returns>The output of the internally stored delegate.</returns>
        public abstract object Execute(object input);
    }
}
