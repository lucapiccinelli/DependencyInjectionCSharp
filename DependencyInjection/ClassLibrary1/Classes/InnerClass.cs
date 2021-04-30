using ClassLibrary1.Attributes;

namespace ClassLibrary1.Classes
{
    [Inject]
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
}