using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Odata;

namespace Entities.SafeShare
{
    [AdditionalWebServiceTypes(typeof(int), typeof(long))]
    [MappingEntity(Entity1 = "Classroom", Entity2 = "Student")]
    public class ClassroomStudentMap : BaseEntity<long>, IClassroomStudentMap
    {
        /// <inheritdoc />
        [RelatedEntity(nameof(Classroom))]
        [DistinctProperty("MappingGroup")]
        public long ClassroomId { get; set; }

        [RelatedEntity("User")]
        [DistinctProperty("MappingGroup")]
        public long StudentId { get; set; }
    }

    public partial interface IClassroomStudentMap : IBaseEntity<long>, IMappingEntity<int, long>
    {
        long ClassroomId { get; set; }
        long StudentId { get; set; }   
    }
}
