namespace GameName.DynamicDialog.Blackboard
{
    using System;
    using GameName.DynamicDialog.Blackboard.Entry;
    using GameName.Utility;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [AddComponentMenu("")]
    public class Blackboard : Singleton<Blackboard>
    {
        [Tooltip("Only add variables here that persist for as long as the blackboard persists through scenes. E.G. Totalscore, lives, health.")]
        [SerializeField]
        private BlackboardProperty[] globalEntries = new BlackboardProperty[0];
        [SerializeField]
        private BlackboardEntry[] sceneEntries = default;

        private BlackboardValue[] globalValues = default;

        /// <summary> Replaces the sceneEntries. </summary>
        /// <param name="entries"> The entries that will be used in future queries. </param>
        public void SetSceneEntries(BlackboardEntry[] entries)
        {
            sceneEntries = entries;
        }

        /// <summary> Queries the argument, scene, and global values.  </summary>
        /// <param name="specificValues"> Unique keys. </param>
        /// <returns> A response. </returns>
        public string Query(BlackboardValue[] specificValues)
        {
            Query query = DictionaryExtensions.Combine(globalValues, sceneEntries);
            query.Combine(specificValues);

            throw new NotImplementedException("TODO: query against the DynamicDialogSystem");
        }

        protected override void OnAwake()
        {
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            // Initialize the globalProperties.
            globalValues = new BlackboardValue[globalEntries.Length];
            for (int i = 0; i < globalEntries.Length; i++)
                globalValues[i].Id = globalEntries[i].Id;
        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
            //throw new NotImplementedException("TODO: implement making it impossible to query logic.");
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) { }

        private void Poll()
        {
            for (int i = 0; i < globalEntries.Length; i++)
            {
                globalValues[i].Value = globalEntries[i].GetValue();
            }
        }
    }
}
