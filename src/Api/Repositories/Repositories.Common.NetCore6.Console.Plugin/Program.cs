// See https://aka.ms/new-console-template for more information
using Autofac;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Repositories;
using Rhyous.EntityAnywhere.Services;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.Wrappers;
using System.Configuration;
using IAppSettings = Rhyous.EntityAnywhere.Interfaces.IAppSettings;

var urlParameters = new UrlParameters();

var builder = new ContainerBuilder();
builder.RegisterType<FileIO>().As<IFileIO>()
       .SingleInstance();
builder.RegisterType<AppSettingsContainer>().As<IAppSettings>()
       .WithParameter("appSettings", ConfigurationManager.AppSettings)
       .SingleInstance();

// Plugin Loader Module and overrides
builder.RegisterModule<SimplePluginLoaderModule>();
builder.RegisterType<MyPluginPaths>().As<IPluginPaths>()
       .SingleInstance();

// Entity PropertyInfo
builder.RegisterType<AutofacDIResolver>().As<IDependencyInjectionResolver>()
       .SingleInstance();
builder.RegisterGeneric(typeof(EntityInfo<>)).As(typeof(IEntityInfo<>))
       .SingleInstance();

builder.RegisterType<MyUserDetails>().As<IUserDetails>();
builder.RegisterInstance(new UrlParameters()).As<IUrlParameters>();
builder.RegisterGeneric(typeof(EntityRepositoryLoader<,,,>)).As(typeof(IRepositoryLoader<,,,>)).InstancePerLifetimeScope();
builder.RegisterGeneric(typeof(EntityRepositoryLoaderCommon<,,,>)).As(typeof(IRepositoryLoaderCommon<,,,>)).InstancePerLifetimeScope();

var container = builder.Build();

var loader = container.Resolve<IRepositoryLoader<IRepository<User,IUser,long>, User, IUser, long>>();
var repo = loader.LoadPlugin();
var user = repo.Get(1);
var user2 = repo.Get(2);

