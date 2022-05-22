using System;

using UnityEditor;

using UnityEngine;

namespace GameName.Utility.Watcher
{
    [Serializable]
    public class PropertyWatcher : ISerializationCallbackReceiver
    {
        [SerializeField]
        private UnityEngine.Object _watchedObject;

        [SerializeField]
        private string _propertyName = string.Empty;

        [SerializeField]
        private BlackboardDataTypes _returnType;

        public Func<bool> FuncBool { get; private set; }
        public Func<int> FuncInt { get; private set; }
        public Func<float> FuncFloat { get; private set; }

        public UnityEngine.Object WatchedObject
        {
            get => _watchedObject;
            set
            {
                if (_watchedObject != value)
                {
                    _watchedObject = value;
                    _propertyName = null;
                    FuncBool = null;
                    FuncFloat = null;
                    FuncInt = null;
                }
            }
        }
        public string PropertyName
        {
            get => _propertyName;
            set
            {
                if (_propertyName != value)
                {
                    _propertyName = value;
                    FuncBool = null;
                    FuncInt = null;
                    FuncFloat = null;
                    SetupFunctions();
                }
            }
        }

        /// <summary> Needs to be called if set by the inspector. </summary>
        private void SetupFunctions()
        {
            switch (_returnType)
            {
                case BlackboardDataTypes.Boolean:
                    FuncBool = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), WatchedObject, PropertyName);
                    break;
                case BlackboardDataTypes.Integer:
                    FuncInt = (Func<int>)Delegate.CreateDelegate(typeof(Func<int>), WatchedObject, PropertyName);
                    break;
                case BlackboardDataTypes.Float:
                    FuncFloat = (Func<float>)Delegate.CreateDelegate(typeof(Func<float>), WatchedObject, PropertyName);
                    break;
            }
        }

        /// <summary> Returns the value of the property being watched. </summary>
        /// <see href="https://stackoverflow.com/a/21897095/6590240"> It's important to note that int to float conversion is implicit. Unlike float to int which needs to be explicitly cast. </see>
        /// <remarks> Any type of value is cast to a float. Please use the functions directly instead to avoid casting! </remarks>
        /// <note> When returning an object here instead of a float/integer or a boolean, it will cause 16.6bytes of garbage. Hence why it's of type float. </note>
        public float GetValue() => _returnType switch
        {
            BlackboardDataTypes.Boolean => FuncBool.Invoke().GetHashCode(),
            BlackboardDataTypes.Integer => FuncInt.Invoke(),
            BlackboardDataTypes.Float => FuncFloat.Invoke(),
            _ => throw new IndexOutOfRangeException(),
        };

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() => SetupFunctions();
    }
}