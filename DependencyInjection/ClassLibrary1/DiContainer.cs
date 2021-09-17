using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClassLibrary1.Attributes;

namespace ClassLibrary1
{
    public class DiContainer
    {
        private readonly Dictionary<Type, object> _objectsRegistry = new Dictionary<Type, object>();

        public void Register<T>(T myTestClass)
        {
            Register(typeof(T), myTestClass);
        }

        private void Register(Type type, object myTestClass)
        {
            _objectsRegistry.Add(type, myTestClass);
        }


        public void Register<T>()
        {
            Register(typeof(T), typeof(T));
        }

        private void Register(Type iinterface, Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Any(info => info.GetParameters().Length == 0))
            {
                Register(iinterface,  Activator.CreateInstance(type));
            }
            else
            {
                object[] parameters = GetParameters(constructors);

                Register(iinterface, Activator.CreateInstance(type, parameters));
            }
        }

        private object[] GetParameters(ConstructorInfo[] constructors) => 
            constructors.First()
                .GetParameters()
                .Select(info => Get(info.ParameterType))
                .ToArray();

        public T Get<T>() => (T)Get(typeof(T));

        private object Get(Type type)
        {
            return _objectsRegistry.ContainsKey(type) ? _objectsRegistry[type] : null;
        }

        public void AutoConfigure()
        {
            IEnumerable<KeyValuePair<Type, Type>> enumerable = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.GetCustomAttribute<InjectAttribute>() != null)
                .Select(type =>
                {
                    Type[] interfaces = type.GetInterfaces();
                    if (interfaces.Length > 0)
                    {
                        return new KeyValuePair<Type, Type>(interfaces.First(), type);
                    }

                    return new KeyValuePair<Type, Type>(type, type); ;
                });

            foreach (KeyValuePair<Type, Type> types in enumerable)
            {
                Register(types.Key, types.Value);
                if (types.Key != types.Value)
                {
                    Register(types.Value, types.Value);
                }
            }
        }
    }
}