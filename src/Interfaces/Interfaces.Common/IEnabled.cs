namespace Rhyous.WebFramework.Interfaces
{
    public interface IEnabled
    {
        /// <summary>
        /// True means it is enabled or active. False mean it is inactive or disabled.
        /// </summary>
        bool Enabled { get; set; }
    }
}
