namespace Rhyous.BusinessRules
{
    public interface IBusinessRule
    {
        string Name { get; }
        string Description { get; }
        BusinessRuleResult IsMet();
    }
}
