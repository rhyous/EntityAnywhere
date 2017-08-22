using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public class RestDictionary : Dictionary<string, string>
    {
        #region Singleton

        private static readonly Lazy<RestDictionary> Lazy = new Lazy<RestDictionary>(() => new RestDictionary());

        public static RestDictionary Instance { get { return Lazy.Value; } }

        internal RestDictionary()
        {
            Init();
        }

        #endregion

        public void Init()
        {
            // https://<server>/<entity>Service.svc/$Metadata (GET)
            // Gets the metadata or csdl of the entity.
            // Returns: Schema of entity. Should be in CSDL (option for both json or xml should exist)
            Add("GetMetadata", "$Metadata");

            // https://<server>/<entity>Service.svc/<entities> (GET)
            // Gets all the entities
            // Returns: All entities
            Add("GetAll", "{0}"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities> (Post of List<Tid>)
            // Takes in a list of entity ids and gets all the entities for the Ids posted.
            // Returns: all the entities for the Ids posted
            Add("GetByIds", "{0}/Ids"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities>(id) (GET)
            // Gets the entity for the id provided.
            // Returns: The etity for the id provided.
            Add("Get", "{0}({{id}})"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities>(id) (GET)
            // Gets the value of  for the id provided. 
            // Complex properties or navigation properties are not supported.
            // Returns: The value of the entity's property in string format.
            Add("GetProperty", "{0}({{id}})/{{property}}"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities>(id)/<property> (GET)
            // Gets the value of  for the property for the enity of the id provided.
            // Returns: The value of the entity's property, after the change, in string format.
            Add("UpdateProperty", "{0}({{id}})/{{property}}"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities> (POST)
            // Adds a new entity.
            // Returns: The entity added.
            Add("Post", "{0}"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities> (PUT)
            // Replaces an existing entity. Call is immutable.
            // Returns: The replaced entity.
            Add("Put", "{0}({{id}})"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities>(id) (PATCH - PatchedEntity object)
            // Replaces the changed properties of an existing entity. Call is immutable.
            // Returns: The patched entity.
            Add("Patch", "{0}({{id}})"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities>(id) (DELETE)
            // Deletes the entity.
            // Returns: bool - true if deleted, false otherwise
            Add("Delete", "{0}({{id}})"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities>(id)/Addenda (GET)
            // Gets the addenda for a specified entity
            // Returns: a list of Addenda for the specified entity.
            Add("GetAddenda", "{0}({{id}})/Addenda"); // {0} should be pluralized entity name

            // https://<server>/<entity>Service.svc/<entities>(id)/Addenda(<name>) (GET)
            // Gets the addendum for a specified entity with the addendum name provided
            // Returns: the addendum with the exact name for the specified entity.
            Add("GetAddendaByName", "{0}({{id}})/Addenda({{name}})"); // {0} should be pluralized entity name, {name} is the addenda name. 

            // No implemented yet - may change
            Add("GetByRelatedEntityId", "{0}/?{relatedEntity}={id})"); // {0} should be pluralized entity name

            // https://<server>/<MappingEntity>Service.svc/<entities>/<MappedEntity1> (Post of List<E1Tid>)
            // Takes in a list of entity ids and gets all the entities for the Ids posted.
            // Returns: all the entities for the Ids posted
            Add("GetByE1Ids", "{0}/{1}/Ids"); // {0} should be pluralized entity name, {1} should be the first mapped entity name pluralized.

            // https://<server>/<MappingEntity>Service.svc/<entities>/<MappedEntity2> (Post of List<E2Tid>)
            // Takes in a list of entity ids and gets all the entities for the Ids posted.
            // Returns: all the entities for the Ids posted
            Add("GetByE2Ids", "{0}/{1}/Ids"); // {0} should be pluralized entity name, {1} should be the second mapped entity name pluralized.
        }
    }
}