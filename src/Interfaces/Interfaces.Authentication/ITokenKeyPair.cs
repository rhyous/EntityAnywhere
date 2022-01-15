namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface ITokenKeyPair
    {
        string PrivateKey { get; }
        string PublicKey { get; }
    }
}