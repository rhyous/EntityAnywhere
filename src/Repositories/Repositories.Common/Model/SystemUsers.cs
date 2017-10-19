namespace Rhyous.WebFramework.Repositories
{
    /// <summary>
    /// This should be temporary. Currently, we don't know who is logged in. So we can use one of these users for auditables when we don't know.
    /// </summary>
    public enum SystemUsers
    {
        System = 1,
        Unknown
    }
}