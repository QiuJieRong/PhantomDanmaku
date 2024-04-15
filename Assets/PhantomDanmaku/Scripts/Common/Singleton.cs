namespace PhantomDanmaku
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T s_Instance;

        public static T Instance => s_Instance ??= new T();
    }
}