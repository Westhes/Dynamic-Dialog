using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace GameName.Utility
{
    public enum BlackboardDataTypes
    {
        Boolean,
        Integer,
        Float
    }

    public static class BlackboardDataTypeExtensions
    {
        public static Type GetObjectType(this BlackboardDataTypes t) => t switch
        {
            BlackboardDataTypes.Boolean => typeof(bool),
            BlackboardDataTypes.Integer => typeof(int),
            BlackboardDataTypes.Float => typeof(float),
            _ => throw new NotImplementedException($"Value of type {t} has not been implemented (yet)."),
        };

        /// <summary> Returns the correct DataType enum value. (Null = -1) </summary>
        public static BlackboardDataTypes GetDataType(object t)
        {
            // Unity really doesn't like this: (Type)SerializedProperty.managedReferenceValue
            //  Causing the inspector to crash upon trying, so i guess this will have to do.
            if (t == null)
                return (BlackboardDataTypes)(-1);
            if (t == typeof(bool))
                return BlackboardDataTypes.Boolean;
            if (t == typeof(int))
                return BlackboardDataTypes.Integer;
            if (t == typeof(float))
                return BlackboardDataTypes.Float;

            //Debug.LogError($"Value of type: " + t + " has not been implemented (yet).");
            return (BlackboardDataTypes)(-1);
            //var tCode = Type.GetTypeCode(t);
            //return tCode switch
            //{
            //    TypeCode.Empty => (BlackboardDataTypes)(-1), // Null
            //    TypeCode.Boolean => BlackboardDataTypes.Boolean,
            //    TypeCode.Int32 => BlackboardDataTypes.Integer,
            //    TypeCode.Single => BlackboardDataTypes.Float,
            //    _ => throw new NotImplementedException($"Value of type {t} has not been implemented (yet)."),
            //};
        }
    }
}
