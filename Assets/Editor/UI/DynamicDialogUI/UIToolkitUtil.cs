namespace GameName.Utility
{
    using System.Linq;
    using UnityEditor;

    public static class UIToolkitUtil
    {
        /// <summary>
        /// Finds the relative path of an object.If this fails it falls back to finding the object by name.
        /// </summary>
        /// <param name="o"> Instance of Unity Object. </param>
        /// <returns> A relative path starting from the Assets/ folder towards the asset. </returns>
        public static string FindRelativePath(UnityEngine.Object o)
        {
            string path;
            path = AssetDatabase.GetAssetPath(o);

            if (!string.IsNullOrEmpty(path)) return path;
            path = AssetDatabase.FindAssets($"{nameof(o)}").FirstOrDefault();
            if (!string.IsNullOrEmpty(path)) return path;

            throw new System.IO.FileNotFoundException($"Object of type {o} not found.");
        }

        /// <summary> Finds the path of the provided class. </summary>
        /// <typeparam name="T"> Object Type. </typeparam>
        /// <returns> A relative path starting from the Assets/ folder towards the class. </returns>
        public static string FindRelativeClassPath<T>()
        {
            var name = typeof(T).Name;
            var id = AssetDatabase.FindAssets(name).FirstOrDefault();
            return AssetDatabase.GUIDToAssetPath(id);
        }

        /// <summary>
        /// Finds the path of the provided class and replaces the path ending with .uxml (Unsafe).
        /// Only suitable when the class and uxml are in the same directory and have the same name.
        /// </summary>
        /// <typeparam name="T"> Object Type. </typeparam>
        /// <returns> A relative path starting from the Assets/ folder towards the class. </returns>
        public static string FindRelativeUXMLPathQuick<T>()
        {
            var name = typeof(T).Name;
            var id = AssetDatabase.FindAssets(name).FirstOrDefault();
            var path = AssetDatabase.GUIDToAssetPath(id);
            return path.Replace(".cs", ".uxml");
        }

        /// <summary> Finds all objects containing the class name and returns the first item containing that + ".uxml" at the end. </summary>
        /// <typeparam name="T"> Object Type. </typeparam>
        /// <returns> A relative path starting from the Assets/ folder towards the class. </returns>
        public static string FindRelativeUXMLPath<T>()
        {
            var name = typeof(T).Name;
            var ids = AssetDatabase.FindAssets(name);
            foreach (var id in ids)
            {
                var path = AssetDatabase.GUIDToAssetPath(id);
                if (path.EndsWith(".uxml")) return path;
            }

            return string.Empty;
        }
    }
}
