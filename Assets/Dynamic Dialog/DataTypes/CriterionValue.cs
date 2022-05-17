using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.DynamicDialog.DataTypes
{
    [Serializable]
    public struct CriterionValue
    {
        public enum ConditionOperator
        {
            Equals,
            Greater,
            GreaterOrEquals,
            Less,
            LessOrEquals
        }

        [SerializeField] private float value;
        [SerializeField] private ConditionOperator conditionOperator;

        public float Value => value;
        public ConditionOperator Operator => conditionOperator;

        public CriterionValue(float v, ConditionOperator o) => (value, conditionOperator) = (v, o);

        public override bool Equals(object obj) => (obj is int i && Compare(i)) || (obj is float f && Compare(f));

        public override int GetHashCode() => value.GetHashCode();

        private bool Compare(int i) => conditionOperator switch
        {
            ConditionOperator.Equals => i == value,
            ConditionOperator.Greater => i > value,
            ConditionOperator.GreaterOrEquals => i >= value,
            ConditionOperator.Less => i < value,
            ConditionOperator.LessOrEquals => i <= value,
            _ => false,
        };

        private bool Compare(float i) => conditionOperator switch
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