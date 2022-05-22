namespace GameName.Utility
{
    using System;
    using UnityEngine;

    /// <summary>
    /// KeyDataTypePair is a struct that contains the name of an field as integer, and the Type it's expected to be.
    /// Internally it also contains a BlackboardDataTypes enum, which is used for obtaining the type while serializing.
    /// </summary>
    [Serializable]
    public struct KeyDataTypePair : ISerializationCallbackReceiver
    {
        [SerializeField]
        private PropertyName name;
        [SerializeField]
        private BlackboardDataTypes blackboardType;

        private Type type;

        public KeyDataTypePair(string name, BlackboardDataTypes blackboardType)
        {
            this.name = name;
            this.blackboardType = blackboardType;
            this.type = BlackboardDataTypeExtensions.GetObjectType(blackboardType);
        }

        public PropertyName Name => this.name;

        public int NameId => this.name.GetHashCode();

        public Type Type => this.type;

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() => this.type = BlackboardDataTypeExtensions.GetObjectType(this.blackboardType);
    }
}
