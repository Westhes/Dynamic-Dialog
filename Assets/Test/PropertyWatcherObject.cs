using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

using UnityEngine;

namespace GameName.Utility.Watcher
{
    public enum DataTypes
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
        private DataTypes _returnType;

        Func<bool> funcBool;
        Func<int> funcInt;
        Func<float> funcFloat;

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
                    Initialize();
                }
            }
        }

        public void Initialize()
        {
            Debug.Log("Return type was set to: " + _returnType);
            switch(_returnType)
            {
                case DataTypes.Boolean:
                    funcBool = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), WatchedObject, PropertyName);
                    break;
                case DataTypes.Integer:
                    funcInt = (Func<int>)Delegate.CreateDelegate(typeof(Func<int>), WatchedObject, PropertyName);
                    break;
                case DataTypes.Float:
                    funcFloat = (Func<float>)Delegate.CreateDelegate(typeof(Func<float>), WatchedObject, PropertyName);
                    break;
            }
        }

        public object GetValue()
        {
            switch(_returnType)
            {
                case DataTypes.Boolean: return funcBool();
                case DataTypes.Integer: return funcInt();
                case DataTypes.Float: return funcFloat();
                default: return null;
            }
            //_property?.GetValue(WatchedObject);
        }

        private static PropertyInfo AcquireProperty(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return null;

            return obj.GetType().GetProperty(propertyName);
        }

    }
}