using Autofac;
using Autofac.Extensions.DependencyInjection;
using Rhyous.EntityAnywhere.WebApi;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

Starter.Start();
var rootContainer = Starter.RootContainer ?? throw new Exception("The Autofac root container cannot be null.");
var wepApiParentScope = Starter.WebApiScope ?? throw new Exception("The Autofac root container cannot be null.");
var builder = WebApplication.CreateBuilder(args);

var containerConfigurator = wepApiParentScope.Resolve<IHostConfigurator>();
containerConfigurator.Configure(builder.Host, wepApiParentScope);

// Add services to the container.
var serviceConfigurator = wepApiParentScope.Resolve<IServiceConfigurator>();
serviceConfigurator.Configure(builder.Services);
ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;



// builder.WebHost.ConfigureKestrel(options =>
// {
//    options.ListenAnyIP(5000);
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
var appConfigurator = wepApiParentScope.Resolve<IAppConfigurator>();
appConfigurator.Configure(app);
var cachPopulator = wepApiParentScope.Resolve<ICachePopulator>();
await app.StartAsync();
await cachPopulator.PopulateAsync();
await app.WaitForShutdownAsync();