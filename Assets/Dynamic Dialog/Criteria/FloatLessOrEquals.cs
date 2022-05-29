using System;

namespace GameName.DynamicDialog.Criteria
{
	/// <summary> Wrapper for int. Overwritten Equals method for comparing other integers whether they are lower or equal. </summary>
	/// <remarks>
	/// Original logic:
	///     if (o.GetType() == typeof(int))
	///	       return (int)o <= number;
	///     return false;
	/// </remarks>
	[Serializable]
	public readonly struct FloatLessOrEquals : ICriteria
	{
		public readonly float value;

		public FloatLessOrEquals(float n) => value = n;

		public bool Equals(float f) => (f <= value);

        public override bool Equals(object o) => (o is float f && f <= value) || (o is int i && i <= value);

		public override int GetHashCode() => value.GetHashCode();
	}
}