namespace Rhyous.WebFramework.Interfaces
{
    public interface ITokenBuilder
    {
        IToken Build(ICredentials creds, int userId);
    }
}
