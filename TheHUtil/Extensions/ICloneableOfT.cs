namespace TheHUtil.Extensions
{
    /// <summary>
    /// Defines the <see cref="IClonable"/> generic interface.
    /// </summary>
    /// <typeparam name="T">The type of object that will be cloned.</typeparam>
    public interface ICloneable<T>
    {
        /// <summary>
        /// Defines that an object will clone itself into another instance of the same type with equivalent data.
        /// </summary>
        /// <returns>A new instance that has equivalent data.</returns>
        T Clone();
    }
}
