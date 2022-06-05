using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

public class CustomEditor : EditorWindow
{
    private VisualElement m_RightPane;

    [MenuItem("Tools/My custom editor")]
    public static void ShowEditor()
    {
        EditorWindow wnd = GetWindow<CustomEditor>();
        wnd.titleContent = new GUIContent("My custom editor");
    }

    public void CreateGUI()
    {
        // Get a list of all sprites in the project
        var allObjectGuids = AssetDatabase.FindAssets("t:Sprite");
        var allObjects = new List<Sprite>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        // Create a two-pane view with the left pane being fixed with
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // Add the view to the visual tree by adding it as a child to the root element
        rootVisualElement.Add(splitView);

        // A TwoPaneSplitView always needs exactly two child elements
        var leftPane = new ListView();
        splitView.Add(leftPane);
        m_RightPane = new VisualElement();
        splitView.Add(m_RightPane);

        // Initialize the list view with all sprites' names
        leftPane.makeItem = () => new Label();
        leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        leftPane.itemsSource = allObjects;

        // React to the user's selection
        leftPane.onSelectionChange += OnSpriteSelectionChange;
    }

    private void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
    {
        // Clear all previous content from the pane
        m_RightPane.Clear();

        // Get the selected sprite
        var selectedSprite = selectedItems.First() as Sprite;
        if (selectedSprite == null)
            return;

        // Add a new Image control and display the sprite
        var spriteImage = new Image();
        spriteImage.scaleMode = ScaleMode.ScaleToFit;
        spriteImage.sprite = selectedSprite;

        // Add the Image control to the right-hand pane
        m_RightPane.Add(spriteImage);
    }
}
