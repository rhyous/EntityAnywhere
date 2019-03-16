using System.Collections.Generic;

namespace EntityAnywhere.EntityWizard
{
    internal interface IStarter
    {
        void Start(IDictionary<string, string> replacementsDictionary);
    }
}