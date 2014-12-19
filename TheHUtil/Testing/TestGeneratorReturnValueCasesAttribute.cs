namespace TheHUtil.Testing
{
    using System;

    /// <summary>
    /// Defines the <see cref="TestGeneratorReturnValueCasesAttribute"/> attribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.ReturnValue)]
    public class TestGeneratorReturnValueCasesAttribute : TestGeneratorAttribute
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="TestGeneratorReturnValueCasesAttribute"/> generic attribute class. This is for creating return value ranges.
        /// </summary>
        /// <remarks>Ideally, this should be used with primitives like the 32-bit integer or character.</remarks>
        /// <param name="lowEnd">The low end of the test case range.</param>
        /// <param name="highEnd">The high end of the test case range.</param>
        public TestGeneratorReturnValueCasesAttribute(object lowEnd, object highEnd) :
            base(new[] { lowEnd, highEnd })
        {
        }
    }
}
