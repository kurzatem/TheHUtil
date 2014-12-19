namespace TheHUtil.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    /// <summary>
    /// Defines the <see cref="TestGeneratorAttribute"/> attribute class.
    /// </summary>
    public abstract class TestGeneratorAttribute : Attribute
    {
        /// <summary>
        /// Gets the cases or range limits to generate test code for.
        /// </summary>
        public object[] Cases { get; private set; }
                
        /// <summary>
        /// Initializes a new instance of the <see cref="TestGeneratorAttribute"/> generic attribute class.
        /// </summary>
        /// <param name="cases">All of the cases that you wish to test for.</param>
        internal TestGeneratorAttribute(object[] cases)
        {
            this.Cases = cases;
        }
    }
}
