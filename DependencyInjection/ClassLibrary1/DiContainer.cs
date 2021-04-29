using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class DiContainer
    {
        private readonly Dictionary<Type, object> _objectsRegistry = new Dictionary<Type, object>();

        public void Register<T>(T myTestClass)
        {
            _objectsRegistry.Add(typeof(T), myTestClass);
        }


        public void Register<T>()
        {
            Register((T)Activator.CreateInstance(typeof(T)));
        }

        public T Get<T>()
        {
            var type = typeof(T);
            return (T)(_objectsRegistry.ContainsKey(type) ? _objectsRegistry[type] : null);
        }
    }
}