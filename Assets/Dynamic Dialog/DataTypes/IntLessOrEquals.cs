using System;

namespace GameName.DynamicDialog.DataTypes
{
	/// <summary> Wrapper for int. Overwritten Equals method for comparing other integers whether they are lower or equal. </summary>
	/// <remarks>
	/// Original logic:
	///     if (o.GetType() == typeof(int))
	///	       return (int)o <= number;
	///     return false;
	/// </remarks>
	[Serializable]
	public readonly struct IntLessOrEquals
	{
		public readonly int value;

		public IntLessOrEquals(int n) => value = n;

		public override bool Equals(object o) => (o is int i && i <= value) || (o is float f && f <= value);

		public override int GetHashCode() => value.GetHashCode();
	}
}