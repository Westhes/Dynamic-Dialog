using Assets.Editor.T4_Template;

using System;
using System.IO;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace GameName.Utility
{
    public class T4Executor : Editor
    {
        static string className = "GeneratedTestClass";

        [MenuItem("Blackboard/GenerateKeysExample")]
        public static void Execute()
        {
            BlackboardKeyGenerator gen = new()
            {
                NameSpace = "GameName.Utility",
                ClassName = className,
                KeyNames = new string[] { "Hello", "World", "test" },
            };
            string content = gen.TransformText();

            if (!TryGetActiveFolderPath(out string currentDirectory))
            {
                Debug.LogError("File generation aborted, folder could not be found!");
                return;
            }
            string outputPath = currentDirectory + Path.AltDirectorySeparatorChar + className + ".cs";
            Debug.Log($"File written to: {outputPath}\n{content}");
            File.WriteAllText(outputPath, content);
            AssetDatabase.Refresh();
        }

        private static bool TryGetActiveFolderPath(out string path)
        {
            var _tryGetActiveFolderPath = typeof(ProjectWindowUtil).GetMethod("TryGetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object[] args = new object[] { null };
            bool found = (bool)_tryGetActiveFolderPath.Invoke(null, args);
            path = (string)args[0];
            return found;
        }

        private static string ProjectDirectory()
        {
            return Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
        }
    }
}