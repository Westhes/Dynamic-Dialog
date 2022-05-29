namespace GameName.Utility
{
    using System;

    /// <summary> Extension methods that help with obtaining the right types. </summary>
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

            return (BlackboardDataTypes)(-1);
        }
    }
}