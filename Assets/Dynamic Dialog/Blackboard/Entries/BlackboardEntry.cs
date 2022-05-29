namespace GameName.DynamicDialog.Blackboard.Entry
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Blackboard Entry", menuName = "ScriptableObjects/Blackboard Entry", order = 1)]
    public class BlackboardEntry : ScriptableObject, IBlackboardEntry
    {
        [SerializeField]
        private new PropertyName name;

        [field: SerializeField]
        public Scope Scope { get; set; }

        [field: SerializeField]
        public float InitialValue { get; set; }

        /// <summary> Gets or sets the variable. this variable is not stored in between sessions, use InitialValue for this instead. </summary>
        [field: NonSerialized]
        public float Value { get; set; }

        public int Id => name.GetHashCode();

    }

    public enum Scope
    {
        Global,
        Scene,
        Region,
        None,
    }

    public class MyHackyScriptableObject : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private int counter;

        public int Counter { get; set; }

        public void OnAfterDeserialize()
        {
            Counter = counter;
        }

        public void OnBeforeSerialize() { }
    }
}
