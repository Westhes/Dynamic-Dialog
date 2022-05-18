using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

using UnityEngine;

namespace GameName.Utility.Watcher
{
    public enum DataType
    {
        Boolean,
        Integer,
        Float
    }

    [Serializable]
    public class PropertyWatcherObject
    {
        [SerializeField]
        private UnityEngine.Object _watchedObject;

        [SerializeField]
        private string _propertyName = string.Empty;

        [SerializeField]
        private DataType _returnType;

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
                    //function = null;
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
                    Initialize();
                }
            }
        }

        /// <summary> Needs to be called if set by the inspector. </summary>
        public void Initialize()
        {
            switch (_returnType)
            {
                case DataType.Boolean:
                    FuncBool = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), WatchedObject, PropertyName);
                    break;
                case DataType.Integer:
                    FuncInt = (Func<int>)Delegate.CreateDelegate(typeof(Func<int>), WatchedObject, PropertyName);
                    break;
                case DataType.Float:
                    FuncFloat = (Func<float>)Delegate.CreateDelegate(typeof(Func<float>), WatchedObject, PropertyName);
                    break;
            }
        }

        /// <summary> Returns the value of the property being watched. </summary>
        /// <remarks> Any type of value is cast to a float. Please use the functions directly instead to avoid casting! </remarks>
        /// <note> When returning an object here instead of a float/integer or a boolean, it will cause 16.6bytes of garbage. Hence why it's of type float. </note>
        public float GetValue() => _returnType switch
        {
            DataType.Boolean => FuncBool.Invoke().GetHashCode(),
            DataType.Integer => FuncInt.Invoke(),
            DataType.Float => FuncFloat.Invoke(),
            _ => throw new IndexOutOfRangeException(),
        };
    }
}