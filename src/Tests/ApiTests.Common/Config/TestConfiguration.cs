using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public class TestConfiguration : ITestConfiguration
    {
        protected readonly TestContext Context;

        public TestConfiguration(TestContext context)
        {
            Context = context;
        }

        public string EntityAdminToken => Context.Properties["EntityAdminToken"].ToString();

        public string EntityWebHost => Context.Properties["EntityWebHost"].ToString();

        public string EntitySubpath => Context.Properties["EntitySubpath"].ToString();

        public string EntitlementServicesUrl => Context.Properties["EntitlementServicesUrl"].ToString();

        public string EntitlementServicesSubPath => Context.Properties["EntitlementServicesSubPath"].ToString();
    }
}