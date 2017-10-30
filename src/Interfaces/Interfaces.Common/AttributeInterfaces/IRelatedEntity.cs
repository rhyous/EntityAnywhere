namespace Rhyous.WebFramework.Interfaces
{
    public interface IRelatedEntity
    {
        string RelatedEntity { get; set; }
        bool AutoExpand { get; set; }
    }
}
