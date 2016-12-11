# WebFramework
A web framework that uses one microservice per entity.

This idea came from looking at the idea of object-oriented development, objects being an Enitity, how entities are used in an ORM (such as Entity Framework) but not necessarily being for an RDBMS, but instead use the idea of a WebServices for entities, each with a decoupled Repository.


Adding an Entity

In the instructions below. Replace $Entity wth the name of your entity. For example, if your entity is "User" replace $Entity with User.

Step 1 - Create an Interface for the Entity

1.	New Project | Class Library | .NET 4.6.1 or later
    Name: Interfaces.$Entity
	
2.	Project Properties:
    Rename Assembly: Rhyous.WebFramework.Interfaces.$Entity
	Rename Default Namespace: Rhyous.WebFramework.Interfaces  <-- Notice it doesn't have $Entity
	
3.	Add References:
	Interfaces.Common

4.	Add Interface
	Name: I$Entity
	Inheritance: Must inherit IId.
	Other: Make the interface partial
	
```
namespace Rhyous.WebFramework.Interfaces
{
    public interface I$Entity : IId
    {
        // Add properties here
		// No methods please!!! Poco only!!!
    }
}
```

5. Add Extension
```
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    using Entity = I$Entity;

    public static class I$EntityExtensions
    {
        public static IEnumerable<T> ToConcrete<T>(this IEnumerable<Entity> items)
            where T : class, Entity, new()
        {
            return items.Select(i => i.ToConcrete<T>()).ToList();
        }

        public static T ToConcrete<T>(this Entity item)
            where T : class, Entity, new()
        {
            return ConcreteConverter.ToConcrete<T, Entity>(item);
        }
    }
}
```

Step 2 - Create a Service for the Entity


1.	New Project | Class Library | .NET 4.6.1 or later
    Name: Services.$Entity
	
2.	Project Properties:
    Rename Assembly: Rhyous.WebFramework.Services.$Entity
	Rename Default Namespace: Rhyous.WebFramework.Services  <-- Notice it doesn't have $Entity
	
3.	Add References:
	Interfaces.Common
	Interfaces.$Entity
	Services.Common

4.	Add Concrete class that implememts Interface I$Entity
	Name: $Entity
	Inheritance: Must inherit I$Entity.
```	
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class $Entity : I$Entity
    {
        public int Id { get; set; } // <-- Required by IId
		// add other implementations here
    }
}
```

5.  Add Service for the entity

	Note: The service can inherit any of a few classes. Below are examples:

ServiceCommon<T, Tinterface> = This class allows for CRUD operations on an Entity loading a repository from a Plugin. By default the Plugin in the Repositories\Common folder is used. However, if there is a repository in Plugins\Repositories\$Entity, it will use that instead of the common repository. If no other library is using your Services layer, then their is no reason to create the below object. In the WebService (next Step) nstantiate a generic instance of: ServiceCommon<$Entity, I$Entity>
```	
using Rhyous.WebFramework.Interfaces;
using System;

namespace Rhyous.WebFramework.Services
{
    using IEntity = I$Entity;
    using Entity = $Entity;

    public class $EntityService : ServiceCommon<Entity, IEntity>
    {
    }
}
```

ServiceCommonSearchable<T, Tinterface> = This class inherits ServiceCommon and adds to it the ability to search a string property for exact match of a string. You have to create this object to tell it what property is searchable.
```	
using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    using IEntity = I$Entity;
    using Entity = $Entity;

    public class $EntityService : ServiceCommonSearchable<Entity, IEntity>
    {
        public override Expression<Func<Entity, string>> PropertyExpression => e => e.$SearchableProperty;
    }
}
```
ServiceCommonManyToMany<T, Tinterface> = This allows for an Entity that is a many to many map between two other entities. 
```
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public class $EntityService : ServiceCommonManyToMany<$Entity, I$Entity>
    {
        public override string PrimaryEntity => "PrimaryEntity";
        public override string SecondaryEntity => "SecondaryEntity";
    }
}
```

Step 3 - Create a web service for the entity


1.	New Project | Class Library | .NET 4.6.1 or later
    Name: WebServices.$Entity
	
2.	Project Properties:
    Rename Assembly: Rhyous.WebFramework.WebServices.$Entity
	Rename Default Namespace: Rhyous.WebFramework.WebServices  <-- Notice it doesn't have $Entity

3.	Add the following post build code:

```
SET copyToDir="$(SolutionDir)WebServices.Main\Plugins\WebServices"
ECHO %copyToDir%
IF NOT EXIST %copyToDir% MKDIR %copyToDir%
COPY /Y "$(TargetPath)" %copyToDir%
COPY /Y "$(TargetDir)$(TargetName).pdb" %copyToDir%

REM Uncomment to copy dependencies
REM SET dllDir="%copyToDir%\bin"
REM ECHO %dllDir%
REM IF NOT EXIST %dllDir% MKDIR %dllDir%
REM COPY /Y "$(Targetdir)*.dll" %dllDir%
REM COPY /Y "$(Targetdir)*.pdb" %dllDir%
```
	
4.	Add References:
	System.ServiceModel
	System.ServiceModel.Web
	Interfaces.Common
	Interfaces.$Entity
	Services.Common
	Services.Entity
	WebServices.Common	
	
5.	Add a RESTful WCF webservice Interface
	Name: I$EntityWebService

There are three types of entities
1. Standalone entity
2. One to Many entity - Has a property that is the id of a related entity
3. Many to Many entity - Has two related entites. The related entities have a many to many relationship through this entity.


