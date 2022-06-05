using GameName.DynamicDialog.Blackboard.Entry;

using System;
using System.Linq;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

public class BlackboardEntryController
{
    private const string fileClassName = "fileName";
    private const string idNameClassName = "idName";
    private const string idClassName = "id";
    private const string typeClassName = "dropDown";
    private const string initialValueClassName = "initialValue";
    private const string currentValueClassName = "currentValue";

    private VisualElement parent;

    Label fileNameLabel;
    TextField nameField;
    Label idLabel;
    DropdownField typeDropdown;
    FloatField initialValue;
    Label currentValueLabel;

    public void SetVisualElement(VisualElement parent)
    {
        this.parent = parent;
        fileNameLabel = parent.Q<Label>(fileClassName);
        nameField = parent.Q<TextField>(idNameClassName);
        idLabel = parent.Q<Label>(idClassName);
        typeDropdown = parent.Q<DropdownField>(typeClassName);
        initialValue = parent.Q<FloatField>(initialValueClassName);
        currentValueLabel = parent.Q<Label>(currentValueClassName);
    }

    public BlackboardEntry Entry { get; private set; }

    public void SetEntryData(BlackboardEntry entry)
    {
        Entry = entry;
        fileNameLabel.text = entry.Name;

        // This is one big #HACK.
        nameField.SetValueWithoutNotify(GetPropertyName());
        nameField.RegisterCallback((BlurEvent b) => { SetPropertyName(new PropertyName(nameField.text)); });

        idLabel.text = $":{entry.Id, 11}";

        typeDropdown.choices = Enum.GetNames(typeof(Scope)).ToList();
        typeDropdown.SetValueWithoutNotify(entry.Scope.ToString());
        typeDropdown.RegisterValueChangedCallback((ChangeEvent<string> cEvent) =>
        {
            if (Enum.TryParse(cEvent.newValue, out Scope result))
                entry.Scope = result;
            else
                Debug.Log($"Something went wrong trying to assign Scope to {entry.Name}");
        });

        initialValue.SetValueWithoutNotify(entry.InitialValue); // TODO: callback
        currentValueLabel.text = entry.Value.ToString();
    }

    /// <summary> Get the property name, only works in editor. </summary>
    string GetPropertyName()
    {
        // name is hidden and only exists in the editor.. This is gonna break, but that's an issue for another day.
        var field = Entry.GetType().GetField("name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var propertyname = field.GetValue(Entry).ToString();
        return propertyname.Substring(0, propertyname.LastIndexOf(':'));
    }

    void SetPropertyName(PropertyName prop)
    {
        var field = Entry.GetType().GetField("name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(Entry, prop);
        idLabel.text = $":{prop.GetHashCode(), 11}";
    }
}