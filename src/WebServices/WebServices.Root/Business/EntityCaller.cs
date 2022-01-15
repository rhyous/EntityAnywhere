using Autofac;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class EntityCaller : IEntityCaller
    {
        private readonly INamedFactory<IEntityClientAsync> _EntityClientFactory;
        private readonly IEntityList _EntityList;

        public EntityCaller(INamedFactory<IEntityClientAsync> entityClientAsyncFactory,
                            IEntityList entityList)
        {
            _EntityClientFactory = entityClientAsyncFactory;
            _EntityList = entityList;
        }

        const string UrlTemplate = "{0}/{1}Service.svc";

        public async Task<IEnumerable<T>> CallAll<T>(string rootUrl, string clientMethod)
            where T : class, IName
        {
            if (string.IsNullOrWhiteSpace(rootUrl) || string.IsNullOrWhiteSpace(clientMethod))
                return null;
            var resultList = new List<T>();
            var taskList = new List<Task>();
            var entityTypeList = _EntityList.Entities.Where(t => t.GetCustomAttributes<EntityAttribute>(false) == null
                                                              || t.GetCustomAttributes<EntityAttribute>(false).All(a => a.CanGenerateRepository))
                                                      .ToList();

            // Do the first entity and make sure it works before doing the rest in parallel
            // It would be nice in the future to group these by repository type somehow
            // And do 1 of each repo type before unleashing the rest in parallel.
            var firstTask = CallOne<T>(rootUrl, clientMethod, entityTypeList.First());
            firstTask.Wait();
            resultList.Add(firstTask.Result);

            // Do the rest of the entities
            foreach (var type in entityTypeList.Skip(1))
            {
                var url = string.Format(UrlTemplate, rootUrl, type.Name);
                if (true) // Check if type has Repo Generation capabilities
                {
                    var task = CallOne<T>(rootUrl, clientMethod, type);
                    taskList.Add(task);
                }
            }
            await Task.WhenAll(taskList);
            foreach (Task<T> task in taskList)
            {
                if (task.Result != null)
                    resultList.Add(task.Result);
            }
            return resultList.OrderBy(r => r.Name);
        }

        public async Task<T> CallOne<T>(string rootUrl, string clientMethod, Type entityType)
            where T : class, IName
        {
            if (string.IsNullOrWhiteSpace(rootUrl) || string.IsNullOrWhiteSpace(clientMethod) || entityType == null)
                return null;
            var client = _EntityClientFactory.Create(entityType.Name);
            CacheMethodInfo(client, clientMethod);

            var task = MethodInfo.Invoke(client, new object[] { true }) as Task<T>;
            return await task;
        }

        internal void CacheMethodInfo(IEntityClientAsync client, string clientMethod)
        {
            if (MethodInfo == null)
                MethodInfo = client.GetType().GetMethod(clientMethod);
            if (MethodInfo == null)
                throw new Exception("IEntityClientAsync doesn't have this method: " + clientMethod);
        }        

        internal IEnumerable<Type> Types { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}