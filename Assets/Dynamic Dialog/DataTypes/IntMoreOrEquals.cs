using System;

namespace GameName.DynamicDialog.DataTypes
{
    [Serializable]
    public readonly struct IntMoreOrEquals : IDataType
    {
        public readonly int value;

        public IntMoreOrEquals(int n) => value = n;

        public bool Compare(float f) => (f >= value);

        public override bool Equals(object o) => (o is int i && i >= value) || (o is float f && f >= value);

        public override int GetHashCode() => value.GetHashCode();
    }
}