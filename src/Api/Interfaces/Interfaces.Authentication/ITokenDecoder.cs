namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface ITokenDecoder 
    {
        IToken Decode(string tokenText);
    }
}
