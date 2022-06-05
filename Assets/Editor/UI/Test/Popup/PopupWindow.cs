using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.IO;

public class PopupWindow
{
    private static readonly string PopupPathName = "Assets/Editor/UI/Test/Popup/PopupWindow.uxml";
    private static readonly string ContainerClassName = "popupWindow";
    private static readonly string TitleClassName = "popupTitle";
    private static readonly string TextfieldClassName = "popupTextfield";
    private static readonly string hintLabelClassName = "popupHint";
    private static readonly string CreateButtonClassName = "popupCreateButton";
    private static readonly string CloseButtonClassName = "popupCloseButton";
    private static readonly int Width = 300;
    private static readonly int Height = 100;

    private VisualElement parent;
    private bool requireText;
    private string textInput = string.Empty;

    private PopupWindow(VisualElement parent, string titleText, string buttonText, Action<bool, string> onClick, string hint, bool requireTextInput = true)
    {
        this.parent = parent;
        Window = CreateUI();
        Container = Window.Q<VisualElement>(ContainerClassName);
        Title = Window.Q<Label>(TitleClassName);
        TextField = Window.Q<TextField>(TextfieldClassName);
        HintLabel = Window.Q<Label>(hintLabelClassName);
        CreateButton = Window.Q<Button>(CreateButtonClassName);
        CloseButton = Window.Q<Button>(CloseButtonClassName);

        Title.text = titleText;
        CreateButton.text = buttonText;
        HintLabel.text = hint;

        Window.style.position = new StyleEnum<Position>(Position.Absolute);
        Window.style.width = Width;
        Window.style.height = Height;
        Window.style.left = (parent.resolvedStyle.width / 2) - (Width / 2f);
        Window.style.top = (parent.resolvedStyle.height / 2) - (Height / 2f);
        parent.hierarchy.Add(Window);

        requireText = requireTextInput;
        if (!requireText)
        {
            TextField.AddToClassList("unselectedContent");
            HintLabel.AddToClassList("unselectedContent");
        }

        RegisterBindings(onClick);
    }

    public VisualElement Window { get; }

    public VisualElement Container { get; }

    public Label Title { get; }

    public TextField TextField { get; }

    public Label HintLabel { get; }

    public Button CreateButton { get; }

    public Button CloseButton { get; }

    public Action<bool, string> Callback { get; private set; }

    /// <summary> Creates a interruptable text prompt containing a question and a button. </summary>
    /// <param name="parent"> The parent object to which this prompt should attach. (incase it should be drawn over the current object, pass that objects parent.) </param>
    /// <param name="question"> The question that the title should state. </param>
    /// <param name="onClicked"> The event that should get executed. Returns true incase the button was pressed, false if cancelled. </param>
    /// <param name="hint"> Additional text the prompt provides. </param>
    /// <param name="buttonText"> The text taht the button should contain. </param>
    /// <returns> An instance of the class. </returns>
    public static PopupWindow CreateTextInputPrompt(VisualElement parent, string question, Action<bool, string> onClicked, string hint, string buttonText = "Create")
        => new(parent, question, buttonText, onClicked, hint, true);

    /// <summary> Creates a interruptable prompt containing a question and a button. (Hides input field and hint) </summary>
    /// <param name="parent"> The parent object to which this prompt should attach. (incase it should be drawn over the current object, pass that objects parent.) </param>
    /// <param name="question"> The question that the title should state. </param>
    /// <param name="onClicked"> The event that should get executed. Returns true incase the button was pressed, false if cancelled. </param>
    /// <param name="buttonText"> The text that the button should contain. </param>
    /// <returns> An instance of the class. </returns>
    public static PopupWindow CreatePrompt(VisualElement parent, string question, Action<bool, string> onClicked, string buttonText = "Delete")
        => new(parent, question, buttonText, onClicked, string.Empty, false);

    /// <summary> Delete this object safely. </summary>
    public void Remove() => CloseButton_clicked();

    private static VisualElement CreateUI() => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PopupPathName).Instantiate();

    private void RegisterBindings(Action<bool, string> callback)
    {
        Callback = callback;
        TextField.RegisterCallback<BlurEvent>(TextfieldOutOfFocus);
        CreateButton.clicked += Clicked;
        CloseButton.clicked += CloseButton_clicked;
    }

    private void UnregisterBindings()
    {
        Callback = null;
        TextField.UnregisterCallback<BlurEvent>(TextfieldOutOfFocus);
        CreateButton.clicked -= Clicked;
        CloseButton.clicked -= CloseButton_clicked;
    }

    private void Clicked()
    {
        if (string.IsNullOrEmpty(textInput) && requireText) return;
        Callback(true, textInput);
        CloseButton_clicked();
    }

    private void TextfieldOutOfFocus(BlurEvent blur) => textInput = TextField.text;

    private void CloseButton_clicked()
    {
        Callback(false, textInput);
        UnregisterBindings();
        RemoveSelf();
    }

    private void RemoveSelf() => parent.Remove(Window);
}

public class Draggable : MouseManipulator
{
    private bool isActive = false;
    private Vector2 mouseStart;

    public Draggable()
    {
        activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse, });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (isActive)
        {
            evt.StopImmediatePropagation();
            return;
        }

        if (CanStartManipulation(evt))
        {
            mouseStart = evt.localMousePosition;
            isActive = true;
            target.CaptureMouse();
            evt.StopPropagation();
        }
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (!isActive || !target.HasMouseCapture())
            return;

        Vector2 diff = evt.localMousePosition - mouseStart;
        //target.style.top = Mathf.Clamp(target.layout.y + diff.y, 0 + height, 900);
        //target.style.left = Mathf.Clamp(target.layout.x + diff.x, 0 + width, )
        target.style.top = target.layout.y + diff.y;  //Mathf.Clamp(target.layout.y + diff.y, -target.layout.height / 2, 0);
        target.style.left = target.layout.x + diff.x; //Mathf.Clamp(target.layout.x + diff.x, -target.layout.width / 2, 0);

        evt.StopPropagation();
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        if (!isActive || !target.HasMouseCapture() || !CanStopManipulation(evt))
            return;

        isActive = false;
        target.ReleaseMouse();
        evt.StopPropagation();
    }
}