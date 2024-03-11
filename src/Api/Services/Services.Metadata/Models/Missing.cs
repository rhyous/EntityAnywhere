namespace Rhyous.EntityAnywhere.Services
{
    public class Missing<T>
    {
        public Missing() { }
        public Missing(T t) { Object = t; }

        public T Object { get; set; }

        public bool IsMissing { get; set; }

        public static implicit operator T(Missing<T> o)
            => o.Object;

        public static implicit operator Missing<T>(T t)
            => new Missing<T>(t);
    }
}
