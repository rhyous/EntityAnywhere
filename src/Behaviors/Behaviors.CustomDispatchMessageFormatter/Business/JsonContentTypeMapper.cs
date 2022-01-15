using System.ServiceModel.Channels;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class JsonContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw;
        }
    }
}
