namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// Interface for turning one type to another
    /// </summary>
    /// <typeparam name="A">The type to be converted from</typeparam>
    /// <typeparam name="B">The type to be converted to</typeparam>
    public interface IConvert<A, B>
    {
        /// <summary>
        /// Convert an <see cref="A"/> to a <see cref="B"/>
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        B Convert(A from);
    }
}
