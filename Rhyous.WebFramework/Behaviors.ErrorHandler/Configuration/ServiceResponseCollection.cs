using System.Configuration;

namespace Rhyous.WebFramework.Behaviors
{
    public class ServiceResponseCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public ServiceResponse this[int index]
        {
            get { return (ServiceResponse)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new ServiceResponse this[string Key]
        {
            get { return (ServiceResponse)BaseGet(Key); }
            set
            {
                if (BaseGet(Key) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(Key)));
                }
                BaseAdd(value);
            }
        }

        public void Add(ServiceResponse ServiceResponse)
        {
            BaseAdd(ServiceResponse);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceResponse();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceResponse)element).Key;
        }

        public void Remove(ServiceResponse element)
        {
            BaseRemove(element.Key);
        }

        public void Remove(string Key)
        {
            BaseRemove(Key);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }
}