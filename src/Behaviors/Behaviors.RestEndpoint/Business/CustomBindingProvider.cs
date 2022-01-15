using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Web.Configuration;

namespace Rhyous.EntityAnywhere.Behaviors
{
    internal class CustomBindingProvider : ICustomBindingProvider
    {
        public Binding Get(string entityName, string scheme)
        {
            var serviceBinding = Get($"{entityName}Binding{scheme}"); 
            if (serviceBinding != null)
                return serviceBinding;

            serviceBinding = Get($"EntityBinding{scheme}");
            if (serviceBinding != null)
                return serviceBinding;
            return null;
        }

        public Binding Get(string bindingName)
        {
            var serviceBindingElement = GetBindingElement(BindingCollectionElementList, bindingName); // Find it case insensitively
            if (serviceBindingElement != null)
                return new WebHttpBinding(serviceBindingElement.Name); // Use exact found name
            return null;
        }

        internal IBindingConfigurationElement GetBindingElement(List<BindingCollectionElement> collections, string name)
        {
            return collections.Where(e => e.ConfiguredBindings != null && e.ConfiguredBindings.Any())
                              .SelectMany(e => e.ConfiguredBindings)
                              .FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<BindingCollectionElement> BindingCollectionElementList
        {
            get { return _BindingCollectionElementList ?? (_BindingCollectionElementList = GetBindingCollectionElementList()); }
            set { _BindingCollectionElementList = value; }
        } private List<BindingCollectionElement> _BindingCollectionElementList;

        internal List<BindingCollectionElement> GetBindingCollectionElementList()
        {
            var webConfig = WebConfigurationManager.OpenWebConfiguration("~");
            if (webConfig == null)
                return null;
            var serviceModelSectionGroup = ServiceModelSectionGroup.GetSectionGroup(webConfig);
            if (serviceModelSectionGroup == null || serviceModelSectionGroup.Bindings == null 
                || serviceModelSectionGroup.Bindings.BindingCollections == null || !serviceModelSectionGroup.Bindings.BindingCollections.Any())
                return null;
            return serviceModelSectionGroup.Bindings.BindingCollections;
        }
    }
}