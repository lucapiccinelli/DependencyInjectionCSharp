using ClassLibrary1;
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
        public void I_CanRegister_AnInterface()
        {
            DiContainer di = new DiContainer();
            di.Register<IMyTestClass>(new MyTestClass1());

            Assert.Equal(new MyTestClass1(), di.Get<IMyTestClass>());
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

        [Fact]
        public void I_CanRegister_NestedTypes()
        {
            DiContainer di = new DiContainer();
            di.Register<IInnerClass>(new InnerClass("x"));
            di.Register<OuterClass>();

            Assert.Equal(new OuterClass(new InnerClass("x")), di.Get<OuterClass>());
        }
    }

    public class OuterClass
    {
        private readonly IInnerClass _innerClass;

        public OuterClass(IInnerClass innerClass)
        {
            _innerClass = innerClass;
        }

        protected bool Equals(OuterClass other)
        {
            return Equals(_innerClass, other._innerClass);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OuterClass) obj);
        }

        public override int GetHashCode()
        {
            return (_innerClass != null ? _innerClass.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"{nameof(_innerClass)}: {_innerClass}";
        }
    }

    public interface IInnerClass
    {
    }

    public class InnerClass: IInnerClass
    {
        private readonly string _value;

        public InnerClass(): this("x")
        {
        }

        public InnerClass(string value)
        {
            _value = value;
        }

        protected bool Equals(InnerClass other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InnerClass) obj);
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

    public class MyTestClass2
    {
        private readonly string _value;

        public MyTestClass2() : this("ciao") { }

        public MyTestClass2(string value)
        {
            _value = value;
        }
    }

    public class MyTestClass1: IMyTestClass
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

    public interface IMyTestClass
    {
    }
}
