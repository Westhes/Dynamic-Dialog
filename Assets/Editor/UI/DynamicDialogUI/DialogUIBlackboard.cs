using GameName.DynamicDialog.Blackboard.Entry;
using GameName.Utility;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace GameName.UI
{
    public class DialogUIBlackboard
    {
        private static readonly string ListClassName = "BlackboardKeys";
        private static readonly string AddButtonClassName = "unity-list-view__add-button";
        private static readonly string RemoveButtonClassName = "unity-list-view__remove-button";

        private VisualElement root;
        private EditorWindow window;
        private ListView listView;
        private Button addButton;
        private Button removeButton;
        private VisualTreeAsset entryTemplate;
        private PopupWindow popupWindow;
        private BlackboardEntry selectedItemForDeletion;

        public DialogUIBlackboard(EditorWindow window)
        {
            this.window = window;
            this.root = window.rootVisualElement;

            TemplateFilePath = UIToolkitUtil.FindRelativeUXMLPath<BlackboardEntry>();
        }

        /// <summary>
        /// Unfortunately, Unity doesn't allow for searching interfaces nor loading them as interfaces.
        /// Perhaps if it was a abstract class instead..
        /// </summary>
        public List<BlackboardEntry> Entries { get; private set; }

        private static string TemplateFilePath { get; set; }

        public void RegisterCallbacks()
        {
            listView = root.Q<ListView>(ListClassName);
            removeButton = listView.Q<Button>(RemoveButtonClassName);
            addButton = listView.Q<Button>(AddButtonClassName);

            ClearHandlers(removeButton.clickable, "clicked");
            ClearHandlers(addButton.clickable, "clicked");
            removeButton.clicked += RemoveButton_clicked;
            addButton.clicked += AddButton_clicked;

            entryTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplateFilePath);

            AssignTemplate();
            FindAllEntries();
            BindData();
        }

        /// <summary> Removes all eventhandlers from an event. </summary>
        /// <param name="obj"> The class instance containing the event. </param>
        /// <param name="fieldName"> The name of the event. </param>
        private static void ClearHandlers(object obj, string fieldName)
        {
            var o = obj.GetType().GetField(fieldName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            if (o != null)
                o.SetValue(obj, null);
        }

        private void AssignTemplate()
        {
            listView.makeItem = () =>
            {
                var entry = entryTemplate.Instantiate();

                var logic = new BlackboardEntryController();

                entry.userData = logic;

                logic.SetVisualElement(entry);

                return entry;
            };
        }

        private void FindAllEntries()
        {
            // Find all the entries in the scene
            Entries = new List<BlackboardEntry>();

            var blackboardEntryGuids = AssetDatabase.FindAssets("t:BlackboardEntry");
            foreach (var guid in blackboardEntryGuids)
            {
                Entries.Add(AssetDatabase.LoadAssetAtPath<BlackboardEntry>(AssetDatabase.GUIDToAssetPath(guid)));
            }
        }

        private void BindData()
        {
            listView.bindItem = (item, index) =>
            {
                var c = item.userData as BlackboardEntryController;
                c.SetEntryData(Entries[index]);
            };

            listView.itemsSource = Entries;
        }

        private void RemoveButton_clicked()
        {
            if (popupWindow != null)
            {
                popupWindow.Remove();
                popupWindow = null;
                return;
            }

            if (listView.selectedItem is BlackboardEntry bEntry)
            {
                selectedItemForDeletion = bEntry;
                popupWindow = PopupWindow.CreatePrompt(listView.parent, $"Delete file:{bEntry.name}?", ClickedDelete);
            }
        }

        private void ClickedDelete(bool success, string text)
        {
            popupWindow = null;
            if (!success) return;

            Entries.Remove(selectedItemForDeletion);
            listView.itemsSource = Entries;
            listView.RefreshItems();

            var path = AssetDatabase.GetAssetPath(selectedItemForDeletion);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
            }
        }

        private void AddButton_clicked()
        {
            if (popupWindow != null)
            {
                popupWindow.Remove();
                popupWindow = null;
                return;
            }

            popupWindow = PopupWindow.CreateTextInputPrompt(
                listView.parent,
                $"Create new {nameof(BlackboardEntry)}",
                ClickedCreate,
                "New files will be created in the asset folder.");
        }

        private void ClickedCreate(bool success, string text)
        {
            // Allow new popupWindows to be created again.
            popupWindow = null;

            if (!success) return;

            var path = $"Assets/{text}.asset";
            if (File.Exists(path))
            {
                Debug.Log("File already exists with name.");
                return;
            }

            var scriptableObject = ScriptableObject.CreateInstance<BlackboardEntry>();
            AssetDatabase.CreateAsset(scriptableObject, path);

            Entries.Add(scriptableObject);
            listView.itemsSource = Entries;
            listView.RefreshItems();
        }
    }
}