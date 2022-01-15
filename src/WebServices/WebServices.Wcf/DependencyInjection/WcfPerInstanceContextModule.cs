using Autofac;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class WcfPerInstanceContextModule : Module
    {
        private readonly IOperationContext _OperationContext;
        private readonly IWebOperationContext _WebOperationContext;

        /// <summary>
        /// The public constructor for this module.
        /// </summary>
        public WcfPerInstanceContextModule()
        {
            _OperationContext = OperationContext.Current is null ? null : new OperationContextWrapper(OperationContext.Current);
            _WebOperationContext = OperationContext.Current is null ? null : new WebOperationContextWrapper(WebOperationContext.Current);
        }

        /// <summary>
        /// This is only for use with Unit tests, so we can mock these.
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="webOperationContext"></param>
        internal WcfPerInstanceContextModule(IOperationContext operationContext = null,
                                             IWebOperationContext webOperationContext = null)
        {
            _OperationContext = operationContext;
            _WebOperationContext = webOperationContext;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_OperationContext != null)
            {
                builder.RegisterInstance(_OperationContext).As<IOperationContext>().SingleInstance();

                var requestedUri = new RequestUri { Uri = _OperationContext.RequestContext?.RequestMessage?.Headers.To };
                builder.RegisterInstance(requestedUri).As<IRequestUri>().SingleInstance();

                var remoteEndpointMessageProperty = (_OperationContext.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty);
                var requestSourceIpAddress = new RequestSourceIpAddress { IpAddress = remoteEndpointMessageProperty?.Address };
                builder.RegisterInstance(requestSourceIpAddress).As<IRequestSourceIpAddress>().SingleInstance();
            }

            if (_WebOperationContext != null)
            {
                builder.RegisterInstance(_WebOperationContext).As<IWebOperationContext>().SingleInstance();
                builder.RegisterInstance(_WebOperationContext.OutgoingResponse).As<IOutgoingWebResponseContext>().SingleInstance();

                var headers = new Headers { Collection = _WebOperationContext.IncomingRequest?.Headers };
                builder.RegisterInstance(headers).As<IHeaders>().SingleInstance();
                builder.RegisterType<ClaimsProvider>().As<IClaimsProvider>().SingleInstance();

                // If there is no token, register the user as Anonymous
                if (string.IsNullOrWhiteSpace(headers.Collection.Get("Token", ""))
                    && string.IsNullOrWhiteSpace(headers.Collection.Get("EntityAdminToken", ""))
                    && string.IsNullOrWhiteSpace(headers.Collection.Get("Bearer", "")))
                    builder.RegisterType<AnonymousUserDetails>().As<IUserDetails>().SingleInstance();
                else
                    builder.RegisterType<UserDetails>().As<IUserDetails>().SingleInstance();

                var urlParameters = new UrlParameters { Collection = _WebOperationContext.IncomingRequest?.UriTemplateMatch?.QueryParameters };
                builder.RegisterInstance(urlParameters).As<IUrlParameters>().SingleInstance();
            }
        }
    }
}