Standalone Entity
```
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.$Entity;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface I$EntityWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Entitys", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "$Entitys/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<int> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Entitys({id})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Entitys({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "$Entitys", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> Post(List<Entity> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "$Entitys({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "$Entitys({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "$Entitys({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);
    }
}

```

One-to-many Entity
Same as standalone entity with this section
```
        #region One to Many Method
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "$Entitys({id})/Token", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByRelatedEntityId(string id);
        #endregion
```

Many to Many Entity
Same as standalone entity with this section
```
        #region Mapping Table Specific
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$PrimaryEntitys({id})/$SecondaryEntitys", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByPrimaryEntityId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$SecondaryEntitys({id})/$PrimaryEntitys", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetBySecondaryEntityId(string id);
        #endregion
```

Searchable Entity has a slightly different Get method.
```
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Entitys({idOrName})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string idOrName);
```

6.  Add WebService for the entity
	Name: $EntityWebService
	Inheritance: Must inherit I$EntityWebService.
	
Standalone Entity
```
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.$Entity;
using IEntity = Rhyous.WebFramework.Interfaces.I$Entity;
using EntityService = Rhyous.WebFramework.Services.$EntityService;

namespace Rhyous.WebFramework.WebServices
{
    public class $EntityWebService : I$EntityWebService
    {
        public List<OdataObject<Entity>> GetAll()
        {
            return Service.Get()?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public OdataObject<Entity> Get(string id)
        {
            return Service.Get(id.ToInt())?.ToConcrete<Entity>().AsOdata(GetRequestUri());
        }

        public List<OdataObject<Entity>> GetByIds(List<int> ids)
        {
            return Service.Get(ids)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public string GetProperty(string id, string property)
        {
            return Service.GetProperty(id.ToInt(), property);
        }

        public List<Entity> Post(List<Entity> entities)
        {
            return Service.Add(entities.ToList<IEntity>()).ToConcrete<Entity>().ToList();
        }

        public Entity Patch(string id, Entity entity, List<string> changedProperties)
        {
            return Service.Update(id.ToInt(), entity, changedProperties).ToConcrete<Entity>();
        }

        public Entity Put(string id, Entity entity)
        {
            return Service.Replace(id.ToInt(), entity).ToConcrete<Entity>();
        }

        public bool Delete(string id)
        {
            return Service.Delete(id.ToInt());
        }

        public Uri GetRequestUri()
        {
            return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri;
        }

        #region Injectable Dependency
        internal IServiceCommon<Entity, IEntity> Service
        {
            get { return _Service ?? (_Service = new EntityService()); }
            set { _Service = value; }
        } private IServiceCommon<Entity, IEntity> _Service;
        #endregion

    }
}
```

One-to-many Entity
Same as standalone with this section and a different injectable service:
```
        #region One to Many methods
        public List<OdataObject<Entity>> GetByRelatedEntityId(string id)
        {
            return Service.GetByRelatedEntityId(id.ToInt())?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }
        #endregion

        #region Injectable Dependency
        internal IServiceCommonOneToMany<Entity,IEntity> Service
        {
            get { return _Service ?? (_Service = new EntityService()); }
            set { _Service = value; }
        } private IServiceCommonOneToMany<Entity, IEntity> _Service;
        #endregion
```


Many-to-many Entity
Same as standalone with this section and a different injectable service:
```
        #region Many to Many methods
        public List<OdataObject<Entity>> GetByPrimaryEntityId(string id)
        {
            return Service.GetByRelatedEntityId(id.ToInt(), Service.PrimaryEntity)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public List<OdataObject<Entity>> GetBySecondaryEntityId(string id)
        {
            return Service.GetByRelatedEntityId(id.ToInt(), Service.SecondaryEntity)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }
        #endregion
		
        #region Injectable Dependency
        internal IServiceCommonManyToMany<Entity, IEntity> Service
        {
            get { return _Service ?? (_Service = new EntityService()); }
            set { _Service = value; }
        } private IServiceCommonManyToMany<Entity, IEntity> _Service;
        #endregion
```

Searchable Entity
This has a slightly different get.
```
        public OdataObject<Entity> Get(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
                return null;
            if (idOrName.Any(c=>!char.IsDigit(c)))
                return Service.Get(idOrName)?.ToConcrete<Entity>().AsOdata(GetRequestUri());
            return Service.Get(idOrName.ToInt())?.ToConcrete<Entity>().AsOdata(GetRequestUri());
        }
```



Step 4 - Update the Web.Config

1. Add the serviceActivations element if it isn't already added. Here is its path. 
   Important!: DO NOT COPY AND PASTE THIS SNIPPET. These elements may already exist. There may be tons of data already in your web.config already populating these elements.
```
<configuration>
  <system.serviceModel>
    <serviceHostingEnvironment>
      <serviceActivations>
        <add relativeAddress="$EntityService.svc" service="Rhyous.WebFramework.WebServices.$EntityWebService, Rhyous.WebFramework.WebServices.$Entity" />
      </serviceActivations>
    </serviceHostingEnvironment>
  </system.serviceModel>
</configuration>
```
2. Add a service element for the webservice and endpoint.
```
<configuration>
  <system.serviceModel>
    <services>
	  <service behaviorConfiguration="NoAuthServiceBehavior" name="Rhyous.WebFramework.WebServices.$EntityWebService">
        <endpoint address="" behaviorConfiguration="AjaxEnabledBehavior" binding="webHttpBinding" contract="Rhyous.WebFramework.WebServices.I$EntityWebService" />
      </service>
    </services>
  </system.serviceModel>
</configuration>
```

Step 5 - 