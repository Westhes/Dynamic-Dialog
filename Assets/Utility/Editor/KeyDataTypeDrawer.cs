using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace GameName.Utility.Watcher
{
    /// <remarks>
    /// Ideally we directly assign _type from the editor, this sadly does not seem to work, and only causes Unity to crash.
    /// The current workaround for this is to assign the type in 'OnAfterDeserialize' instead.
    /// Likewise, the struct is not set to readonly, since this will make variables not show up in the propertydrawer.
    /// </remarks>
    [CustomPropertyDrawer(typeof(KeyDataTypePair))]
    public class KeyDataTypeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var watchedObjectField = new PropertyField(property.FindPropertyRelative("name"));
            var propertyNameField = new PropertyField(property.FindPropertyRelative("blackboardType"));

            // Add fields to the container.
            container.Add(watchedObjectField);
            container.Add(propertyNameField);

            return container;
        }

        private SerializedProperty pType;
        private SerializedProperty pName;
        private BlackboardDataTypes _selectedDataType = (BlackboardDataTypes)(-1);
        public BlackboardDataTypes SelectedDataType
        {
            get => _selectedDataType;
            set
            {
                if (_selectedDataType != value)
                {
                    _selectedDataType = value;
                    pType.intValue = (int)value;
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            pName = property.FindPropertyRelative("name");
            pType = property.FindPropertyRelative("blackboardType");
            _selectedDataType = (BlackboardDataTypes)pType.intValue;

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Remove indents
            int indent = EditorGUI.indentLevel;
            int tabSpacing = 10;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect pNameRect = new Rect(position.x, position.y, position.width * 0.4f, position.height);
            Rect pTypeRect = new Rect(pNameRect.xMax + tabSpacing, position.y, position.width * 0.6f - tabSpacing, position.height);

            EditorGUI.PropertyField(pNameRect, pName, GUIContent.none, false);
            SelectedDataType = (BlackboardDataTypes)EditorGUI.EnumPopup(pTypeRect, SelectedDataType);

            // Finishing up
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }

}