using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Rhyous.EntityAnywhere.Security;
using Rhyous.EntityAnywhere.Tools;
using Rhyous.EntityAnywhere.WebApi.Filters;

namespace Rhyous.EntityAnywhere.WebApi
{
    public class ServiceConfigurator : IServiceConfigurator
    {
        private readonly IEntityControllerFeatureProvider _EntityControllerFeatureProvider;

        public ServiceConfigurator(IEntityControllerFeatureProvider entityControllerFeatureProvider)
        {
            _EntityControllerFeatureProvider = entityControllerFeatureProvider;
        }

        public void Configure(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(new ExampleResourceFilter());
                options.Filters.Add(new ExampleActionFilter());
            })
            .AddControllersAsServices()
            .ConfigureApplicationPartManager(apm => apm.FeatureProviders.Add(_EntityControllerFeatureProvider))
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                //options.SerializerSettings.Converters.Add(new SafeIntConverter());
                options.SerializerSettings.ContractResolver = ContractResolver.Instance;
            });

            services.UseTokenAuthentication();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Rhyous.EntityAnywhere", Version = "v2" });
                c.CustomSchemaIds(type => type.FullName); // This is used so that custom methods with similarly named models can work
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyAllowedOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            services.ConfigureSwaggerGen(o => o.OperationFilter<SwaggerHeaderOperationFilter>());
        }
    }
}