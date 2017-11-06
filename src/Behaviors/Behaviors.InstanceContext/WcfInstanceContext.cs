using System.Collections;
using System.ServiceModel;

namespace Rhyous.WebFramework.Behaviors
{
    public class WcfInstanceContext : IExtension<InstanceContext>
    {
        internal WcfInstanceContext() { }

        ///<summary>A list of items that cab be stored per wcf call.</summary>
        public IDictionary Items
        {
            get { return _Items ?? (_Items = new Hashtable()); }
        } private IDictionary _Items;

        ///<summary>
        /// Gets the current wcf call's instance of <see cref="WcfInstanceContext"/>
        ///</summary>
        public static WcfInstanceContext Current =>  OperationContext.Current.InstanceContext.Extensions.Find<WcfInstanceContext>() ?? GetContext();

        internal static WcfInstanceContext GetContext()
        {
            var context = new WcfInstanceContext();
            OperationContext.Current.InstanceContext.Extensions.Add(context);
            return context;
        }

        /// <inheritdoc />
        public void Attach(InstanceContext owner) { }

        /// <inheritdoc />
        public void Detach(InstanceContext owner) { }
    }
}