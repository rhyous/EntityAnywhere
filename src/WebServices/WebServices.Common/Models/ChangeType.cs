namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// This is the type of change requested for a webservice.
    /// Post = Create
    /// Patch/Put = Update
    /// Delete = Delete
    /// </summary>
    public enum ChangeType
    {
        Create,
        Update,
        Delete
    }
}
