using System;

namespace zSpec.Automation
{
    internal readonly struct TypeKey
    {
        public TypeKey(Type type, string key)
        {
            this.Type = type;
            this.Key = key;
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
                return this.Equals(typeKey);
            }

            return false;
        }

        public bool Equals(TypeKey typeKey) => this.Type == typeKey.Type && this.Key == typeKey.Key;

        public override int GetHashCode() => (this.Type, this.Key).GetHashCode();

        public override string ToString() => $"{this.Type},{this.Key}";
    }
}
