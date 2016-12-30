namespace Rhyous.WebFramework.Interfaces
{
    public interface ITokenBuilder
    {
        IToken Build(ICredentials creds, long userId);
    }
}
