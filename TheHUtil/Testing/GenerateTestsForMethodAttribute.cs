namespace TheHUtil.Testing
{
    using System;
    using System.Collections.Generic;
    
    [AttributeUsage(AttributeTargets.Method)]
    public class GenerateTestsForMethodAttribute : Attribute
    {
        public Dictionary<string, object[]> ParameterNamesAndCases { get; private set; }

        public object[] ReturnValue { get; private set; }

        public bool ReturnValuesAreAcceptable { get; private set; }

        public GenerateTestsForMethodAttribute(IDictionary<string, object[]> parameterNamesAndCases, object[] returnValues, bool returnValuesAreAcceptable = true)
        {
            this.ParameterNamesAndCases = new Dictionary<string,object[]>(parameterNamesAndCases);
            this.ReturnValue = returnValues;
            this.ReturnValuesAreAcceptable = returnValuesAreAcceptable;
        }
    }
}
