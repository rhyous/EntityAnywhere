using System;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace Rhyous.WebFramework.WebServices
{
    public abstract class BehaviorExtensionBase<T> : BehaviorExtensionElement where T : IServiceBehavior, new()
    {
        #region BehaviorExtensionElement

        public override Type BehaviorType => typeof(T);

        protected override object CreateBehavior()
        {
            return new T();
        }

        #endregion
    }
}
