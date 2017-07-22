using System;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
public class Entity1 : IEntity1
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? LastUpdated { get; set; }
    public int CreatedBy { get; set; }
    public int? LastUpdatedBy { get; set; }
}
}
