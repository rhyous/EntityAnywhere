using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Entities.SafeShare
{
    public class GradeSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<Grade>
        {
            new Grade { Id = 1,  Year = 1,  Name = "1st Grade" },
            new Grade { Id = 2,  Year = 2,  Name = "2nd Grade" },
            new Grade { Id = 3,  Year = 3,  Name = "3rd Grade" },
            new Grade { Id = 4,  Year = 4,  Name = "4th Grade" },
            new Grade { Id = 5,  Year = 5,  Name = "5th Grade" },
            new Grade { Id = 6,  Year = 6,  Name = "6th Grade" },
            new Grade { Id = 7,  Year = 7,  Name = "7th Grade" },
            new Grade { Id = 8,  Year = 8,  Name = "8th Grade" },
            new Grade { Id = 9,  Year = 9,  Name = "Freshman" },
            new Grade { Id = 10, Year = 10, Name = "Sophomore" },
            new Grade { Id = 11, Year = 11, Name = "Junior" },
            new Grade { Id = 12, Year = 12, Name = "Senior" },
            new Grade { Id = 13, Year = 0, Name = "Kindergarten" },
        }.ToList<object>();
    }
}