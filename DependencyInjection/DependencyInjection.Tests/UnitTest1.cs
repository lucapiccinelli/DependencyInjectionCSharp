using System;
using Xunit;

namespace DependencyInjection.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            DiContainer di = new DiContainer();
            di.Register(new MyTestClass("ciao"));

            Assert.Equal(new MyTestClass("ciao"), di.Get<MyTestClass>());
        }
    }

    public class DiContainer
    {
        public void Register(MyTestClass myTestClass)
        {
            
        }

        public T Get<T>() where T : new()
        { 
            return new T();
        }
    }

    public class MyTestClass
    {
        private readonly string _value;

        public MyTestClass() : this("ciao"){}

        public MyTestClass(string value)
        {
            _value = value;
        }

        protected bool Equals(MyTestClass other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyTestClass) obj);
        }

        public override int GetHashCode()
        {
            return (_value != null ? _value.GetHashCode() : 0);
        }
    }
}
