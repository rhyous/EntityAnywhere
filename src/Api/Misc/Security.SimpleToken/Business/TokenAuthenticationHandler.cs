using System;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Services;
using Rhyous.StringLibrary;
using Rhyous.Wrappers;

namespace Rhyous.EntityAnywhere.Security
{
    /// <summary>A handler for Token Authentication</summary>
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationSchemeOptions>
    {
        private const string Token = "Token";
        private readonly IAccessController _AccessController;
        private readonly IPluginHeaderValidator _PluginHeaderValidator;
        private readonly IAuthenticationTicketBuilder _AuthenticationTicketBuilder;

        /// <summary>The constructor</summary>
        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationSchemeOptions> options,
                                          ILoggerFactory loggerFactory,
                                          UrlEncoder urlEncoder,
                                          ISystemClock systemClock,
                                          IAccessController accessController,
                                          IPluginHeaderValidator pluginHeaderValidator,
                                          IAuthenticationTicketBuilder authenticationTicketBuilder)
            : base(options, loggerFactory, urlEncoder, systemClock)
        {
            _AccessController = accessController;
            _PluginHeaderValidator = pluginHeaderValidator;
            _AuthenticationTicketBuilder = authenticationTicketBuilder;
        }

        /// <summary>Handles validating the Token value in the header.</summary>
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string tokenStr = Request.Headers[Token];
            var urlAbsolutePath = new Uri(Request.GetDisplayUrl()).AbsolutePath;

            // There are many ways to get headers, but this one works in One Way where others didn't
            Request.Headers.Add("AbsolutePath", urlAbsolutePath);
            var pathAndQuery = string.IsNullOrWhiteSpace(Request.QueryString.Value)
                             ? Request.Path.Value
                             : StringConcat.WithSeparator('?', Request.Path, Request.QueryString.Value);
            Request.Headers.Add("PathAndQuery", pathAndQuery);
            Request.Headers.Add("HttpMethod", Request.Method.ToString());

            if (_AccessController.IsSystemAdmin(Request.Headers) || _AccessController.IsAnonymousAllowed(urlAbsolutePath))
            {
                var authTicket = _AuthenticationTicketBuilder.BuildAdmin();
                return AuthenticateResult.Success(authTicket);
            }

            if (string.IsNullOrWhiteSpace(tokenStr))
            {
                return AuthenticateResult.Fail("Authorization header not found");
            }

            // At this point we need to verify the token is valid, so we will get it and verify
            var headersContainer = new HeadersContainer(Request.Headers);
            if (await _PluginHeaderValidator.IsValidAsync(headersContainer))
            {
                var authTicket = _AuthenticationTicketBuilder.Build(tokenStr);
                return AuthenticateResult.Success(authTicket);
            }
            return AuthenticateResult.Fail(new RestException(urlAbsolutePath, HttpStatusCode.Forbidden));
        }
    }
}