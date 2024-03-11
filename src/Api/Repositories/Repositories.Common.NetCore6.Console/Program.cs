// See https://aka.ms/new-console-template for more information
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Repositories;
using System.Configuration;

var urlParameters = new UrlParameters();
var handler = new AuditablesHandler(urlParameters);
var connectionStringProvider = new EntityConnectionStringProvider<User>();
var userDetails = new MyUserDetails();
var appSettings = new AppSettingsContainer(ConfigurationManager.AppSettings, new Rhyous.Wrappers.FileIO());
var settings = new Settings<User>(appSettings);
var migrationsContainers = new MigrationsConfigurationContainer<User>(settings);
using (var context = new BaseDbContext<User>(connectionStringProvider, handler, settings, userDetails, migrationsContainers))
{
    var userCount = context.Entities.Count();
}
