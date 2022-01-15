using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    /// <summary>
    /// This test object is used for testing RelatedEntities by a field other than the Id.
    /// </summary>
    public interface IUser2 : IBaseEntity<int>, IName
    {
        string UserTypeName { get; set; }
    }
}
