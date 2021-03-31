using System;
using System.Threading;

namespace RulersCourt
{
    /// <summary>
    /// This class has function related to db instance creation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbClientFactory<T>
    {
        private static Lazy<T> _factoryLazy = new Lazy<T>(
            () => (T)Activator.CreateInstance(typeof(T)),
            LazyThreadSafetyMode.ExecutionAndPublication);

        public static T Instance
        {
            get
            {
                return _factoryLazy.Value;
            }
        }
    }
}
