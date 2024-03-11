using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.WebServices;
using Rhyous.Wrappers;

namespace Rhyous.EntityAnywhere.WebApi
{
    /// <summary>A module for registering per request dependencies.</summary>
    public class WebApiPerRequestModule : Module
    {
        /// <summary>The method to register objects.</summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => ctx.Resolve<IHttpContextAccessor>().HttpContext!).InstancePerLifetimeScope();
            builder.Register(ctx => new HttpRequestWrapper(ctx.Resolve<IHttpContextAccessor>()!.HttpContext!.Request)).As<IHttpRequest>().InstancePerLifetimeScope();
            builder.Register(ctx => new HttpResponseWrapper(ctx.Resolve<IHttpContextAccessor>()!.HttpContext!.Response)).As<IHttpResponse>().InstancePerLifetimeScope();

            builder.RegisterType<WebApiHttpResponseSetter>().As<IHttpStatusCodeSetter>().InstancePerLifetimeScope();

            builder.Register(ctx => new RequestUri { Uri = new Uri(ctx.Resolve<IHttpRequest>().GetDisplayUrl()) }).As<IRequestUri>().InstancePerLifetimeScope();
            builder.Register(ctx => new RequestSourceIpAddress { IpAddress = ctx.Resolve<HttpContext>().Connection?.RemoteIpAddress?.ToString() ?? "" }).As<IRequestSourceIpAddress>().InstancePerLifetimeScope();
            builder.Register(ctx => new Headers { Collection = ctx.Resolve<IHttpRequest>().Headers.ToNameValueCollection() }).As<IHeaders>().InstancePerLifetimeScope();
            builder.RegisterType<ClaimsProvider>().As<IClaimsProvider>().InstancePerLifetimeScope();

            // If there is no token, register the user as Anonymous
            builder.RegisterType<AnonymousUserDetails>();
            builder.RegisterType<UserDetails>();
            builder.Register<IUserDetails>(ctx =>
            {
                var headers = ctx.Resolve<IHeaders>();
                if (string.IsNullOrWhiteSpace(headers.Collection.Get("Token", ""))
                  && string.IsNullOrWhiteSpace(headers.Collection.Get("EntityAdminToken", ""))
                  && string.IsNullOrWhiteSpace(headers.Collection.Get("Bearer", "")))
                    return ctx.Resolve<AnonymousUserDetails>();
                else
                    return ctx.Resolve<UserDetails>();
            }).As<IUserDetails>().InstancePerLifetimeScope();

            builder.Register(ctx => new UrlParameters { Collection = ctx.Resolve<IHttpRequest>().Query.ToNameValueCollection() }).As<IUrlParameters>().InstancePerLifetimeScope();

            // Endpoint handler Provider
            builder.RegisterType<RestHandlerProvider>().As<IRestHandlerProvider>().InstancePerLifetimeScope();
        }
    }
}