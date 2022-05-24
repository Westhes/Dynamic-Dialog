namespace GameName.DynamicDialog.DataTypes
{
    using System;
    using UnityEngine;

    /// <summary>
    /// A criterion for single operator comparisons.
    /// </summary>
    [Serializable]
    public struct CriterionValue : ICriteria
    {
        [SerializeField]
        private float value;
        [SerializeField]
        private ConditionOperator conditionOperator;

        public CriterionValue(float v, ConditionOperator o) => (value, conditionOperator) = (v, o);

        public float Value => value;

        public ConditionOperator Operator => conditionOperator;

        public override bool Equals(object obj) => obj switch
        {
            int i => Equals(i),
            float f => Equals(f),
            _ => false,
        };

        public override int GetHashCode() => value.GetHashCode();

        public bool Equals(float i) => conditionOperator switch
        {
            ConditionOperator.Equals => i == value,
            ConditionOperator.Greater => i > value,
            ConditionOperator.GreaterOrEquals => i >= value,
            ConditionOperator.Less => i < value,
            ConditionOperator.LessOrEquals => i <= value,
            _ => false,
        };
    }
}