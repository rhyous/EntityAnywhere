using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class InvalidEntityIdException : Exception
    {
        public InvalidEntityIdException(string entity, string entityId) : base($"The id for the {entity} entity is invalid: {entityId}")
        {
        }
    }
}
