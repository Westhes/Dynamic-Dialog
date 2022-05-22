namespace GameName.Utility.Watcher
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PropertyWatcher : ISerializationCallbackReceiver
    {
        [SerializeField]
        private UnityEngine.Object watchedObject;

        [SerializeField]
        private string propertyName = string.Empty;

        [SerializeField]
        private BlackboardDataTypes returnType;

        public Func<bool> FuncBool { get; private set; }

        public Func<int> FuncInt { get; private set; }

        public Func<float> FuncFloat { get; private set; }

        public BlackboardDataTypes ReturnType => this.returnType;

        public UnityEngine.Object WatchedObject
        {
            get => watchedObject;
            set
            {
                if (watchedObject != value)
                {
                    watchedObject = value;
                    propertyName = null;
                    FuncBool = null;
                    FuncFloat = null;
                    FuncInt = null;
                }
            }
        }

        public string PropertyName
        {
            get => propertyName;
            set
            {
                if (propertyName != value)
                {
                    propertyName = value;
                    FuncBool = null;
                    FuncInt = null;
                    FuncFloat = null;
                    SetupFunctions();
                }
            }
        }

        /// <summary> Returns the value of the property being watched. </summary>
        /// <see href="https://stackoverflow.com/a/21897095/6590240"> It's important to note that int to float conversion is implicit. Unlike float to int which needs to be explicitly cast. </see>
        /// <remarks> Any type of value is cast to a float. Please use the functions directly instead to avoid casting. </remarks>
        /// <note> When returning an object here instead of a float/integer or a boolean, it will cause 16.6bytes of garbage. Hence why it's of type float. </note>
        /// <returns> A float value. Boolean {0-1}. Int implicityly cast to float. </returns>
        public float GetValue() => this.returnType switch
        {
            BlackboardDataTypes.Boolean => this.FuncBool.Invoke().GetHashCode(),
            BlackboardDataTypes.Integer => this.FuncInt.Invoke(),
            BlackboardDataTypes.Float => this.FuncFloat.Invoke(),
            _ => throw new IndexOutOfRangeException(),
        };

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() => this.SetupFunctions();

        /// <summary> Creates the correct function for the assigned values. </summary>
        private void SetupFunctions()
        {
            if (string.IsNullOrEmpty(this.propertyName) || this.watchedObject == null)
                return;

            switch (this.returnType)
            {
                case BlackboardDataTypes.Boolean:
                    this.FuncBool = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), this.WatchedObject, this.PropertyName);
                    break;
                case BlackboardDataTypes.Integer:
                    this.FuncInt = (Func<int>)Delegate.CreateDelegate(typeof(Func<int>), this.WatchedObject, this.PropertyName);
                    break;
                case BlackboardDataTypes.Float:
                    this.FuncFloat = (Func<float>)Delegate.CreateDelegate(typeof(Func<float>), this.WatchedObject, this.PropertyName);
                    break;
            }
        }
    }
}