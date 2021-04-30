using ClassLibrary1.Attributes;

namespace ClassLibrary1.Classes
{
    [Inject]
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
}