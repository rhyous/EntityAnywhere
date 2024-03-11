// See https://aka.ms/new-console-template for more information
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Repositories;

var urlParameters = new UrlParameters();
var handler = new AuditablesHandler(urlParameters);
var connectionStringProvider = new EntityConnectionStringProvider<User>();
var userDetails = new MyUserDetails();
var settings = new Settings<User>();
using (var context = new BaseDbContext<User>(connectionStringProvider, handler, settings, userDetails))
{
    var userCount = context.Entities.Count();
}
