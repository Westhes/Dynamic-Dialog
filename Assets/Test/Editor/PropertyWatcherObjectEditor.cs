using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace GameName.Utility.Watcher
{
    [CustomPropertyDrawer(typeof(PropertyWatcherObject))]
    public class PropertyWatcherObjectEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var watchedObjectField = new PropertyField(property.FindPropertyRelative("WatchedObject"));
            var propertyNameField = new PropertyField(property.FindPropertyRelative("PropertyName"), "Fancy Name");

            // Add fields to the container.
            container.Add(watchedObjectField);
            container.Add(propertyNameField);

            return container;
        }

        private SerializedProperty property;
        private SerializedProperty watchedObject;
        private SerializedProperty propertyName;
        private SerializedProperty returnType;
        int _selectedIndex = -1;
        int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;

                    if (PropertyCollection != null && PropertyCollection.Length > 0)
                    {
                        var (c, pi) = PropertyCollection[_selectedIndex];
                        Commit(c, pi);
                    }
                }
            }
        }

        string[] PropertyStrings { get; set; }
        (Component, MethodInfo)[] PropertyCollection { get; set; }
        Object scannedObject = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = property;
            watchedObject = property.FindPropertyRelative("_watchedObject");
            propertyName = property.FindPropertyRelative("_propertyName");
            returnType = property.FindPropertyRelative("_returnType");

            // UI stuff
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            int tabSpacing = 10;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, position.width * 0.4f, position.height);
            var nameRect = new Rect(amountRect.xMax + tabSpacing, position.y, position.width * 0.6f - tabSpacing, position.height);

            EditorGUI.PropertyField(amountRect, watchedObject, GUIContent.none);
            FetchData(property);
            SelectedIndex = EditorGUI.Popup(nameRect, SelectedIndex, PropertyStrings ?? (new string[0]));

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        void FetchData(SerializedProperty property)
        {
            // Obtain values
            var obj = watchedObject.objectReferenceValue;
            string propName = propertyName.stringValue;


            // Abort/clean arrays incase they're already populated or populated incorrectly.
            _selectedIndex = -1;
            if (obj == null)
            {
                PropertyStrings = default;
                PropertyCollection = default;
                //_selectedIndex = -1;
                propertyName.stringValue = null;
                return;
            }
            //if (obj == scannedObject) return;
            scannedObject = obj;

            // Create 2 lists for storing properties and references.
            List<string> propertyOptions = new List<string>();
            List<(Component, MethodInfo)> propertyCollection = new List<(Component, MethodInfo)>();


            // Obtain the components
            int matchingOptionIndex = 0;
            Component[] components = null;
            if (obj is GameObject go) components = go.GetComponents<Component>();
            else if (obj is Component co) components = co.GetComponents<Component>();
            if (components == null) return;

            // Loop over all components
            foreach (var c in components)
            {
                var type = c.GetType();
                //var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(x => x.CanRead);
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.IsSpecialName && 
                           (x.ReturnType == typeof(bool) || x.ReturnType == typeof(int) || x.ReturnType == typeof(float)) &&
                           x.GetParameters().Length == 0);

                // Obtain the properties from each component
                foreach (var method in methods)
                {
                    // Assign the right index incase we find the right index.
                    if (obj == c && propertyName.stringValue == method.Name) _selectedIndex = matchingOptionIndex;

                    // Fill lists
                    propertyOptions.Add($"{type.Name}/{method.Name}");
                    propertyCollection.Add((c, method));
                    matchingOptionIndex++;
                }
            }

            PropertyStrings = propertyOptions.ToArray();
            PropertyCollection = propertyCollection.ToArray();
        }

        void Commit(Component c, MethodInfo method)
        {
            watchedObject.objectReferenceValue = c;
            propertyName.stringValue = method.Name;

            int enumValueIndex = -1;
            if (method.ReturnType == typeof(bool)) enumValueIndex = (int)DataType.Boolean;
            else if (method.ReturnType == typeof(int)) enumValueIndex = (int)DataType.Integer;
            else if (method.ReturnType == typeof(float)) enumValueIndex = (int)DataType.Float;
            returnType.enumValueIndex = enumValueIndex;
        }
    }

}