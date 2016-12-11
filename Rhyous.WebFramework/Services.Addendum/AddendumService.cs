using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class AddendumService : ServiceCommonOneToMany<Addendum, IAddendum, long, string>
    {
    }
}