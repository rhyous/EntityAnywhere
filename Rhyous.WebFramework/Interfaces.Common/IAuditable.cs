using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IAuditable
    {
        DateTime CreateDate { get; set; }
        DateTime? LastUpdated { get; set; }
        int CreatedBy { get; set; }
        int? LastUpdatedBy { get; set; }
    }
}
