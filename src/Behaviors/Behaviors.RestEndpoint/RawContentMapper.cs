using System.ServiceModel.Channels;

namespace Rhyous.WebFramework.Behaviors
{
    class RawContentMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw;
        }
    }
}
