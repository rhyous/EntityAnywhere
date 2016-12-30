namespace Rhyous.WebFramework.Interfaces
{
    public interface ICredentialsValidator
    {
        bool IsValid(ICredentials creds, out IToken token);
    }
}