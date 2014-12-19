namespace TheHUtil.Pipeline
{
    using System;

    using TheHUtil.Extensions;

    /// <summary>
    /// Defines the <see cref="StepLocation"/> struct.
    /// </summary>
    public struct StepLocation : IEquatable<StepLocation>
    {
        /// <summary>
        /// Defines an invalid location in a pipeline. Valid locations are where both the tier and branch values are not negative.
        /// </summary>
        public static readonly StepLocation InvalidLocation = new StepLocation(-1, -1);

        /// <summary>
        /// The number of steps from the beginning at which a step is located.
        /// </summary>
        internal readonly short Tier;

        /// <summary>
        /// The branch at which a step is located. For visualization purposes, branches are perpendicular to tiers.
        /// </summary>
        internal readonly short Branch;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepLocation"/> struct.
        /// </summary>
        /// <param name="tier">The tier to place the pipeline step.</param>
        /// <param name="branch">The branch to place the pipeline step.</param>
        internal StepLocation(short branch, short tier)
        {
            this.Tier = tier;
            this.Branch = branch;
        }
        
        /// <summary>
        /// Parses a string to a <see cref="StepLocation"/> value.
        /// </summary>
        /// <param name="input">A string in the format: (#1,#2) where "#1" is the branch and "#2" is the tier.</param>
        /// <returns>A <see cref="StepLocation"/> instance containing the location for a pipeline step.</returns>
        public static StepLocation Parse(string input)
        {
            var splitInput = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var tier = splitInput[1].ParseToNumeric<short>();
            var branch = splitInput[0].ParseToNumeric<short>();
            return new StepLocation(branch, tier);
        }

        /// <summary>
        /// Indicates whether this instance and another specified object have equal values.
        /// </summary>
        /// <param name="obj">Another object to compare this instance with.</param>
        /// <returns>True: has equal values. False: not equal values.</returns>
        public override bool Equals(object obj)
        {
            if (obj.IsNull() || !(obj is StepLocation))
            {
                return false;
            }
            else
            {
                return this.Equals((StepLocation)obj);
            }
        }

        /// <summary>
        /// Gets an integer that represents this instance.
        /// </summary>
        /// <returns>A hashing integer.</returns>
        public override int GetHashCode()
        {
            return ((int)this.Tier * short.MaxValue) + this.Branch;
        }

        /// <summary>
        /// Writes the location of a step using this format: "({Tier},{Branch})" where the names in curly braces are the values of the instance.
        /// </summary>
        /// <returns>A formatted string with the location data.</returns>
        public override string ToString()
        {
            if (this.Branch >= 0 && this.Tier >= 0)
            {
                return string.Format("({0},{1})", this.Branch, this.Tier);
            }
            else
            {
                return "Invalid location";
            }
        }

        /// <summary>
        /// Indicates whether this instance and another specific location have equal values.
        /// </summary>
        /// <param name="other">Another location to compare the values to.</param>
        /// <returns>True: values are equal. False: values are not equal.</returns>
        public bool Equals(StepLocation other)
        {
            return this.Branch == other.Branch && this.Tier == other.Tier;
        }
    }
}
