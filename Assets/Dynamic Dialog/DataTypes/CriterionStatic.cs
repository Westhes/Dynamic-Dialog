using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.DynamicDialog.DataTypes
{
    [Serializable]
    public struct CriterionStatic : IDataType
    {
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        public float MaxValue => maxValue;
        public float MinValue => minValue;
        public CriterionStatic(float min, float max) => (minValue, maxValue) = (min, max + float.Epsilon);

        public static CriterionStatic CreateEquals(float value) => new (value, value);
        public static CriterionStatic CreateRange(float value, float range) => new (value - (range * 0.5f), value + (range * 0.5f));

        public override bool Equals(object obj) => obj switch
        {
            int i => Compare(i),
            float f => Compare(f),
            _ => false,
        };//=> (obj is int i && Compare(i)) || (obj is float f && Compare(f));
        public override int GetHashCode() => (maxValue + minValue).GetHashCode();

        public bool Compare(float f) 
        {
            return f >= minValue && f <= maxValue;
        }
    }
}