namespace GameName.UI
{
    using GameName.Utility;

    using System.Linq;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class DynamicDialogWindow : EditorWindow
    {
        // Logic containers
        private TabbedMenuController tabbedMenuController;
        private DialogUIBlackboard blackboard;

        // File paths
        public static string CustomEditorTKFilePath { get; private set; }


        [MenuItem("DynamicDialog/DynamicDialog Window")]
        public static void ShowExample()
        {
            DynamicDialogWindow wnd = GetWindow<DynamicDialogWindow>();
            wnd.titleContent = new GUIContent(nameof(DynamicDialogWindow));
        }

        public void OnEnable()
        {
            Debug.Log("OnEnable");
            CustomEditorTKFilePath = UIToolkitUtil.FindRelativeUXMLPathQuick<DynamicDialogWindow>();

            // Load and create UI.
            VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(CustomEditorTKFilePath);
            TemplateContainer treeAsset = original.CloneTree();
            treeAsset.style.flexGrow = 1;
            rootVisualElement.Add(treeAsset);

            // Initialize controls.
            tabbedMenuController = new TabbedMenuController(rootVisualElement);
            tabbedMenuController.RegisterTabCallbacks();

            blackboard = new DialogUIBlackboard(this);
            blackboard.RegisterCallbacks();
        }

        public void OnGUI()
        {
        }
    }
}