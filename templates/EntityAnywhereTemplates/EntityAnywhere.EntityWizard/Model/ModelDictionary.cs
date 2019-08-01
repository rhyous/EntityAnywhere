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
                WindowTitle = "Entity Anywhere - Entity Interface",
                Heading = "What is your entity name? Leave off the \"I\" prefix.",
                Label = "Entity name:"
            });
            Add("Service", new Model
            {
                WindowTitle = "Entity Anywhere - Entity Service",
                Heading = "What is your entity name?",
                Label = "Entity name:"
            });
            Add("WebService", new Model
            {
                WindowTitle = "Entity Anywhere - Entity Web Service",
                Heading = "What is your entity name?",
                Label = "Entity name:"
            });
            Add("CustomWebService", new Model
            {
                WindowTitle = "Entity Anywhere - Custom Web Service",
                Heading = "What is your Web Service name?",
                Label = "Web Service name:"
            });
            Add("EntityEvent", new Model
            {
                WindowTitle = "Entity Anywhere - Entity Event",
                Heading = "What is your entity name?",
                Label = "Entity name:"
            });
        }
    }
}
