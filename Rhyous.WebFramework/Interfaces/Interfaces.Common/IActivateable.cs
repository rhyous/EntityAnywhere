namespace Rhyous.WebFramework.Interfaces
{
    public interface IActivateable
    {
        /// <summary>
        /// True means it is active or enabled. False mean it is inactive or disabled.
        /// </summary>
        bool Active { get; set; }
    }
}
