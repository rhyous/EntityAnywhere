using System.Collections.Generic;

namespace EntityAnywhere.EntityWizard
{
    public interface IStarter
    {
        void Start(IDictionary<string, string> replacementsDictionary);
    }
}