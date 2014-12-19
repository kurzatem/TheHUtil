namespace TheHUtil.Pipeline
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;

    using TheHUtil.Extensions;

    /// <summary>
    /// Defines the <see cref="StepMetadata"/> class.
    /// </summary>
    /// <remarks>This is meant to define what method is to be called when being used by a <see cref="PipelineManager"/>.</remarks>
    public class StepMetadata : ISerializable, IEquatable<StepMetadata>
    {
        /// <summary>
        /// Defines the constant string for the field name of the type that the method is contained in.
        /// </summary>
        private const string string_field_name_for_type = "Type";

        /// <summary>
        /// Defines the constant string for the field name of the name of the method to be used in the pipeline.
        /// </summary>
        private const string string_field_name_for_method_name = "Method";

        /// <summary>
        /// Defines the constant string for the field name of the input type of a pipeline.
        /// </summary>
        private const string string_field_name_for_input_type = "Input type name";

        /// <summary>
        /// Defines the constant string for the field name of the output type of a pipeline.
        /// </summary>
        private const string string_field_name_for_output_type = "Output type name";

        /// <summary>
        /// Defines the constant string for the field name of the location of the step within a pipeline.
        /// </summary>
        private const string string_field_name_for_location = "Location";

        /// <summary>
        /// The name of the method to be called.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// The type of object that contains the method.
        /// </summary>
        public Type TypeContainingMethod { get; set; }

        /// <summary>
        /// The type of object that is passed into the method when called.
        /// </summary>
        public Type InputType { get; set; }

        /// <summary>
        /// The type of object that is returned from the method.
        /// </summary>
        public Type OutputType { get; set; }

        /// <summary>
        /// The location, in the form of a <see cref="StepLocation"/> instance, that the method is to be placed. The only concerns are that they are unique, and usable by a class derived from the <see cref="PipelineManager"/>.
        /// </summary>
        public StepLocation Location { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepMetadata"/> class.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="typeName">The type that contains the method.</param>
        /// <param name="inputType">The input type for the method.</param>
        /// <param name="outputType">The output type of the method.</param>
        /// <param name="location">The location in a pipeline that the method is to be located at.</param>
        internal StepMetadata(string methodName, Type typeName, Type inputType, Type outputType, string location)
        {
            this.MethodName = methodName;
            this.TypeContainingMethod = typeName;
            this.InputType = inputType;
            this.OutputType = outputType;
            if (location.IsNullOrEmptyOrWhiteSpace())
            {
                this.Location = new StepLocation((short)-1, (short)-1);
            }
            else
            {
                this.Location = StepLocation.Parse(location);
            }
        }

        /// <summary>
        /// Parses a string that uses named fields into a <see cref="StepMetadata"/> instance.
        /// </summary>
        /// <param name="SplitRawData">The input string that has been split at the semi-colons.</param>
        /// <returns>A new instance of the <see cref="StepMetadata"/> class.</returns>
        private static StepMetadata ParseWithNamedFields(string[] SplitRawData)
        {
            Type typeName = null, inputType = null, outputType = null;
            string methodName = null, location = null;
            for (var index = 0; index < SplitRawData.Length; index++)
            {
                var locationOfColon = SplitRawData[index].IndexOf(':');
                if (locationOfColon == -1)
                {
                    if (!SplitRawData[index].Equals(" ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new ArgumentException("Cannot parse string due to the inconsistent presence of colons.");
                    }
                }
                else
                {
                    // Determine what the name of the field is and assign the appropriate variable.
                    var fieldParts = SplitRawData[index].Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fieldParts[0].RemoveFirst(' ').Equals(string_field_name_for_type, StringComparison.InvariantCultureIgnoreCase))
                    { typeName = Type.GetType(fieldParts[1].RemoveFirst(' '), true, true); }
                    else if (fieldParts[0].RemoveFirst(' ').Equals(string_field_name_for_method_name, StringComparison.InvariantCultureIgnoreCase))
                    { methodName = fieldParts[1].RemoveFirst(' '); }
                    else if (fieldParts[0].RemoveFirst(' ').Equals(string_field_name_for_input_type, StringComparison.InvariantCultureIgnoreCase))
                    { inputType = Type.GetType(fieldParts[1].RemoveFirst(' '), true, true); }
                    else if (fieldParts[0].RemoveFirst(' ').Equals(string_field_name_for_output_type, StringComparison.InvariantCultureIgnoreCase))
                    { outputType = Type.GetType(fieldParts[1].RemoveFirst(' '), true, true); }
                    else if (fieldParts[0].RemoveFirst(' ').Equals(string_field_name_for_location, StringComparison.InvariantCultureIgnoreCase))
                    { location = fieldParts[1].RemoveFirst(' '); };
                }
            }

            return new StepMetadata(methodName, typeName, inputType, outputType, location);
        }

        /// <summary>
        /// Parses a string that does not use named fields into a <see cref="StepMetadata"/> instance.
        /// </summary>
        /// <param name="SplitRawData">The input string that has been split at the semi-colons.</param>
        /// <returns>A new instance of the <see cref="StepMetadata"/> class.</returns>
        private static StepMetadata ParseWithoutNamedFields(string[] SplitRawData)
        {
            var inputType = Type.GetType(SplitRawData[2], true, true);
            var typeName = Type.GetType(SplitRawData[0].RemoveAll(' '), true, true);
            var methodName = SplitRawData[1].RemoveAll(' ');
            Type outputType = null;
            string location = null;
            if (SplitRawData.Length > 4)
            {
                outputType = Type.GetType(SplitRawData[3], true, true);
                location = SplitRawData[4];
            }
            else
            {
                location = SplitRawData[3];
            }

            return new StepMetadata(methodName, typeName, inputType, outputType, location);
        }

        /// <summary>
        /// Parses a string into a <see cref="StepMetadata"/> instance.
        /// </summary>
        /// <param name="lineOfData">The data to parse.</param>
        /// <returns>A new instance of the <see cref="StepMetadata"/> class containing the data outline in the input string.</returns>
        public static StepMetadata Parse(string lineOfData)
        {
            var hasNamedFields = lineOfData.Contains(':');
            var SplitRawData = lineOfData.Split(new[] { '{', ';', '}' }, StringSplitOptions.RemoveEmptyEntries);
            if (hasNamedFields)
            {
                return ParseWithNamedFields(SplitRawData);
            }
            else
            {
                return ParseWithoutNamedFields(SplitRawData);
            }
        }

        /// <summary>
        /// Attempts to parse a string into a <see cref="StepMetadata"/> instance.
        /// </summary>
        /// <param name="lineOfData">The string to attempt to parse.</param>
        /// <param name="output">If successful, a <see cref="StepMetadata"/> instance, otherwise, null.</param>
        /// <returns>True only if parsing was a success.</returns>
        public static bool TryParse(string lineOfData, out StepMetadata output)
        {
            try
            {
                output = StepMetadata.Parse(lineOfData);
                return true;
            }
            catch
            {
                output = null;
                return false;
            }
        }

        /// <summary>
        /// Determines if 2 objects have equivalent data.
        /// </summary>
        /// <param name="obj">The other instance to compare with.</param>
        /// <returns>True only if the data in both instances is equivalent.</returns>
        public override bool Equals(object obj)
        {
            if (!obj.IsNull() || obj is StepMetadata)
            {
                return this.Equals(obj as StepMetadata);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the hash code of the location of the metadata.
        /// </summary>
        /// <returns>The hash code of the location of the metadata.</returns>
        public override int GetHashCode()
        {
            return this.Location.GetHashCode();
        }

        /// <summary>
        /// Dumps the data into a single string. This can serve as an example of how the data can be stored in a file.
        /// </summary>
        /// <returns>The internal data in a single string.</returns>
        public override string ToString()
        {
            var formatStringForData = "{0}: {1}; ";
            var beginning = string.Format
                (
                    "{{{0}: {1}; {2}: {3}; ",
                    string_field_name_for_type,
                    this.TypeContainingMethod.FullName,
                    string_field_name_for_method_name,
                    this.MethodName
                );
            var inputName = this.InputType.IsNull() ?
                string.Empty :
                string.Format(formatStringForData, string_field_name_for_input_type, this.InputType.AssemblyQualifiedName);
            var outputName = this.OutputType.IsNull() ?
                string.Empty :
                string.Format(formatStringForData, string_field_name_for_output_type, this.OutputType.AssemblyQualifiedName);
            var ending = string.Format("{0}: {1};", string_field_name_for_location, this.Location.ToString());

            return string.Concat(beginning, inputName, outputName, ending);
        }

        /// <summary>
        /// Determines if a given method matches the signature defined in this instance.
        /// </summary>
        /// <typeparam name="TIn">The type of input parameter the method accepts.</typeparam>
        /// <typeparam name="TOut">The output type of the method.</typeparam>
        /// <param name="method">The method, in delegate form, that is being tested.</param>
        /// <returns>True if all metadata for the method matches.</returns>
        public bool DoesMethodMatch<TIn, TOut>(Func<TIn, TOut> method)
        {
            if (!method.Method.DeclaringType.Equals(this.TypeContainingMethod)) { return false; }

            if (!method.Method.Name.Equals(this.MethodName, StringComparison.InvariantCulture)) { return false; }

            return this.InputType.Equals(typeof(TIn)) && this.OutputType.Equals(method.Method.ReturnType);
        }

        /// <summary>
        /// Determines if the values within 2 instances are equivalent.
        /// </summary>
        /// <param name="other">Another instance of the <see cref="StepMetadata"/> class to compare values to.</param>
        /// <returns>True only if all values are equivalent.</returns>
        public bool Equals(StepMetadata other)
        {
            if (!this.TypeContainingMethod.Equals(other.TypeContainingMethod)) { return false; }
            if (!this.MethodName.Equals(other.MethodName, StringComparison.InvariantCulture)) { return false; }
            if (!this.InputType.Equals(other.InputType)) { return false; }
            if (!this.OutputType.Equals(other.OutputType)) { return false; }
            if (!this.Location.Equals(other.Location)) { return false; }

            return true;
        }

        /// <summary>
        /// Determines how the data is to appear when serialized.
        /// </summary>
        /// <param name="info">An instance of the <see cref="SerializationInfo"/> class that contains the serialization information.</param>
        /// <param name="context">An instance of the <see cref="StreamingContext"/> struct that helps in determining how the data is stored.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State == StreamingContextStates.Persistence || context.State == StreamingContextStates.File)
            {
                info.AddValue("Type Name", this.TypeContainingMethod.AssemblyQualifiedName);
                info.AddValue("Method Name", this.MethodName);
                if (!this.InputType.IsNull())
                {
                    info.AddValue("Input Type Name", this.InputType.AssemblyQualifiedName);
                }

                if (!this.OutputType.IsNull())
                {
                    info.AddValue("Output Type Name", this.OutputType.AssemblyQualifiedName);
                }

                info.AddValue("Location", this.Location.ToString());
            }
        }
    }
}
