namespace TheHUtil.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using TheHUtil.Extensions;

    public class CodeGenerator
    {
        private static Type customAttributeType = typeof(TestGeneratorAttribute);

        // should use the KingISS parser/writer for this one.

        // NOTE: when getting the values from an attribute for a parameter of a method, remember to check if the cases are an array.
        // This is due to the possibility of miswritten constructor code by the consumer.

        internal static string CreateTestMethodCodeFor
            (
                MethodInfo method,
                object testValue,
                object expectedValue
            )
        {
            const string declaration_for_expected_value = "var expected = ";
            const string declaration_for_actual_value = "var actual = ";
            const string format_for_getting_actual_value = "testee.{0}({1})";
            const string format_for_assertion = "Assert.IsTrue(expected.{0}(actual))";
            const string end_of_line_of_code = ";";

            var result = new StringBuilder(10);
            result.Append(declaration_for_expected_value);
            result.Append(expectedValue.ToString());
            result.AppendLine(end_of_line_of_code);
            result.AppendLine();

            result.Append(declaration_for_actual_value);
            result.AppendFormat(format_for_getting_actual_value, method.Name, testValue.IsNull() ? string.Empty : testValue.ToString());
            result.AppendLine(end_of_line_of_code);
            result.AppendLine();

            var equalityMethodName = expectedValue is System.Collections.IEnumerable ? "SequenceEqual" : "Equals";
            result.AppendFormat(format_for_assertion, equalityMethodName);
            result.Append(end_of_line_of_code);
            
            return result.ToString();
        }

        internal static IEnumerable<string> CreateTestMethods(MethodInfo method)
        {
            var parameterInfos = method.GetParameters();
            var parametersWithAttribute = parameterInfos.Where(p => !p.GetCustomAttribute(customAttributeType, false).IsNull());
            var returnTypeAttribute = method.ReturnParameter.GetCustomAttribute(customAttributeType);

            // need to iterate through all of the parameters that have the attribute and write the code for those.

            throw new NotImplementedException();
        }
        
        internal static IEnumerable<TestGeneratorAttribute> GetTestGeneratorAttributesForMethod(MethodInfo method)
        {
            foreach (var parameter in method.GetParameters())
            {
                yield return parameter.GetCustomAttribute(customAttributeType, true) as TestGeneratorAttribute;
            }
        }

        public static Action CreateTestExpressionsFor<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public static Func<Task> CreateAsyncTestExpressionFor<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public static string CreateTestClassFor<T>(T obj)
        {
            throw new NotImplementedException();
            // For async method test gen, look for AsyncStateMachineAttribute in the custom atributes
        }
    }
}
