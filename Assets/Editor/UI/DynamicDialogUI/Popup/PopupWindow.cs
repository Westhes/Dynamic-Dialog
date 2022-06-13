namespace GameName.UI
{
    using System;
    using GameName.Utility;
    using UnityEditor;
    using UnityEngine.UIElements;

    public class PopupWindow
    {
        private static readonly string ContainerClassName = "popupWindow";
        private static readonly string TitleClassName = "popupTitle";
        private static readonly string TextfieldClassName = "popupTextfield";
        private static readonly string HintLabelClassName = "popupHint";
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
            HintLabel = Window.Q<Label>(HintLabelClassName);
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

        private static string PopupPathName { get; set; }

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

        private static VisualElement CreateUI()
        {
            PopupPathName = UIToolkitUtil.FindRelativeUXMLPathQuick<PopupWindow>();
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PopupPathName).Instantiate();
        }

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
}