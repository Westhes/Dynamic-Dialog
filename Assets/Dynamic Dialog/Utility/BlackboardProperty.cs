namespace GameName.DynamicDialog
{
    using System;
    using GameName.Utility.Watcher;
    using UnityEngine;

    [Serializable]
    public class BlackboardProperty : ISerializationCallbackReceiver
    {
        [SerializeField]
        private PropertyName name;

        [SerializeField]
        private PropertyWatcher watcher;

        public int Id => name.GetHashCode();

        public float GetValue() => watcher.GetValue();

        public void OnBeforeSerialize() { }
        
        public void OnAfterDeserialize()
        {
            // TODO: Check if PropertyName is assigned.
        }
    }
}