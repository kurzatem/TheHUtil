namespace TheHUtil.Testing
{
    using System;

    /// <summary>
    /// Defines the <see cref="TestGeneratorParameterCasesAttribute"/> attribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class TestGeneratorParameterCasesAttribute : TestGeneratorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestGeneratorParameterCasesAttribute"/> attribute class.
        /// </summary>
        /// <param name="cases">The cases in which to generate test code.</param>
        public TestGeneratorParameterCasesAttribute(params object[] cases) :
            base(cases)
        {
        }
    }
}
