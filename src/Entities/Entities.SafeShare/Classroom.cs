using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Odata;

namespace Entities.SafeShare
{
    [RelatedEntityForeign(nameof(ClassroomStudentMap), nameof(Classroom))]
    [RelatedEntityMapping("User", nameof(ClassroomStudentMap), nameof(Classroom))]
    public class Classroom : AuditableEntity<long>, IClassroom
    {
        [RelatedEntity("User")]
        public long TeacherId { get; set; }
        [RelatedEntity(nameof(Grade))]
        public int GradeId { get; set; } // Id not Grade year
        [RelatedEntity(nameof(School))]
        public long SchoolId { get; set; }
    }

    public interface IClassroom : IBaseEntity<long>, IAuditable
    {
        long TeacherId { get; set; }
        int GradeId { get; set; } // Id not Grade year
        long SchoolId { get; set; }
    }
}