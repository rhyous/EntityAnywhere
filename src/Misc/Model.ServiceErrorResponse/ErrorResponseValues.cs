using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Models
{
    public class ErrorResponseValues
    {
        public string Type { get; set; }
        public List<object> FailedObjects { get; set; }
        public string Result { get; set; }        
    }
}
