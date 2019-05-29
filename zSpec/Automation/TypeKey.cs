using System;

namespace zSpec.Automation
{
    public readonly struct TypeKey
    {
        public TypeKey(Type type, string key)
        {
            Type = type;
            Key = key;
        }

        public Type Type { get; }

        public string Key { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is TypeKey typeKey)
            {
                return Equals(typeKey);
            }

            return false;
        }

        public bool Equals(TypeKey typeKey)
        {
            return Type == typeKey.Type && Key == typeKey.Key;
        }

        public override int GetHashCode()
        {
            return (Type, Key).GetHashCode();
        }

        public override string ToString()
        {
            return $"{Type},{Key}";
        }
    }
}