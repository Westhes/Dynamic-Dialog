using System;

namespace GameName.DynamicDialog.Criteria
{
    [Serializable]
    public readonly struct IntMoreOrEquals : ICriteria
    {
        public readonly int value;

        public IntMoreOrEquals(int n) => value = n;

        public bool Equals(float f) => (f >= value);

        public override bool Equals(object o) => (o is int i && i >= value) || (o is float f && f >= value);

        public override int GetHashCode() => value.GetHashCode();
    }
}