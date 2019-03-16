using System.Collections.Generic;

namespace EntityAnywhere.EntityWizard
{
    internal class ModelDictionary : Dictionary<string, Model>
    {
        public ModelDictionary()
        {
            Add("Entity", new Model
            {
                WindowTitle = "Entity Anywhere - Entity",
                Heading = "What is your entity name?",
                Label = "Entity name:"
            });
            Add("Interface", new Model
            {
                WindowTitle = "Entity Anywhere - Interface",
                Heading = "What is your entity name? Leave off the \"I\" prefix.",
                Label = "Entity name:"
            });
            Add("Service", new Model
            {
                WindowTitle = "Entity Anywhere - Service",
                Heading = "What is your entity name?",
                Label = "Entity name:"
            });
            Add("WebService", new Model
            {
                WindowTitle = "Entity Anywhere - Web Service",
                Heading = "What is your entity name?",
                Label = "Entity name:"
            });
        }
    }
}
