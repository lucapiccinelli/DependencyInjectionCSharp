using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            var constructors = typeof(T).GetConstructors();
            if (constructors.Any(info => info.GetParameters().Length == 0))
            {
                ParameterlessConstructor<T>();
            }
            else
            {
                ContrstructoriWithParameters<T>(constructors);
            }
        }

        public T Get<T>() => (T)Get(typeof(T));

        private object Get(Type type)
        {
            return _objectsRegistry.ContainsKey(type) ? _objectsRegistry[type] : null;
        }

        private void ContrstructoriWithParameters<T>(ConstructorInfo[] constructors)
        {
            object[] parameters = constructors.First()
                .GetParameters()
                .Select(info => Get(info.ParameterType))
                .ToArray();

            Register((T) Activator.CreateInstance(typeof(T), parameters));
        }

        private void ParameterlessConstructor<T>()
        {
            Register((T) Activator.CreateInstance(typeof(T)));
        }
    }
}