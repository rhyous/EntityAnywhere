namespace Rhyous.WebFramework.WebServices
{
    public class Starter
    {
        public static void Start()
        {
            WebServiceLoader.LoadEntities();
            EntityLoader.LoadEntities();
        }
    }
}
