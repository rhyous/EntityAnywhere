# Entity Anywhere 
A new API for developing Business Applications.

What is EntityAnywhere? It is a web framework that uses one microservice per entity. It is a low-code development solution, where the need for new code is limited to the repository only.

In many environments, your could have entities that come from various different places: cloud service, different databases, files, etc. What if you could create in minutes an authentication-ready REST API with Create, Read, Update Delete (CRUD) (and granual *authorization) for all the spread out entities in your environment. That is the solution Entity Anywhere exists to provide.

How did this API come about? Well, after using Entity Framework, it came to my attention that boiler plate code doesn't end with the entity to database code. Boilerplate code includes almost all REST/CRUD/Authentication/Authorization code. The only coding that should be needed is 
1. Defining an entity.
2. Providing UI interactions beyond CRUD.
3. Sometimes creating a custom repository when the repository isn't a new MS SQL database.

## Getting Started 

1. Check out the source code.

2. Build and Run. You have many common Entities built-in.

### Adding an Entity

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
	Inheritance: Must inherit IEntity and IId<Tid> or it is easiest to implement IEntity<Tid>.
	Other: Make the interface partial
	
```
namespace Rhyous.WebFramework.Interfaces
{
    public interface I$Entity : IEntity
    {
        // Add properties here
		// No methods please!!! Poco only!!!
    }
}
```

5. Add this Extension.
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

Step 2 - Create a Model for the Entity


1.	New Project | Class Library | .NET 4.6.1 or later
    Name: Models.$Entity
	
2.	Project Properties:
    Rename Assembly: Rhyous.WebFramework.Services.$Models
	Rename Default Namespace: Rhyous.WebFramework.Models  <-- Notice it doesn't have $Entity
	
3.	Add References:
	Interfaces.Common
	Interfaces.$Entity
	Models.Common

4.	Add Concrete class.
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

5. Add a Post-Build action to copy the entity dll to the Plugin\Entities folder

```
SET copyToDir="$(SolutionDir)WebServices\WebServices.Main\Plugins\Entities"
ECHO %copyToDir%
IF NOT EXIST %copyToDir% MKDIR %copyToDir%
COPY /Y "$(TargetPath)" %copyToDir%
COPY /Y "$(TargetDir)$(TargetName).pdb" %copyToDir%

SET dllDir="%copyToDir%"
ECHO %dllDir%
IF NOT EXIST %dllDir% MKDIR %dllDir%
COPY /Y "$(Targetdir)*.dll" %dllDir%
COPY /Y "$(Targetdir)*.pdb" %dllDir%
```

That is it. You now have a Microservce CRUD API for that entity.

But just because you don't have to customize the WebService, Service, or Repository, doesn't mean you can't.

## Customizations

There are three layers of interaction for each entity. 

1. WebService - The REST front end to be hosted in IIS
2. Service - A layer that translates REST to CRUD for the repository.
3. Repository - A Concrete implementation of the generic IRepository that actually does the CRUD operations.

Each layer has a common generic implementation that will work for many of your entities. The concrete Repository is the one that we expect will change the most. 

### (Optional) Create a Repository for the Entity



### (Optional) Create a Custom Service for the Entity


Only do this step if you need a custom service. Otherwise, use ServiceCommon.cs or another variant such as ServiceCommonSearchable.cs.

1.	New Project | Class Library | .NET 4.6.1 or later
    Name: Services.$Entity
	
2.	Project Properties:
    Rename Assembly: Rhyous.WebFramework.Services.$Entity
	Rename Default Namespace: Rhyous.WebFramework.Services  <-- Notice it doesn't have $Entity
	
3.	Add References:
	Interfaces.Common
	Interfaces.$Entity
    Models.$Entity
	Services.Common

5.  Add Service for the entity

	Note: The service can inherit any of a few classes. Below are examples:

ServiceCommon<T, Tinterface> = This class allows for CRUD operations on an Entity loading a repository from a Plugin. By default the Plugin in the Repositories\Common folder is used. However, if there is a repository in Plugins\Repositories\$Entity, it will use that instead of the common repository. If no other library is using your Services layer, then their is no reason to create the below object. In the WebService (next Step) nstantiate a generic instance of: ServiceCommon<$Entity, I$Entity>
```	
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Models;
using System;

namespace Rhyous.WebFramework.Services
{
    using IEntity = I$Entity;
    using Entity = $Entity;

    public class $EntityService : ServiceCommon<Entity, IEntity>
    {
        // Custom Service methods and overrides here . . .
    }
}
```

### (Optional) Create a Custom Web Service for the Entity


1.	New Project | Class Library | .NET 4.6.1 or later
    Name: WebServices.$Entity
	
2.	Project Properties:
    Rename Assembly: Rhyous.WebFramework.WebServices.$Entity
	Rename Default Namespace: Rhyous.WebFramework.WebServices  <-- Notice it doesn't have $Entity

3.	Add the following post build code:

```
SET copyToDir="$(SolutionDir)WebServices\WebServices.Main\Plugins\WebServices"
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
	Models.$Entity
	Services.Common
	*Services.Entity <-- Only if you added a custom service.
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
    public interface I$EntityWebService<> : IEntityWebService<>
    {        
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
