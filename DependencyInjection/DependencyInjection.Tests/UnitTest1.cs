using System;
using System.Collections.Generic;
using Xunit;

namespace DependencyInjection.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ICan_GetAnInstance_PreviouslyInjected()
        {
            DiContainer di = new DiContainer();
            var myTestClass = new MyTestClass1();
            di.Register(myTestClass);

            Assert.Equal(myTestClass, di.Get<MyTestClass1>());
        }

        [Fact]
        public void ICan_GetAnInstace_ByGenerics()
        {
            DiContainer di = new DiContainer();
            di.Register<MyTestClass1>();

            Assert.Equal(new MyTestClass1(), di.Get<MyTestClass1>());
        }

        [Fact]
        public void IF_IGet_SomethingNotRegistered_IGetANull()
        {
            DiContainer di = new DiContainer();
            Assert.Null(di.Get<MyTestClass1>());
        }

        [Fact]
        public void I_CanRegister_MoreThaOneType()
        {
            DiContainer di = new DiContainer();
            di.Register<MyTestClass1>();
            di.Register<MyTestClass2>();

            Assert.NotNull(di.Get<MyTestClass1>());
            Assert.NotNull(di.Get<MyTestClass2>());
        }
    }

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

    public class MyTestClass2
    {
        private readonly string _value;

        public MyTestClass2() : this("ciao") { }

        public MyTestClass2(string value)
        {
            _value = value;
        }
    }

    public class MyTestClass1
    {
        private readonly string _value;

        public MyTestClass1() : this("ciao"){}

        public MyTestClass1(string value)
        {
            _value = value;
        }

        protected bool Equals(MyTestClass1 other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyTestClass1)obj);
        }

        public override int GetHashCode()
        {
            return (_value != null ? _value.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"{nameof(_value)}: {_value}";
        }
    }
}
