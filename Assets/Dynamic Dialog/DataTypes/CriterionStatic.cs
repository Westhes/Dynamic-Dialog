namespace GameName.DynamicDialog.DataTypes
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct CriterionStatic : ICriteria
    {
        [SerializeField]
        private float minValue;
        [SerializeField]
        private float maxValue;

        /// <summary> Initializes a new instance of the <see cref="CriterionStatic"/> struct for is-value-between float comparisons. </summary>
        /// <param name="min"> The minimum value the expected float should have. </param>
        /// <param name="max"> The maximum value the expected float should have. </param>
        public CriterionStatic(float min, float max) => (minValue, maxValue) = (min, max + float.Epsilon);

        /// <summary>
        /// Initializes a new instance of the <see cref="CriterionStatic"/> struct.
        /// Dependant of <paramref name="expectTrue"/> internal parameter for the struct will be set to 1 for true, and 0 for false.
        /// </summary>
        /// <param name="expectTrue"> Should the struct expect to retrieve a value which is true. </param>
        public CriterionStatic(bool expectTrue) => (minValue, maxValue) = expectTrue ? (1f, 1f) : (0f, 0f);

        public float MaxValue => maxValue;

        public float MinValue => minValue;

        public static CriterionStatic CreateEquals(float value) => new(value, value);

        public static CriterionStatic CreateRange(float value, float range) => new(value - (range * 0.5f), value + (range * 0.5f));

        public override bool Equals(object obj) => obj switch
        {
            int i => Equals(i),
            float f => Equals(f),
            _ => false,
        };

        public bool Equals(float f) => f >= minValue && f <= maxValue;

        public override int GetHashCode() => (maxValue + minValue).GetHashCode();
    }
}