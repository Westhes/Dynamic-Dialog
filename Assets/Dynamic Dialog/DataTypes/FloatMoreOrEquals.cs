using System;

namespace GameName.DynamicDialog.DataTypes
{
    [Serializable]
    public readonly struct FloatMoreOrEquals : ICriteria
    {
        public readonly float value;

        public FloatMoreOrEquals(float n) => value = n;

        public bool Equals(float f) => (f >= value);

        public override bool Equals(object o) => (o is float f && f >= value) || (o is int i && i >= value);

        public override int GetHashCode() => value.GetHashCode();
    }
}