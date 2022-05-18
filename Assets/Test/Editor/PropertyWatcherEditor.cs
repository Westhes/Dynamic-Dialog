using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(PropertyWatcher))]
public class PropertyWatcherEditor : Editor
{
    PropertyWatcher Instance => (PropertyWatcher)target;
    int _selectedIndex = -1;
    int SelectedIndex
    {
        get => _selectedIndex;
        set 
        {
            if (_selectedIndex != value)
            {
                _selectedIndex = value;

                if (propertyInfoComponentBindings != null && propertyInfoComponentBindings.Count > 0)
                {
                    var (c, pi) = propertyInfoComponentBindings[_selectedIndex];
                    Commit(c, pi);
                }
            }
        }
    }

    Object SelectedObject
    {
        get => Instance.WatchedObject;
        set
        {
            if (Instance.WatchedObject != value)
            {
                Instance.WatchedObject = value;
                _selectedIndex = -1;
                EditorUtility.SetDirty(target);
            }
        }
    }

    List<(Component, PropertyInfo)> propertyInfoComponentBindings;

    void Commit(Component c, PropertyInfo pi)
    {
        Instance.WatchedObject = c;
        Instance.PropertyName = pi.Name;
        EditorUtility.SetDirty(target);

        Debug.Log("Updated data!");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        SelectedObject = EditorGUILayout.ObjectField("Watched Object", SelectedObject, typeof(Object), true);


        Component[] components = null;
        if (SelectedObject is GameObject go)
            components = go.GetComponents<Component>();
        else if (SelectedObject is Component co)
            components = co.GetComponents<Component>();

        if (components != null && components.Length > 0)
        {
            List<string> options = new List<string>();
            propertyInfoComponentBindings = new List<(Component, PropertyInfo)>();
            
            int matchingOptionIndex = 0;
            foreach (var c in components)
            {
                var type = c.GetType();
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                    Where(x => x.CanRead);

                foreach (var prop in props)
                {
                    if (Instance.WatchedObject == c && Instance.PropertyName == prop.Name)
                        _selectedIndex = matchingOptionIndex;

                    // Fill lists
                    options.Add($"{type.Name}/{prop.Name}");
                    propertyInfoComponentBindings.Add((c, prop));
                    matchingOptionIndex++;
                }
            }
            SelectedIndex = EditorGUILayout.Popup("Watched Property", SelectedIndex, options.ToArray());
        }
    }
}
