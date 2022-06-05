using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using GameName.UI;

public class CustomEditorTK : EditorWindow
{
    // File paths
    public static readonly string BasePathAssetPath = "Assets/Editor/UI/Test/";

    // Logic classes
    private TabbedMenuController tabbedMenuController;
    private DialogUIBlackboard blackboard;

    private static string CustomEditorTKFilePath => BasePathAssetPath + "CustomEditorTK.uxml";

    [MenuItem("Window/UI Toolkit/CustomEditorTK")]
    public static void ShowExample()
    {
        CustomEditorTK wnd = GetWindow<CustomEditorTK>();
        wnd.titleContent = new GUIContent("CustomEditorTK");
    }

    public void OnEnable()
    {
        Debug.Log("OnEnable");

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

    /// <summary> I guess this is the new update method. </summary>
    public void OnGUI()
    {
        //Debug.Log("Reloaded GUI");
    }
}