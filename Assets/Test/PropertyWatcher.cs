using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class PropertyWatcher : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Object _watchedObject;

    /// <summary> Upon reassignment, loses the PropertyName assignment
    /// </summary>
    public UnityEngine.Object WatchedObject
    {
        get => _watchedObject;
        set
        {
            if (_watchedObject != value)
            {
                _watchedObject = value;
                _propertyName = null;
                propInfo = null;
            }
        }
    }
    [SerializeField]
    private string _propertyName = string.Empty;
    public string PropertyName
    {
        get => _propertyName; 
        set
        {
            if (WatchedObject != null && _propertyName != value)
            {
                _propertyName = value;
                if (Application.isPlaying)
                    propInfo = AcquireProperty(WatchedObject, value);
            }
        }
    }
    private PropertyInfo propInfo;

    public object PropGetValue() => propInfo?.GetValue(WatchedObject);

    public void Awake()
    {
        //if (watchedObject is GameObject go)
        //{
        //    var allComponents = go.GetComponents<Component>();
        //    foreach (var comp in allComponents)
        //    {
        //        var type = comp.GetType();
        //        Debug.Log($"Name: {comp.name} Type: {comp.GetType()}");
        //        watchedObject = comp; // Has the script reference in the scene from the looks of it..


        //        var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
        //            Where(x => x.CanRead);
        //        foreach (var prop in allProperties)
        //        {
        //            Debug.Log($"    Prop: {prop.Name} type: {prop.PropertyType} value: {prop.GetValue(comp)}");
        //            propInfo = prop; // So this theoritically should save the value as well, if set using an editor
        //        }
        //    }
        //}

        propInfo = AcquireProperty(WatchedObject, PropertyName);
    }

    private void FixedUpdate()
    {
        Debug.Log(PropGetValue());
    }

    private static PropertyInfo AcquireProperty(object obj, string propertyName)
    {
        if (obj == null || string.IsNullOrEmpty(propertyName))
            return null;

        return obj.GetType().GetProperty(propertyName);
    }
}