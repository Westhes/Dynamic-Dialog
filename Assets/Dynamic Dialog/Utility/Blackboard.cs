namespace GameName.DynamicDialog
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using GameName.Utility.Watcher;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Blackboard : Singleton<Blackboard>
    {
        private const float TimeBeforeActivation = 2f;
        [Tooltip("Only add variables here that persist for as long as the blackboard persists through scenes. E.G. Totalscore, lives, health.")]
        [SerializeField]
        private BlackboardProperty[] globalProperties;
        private BlackboardValue[] globalValues;
        private BlackboardValue[] sceneValues;
        private List<BlackboardValue> registeredSceneValues = new List<BlackboardValue>();
        private Coroutine countdown;

        public bool IsRegisteringAllowed { get; private set; } = true;

        // IsQueryingAllowed seems to be the direct opposite of IsRegisteringAllowed.
        public bool IsQueryingAllowed { get; private set; } = false;

        /// <summary> You can only register the first few seconds once a new scene is loaded. </summary>
        /// <param name="id"> Your PropertyName id (PropertyName.GetHashCode()). </param>
        /// <returns> True if the value was registered, false if you were too late. </returns>
        public bool Register(int id)
        {
            if (!IsRegisteringAllowed) return false;

            registeredSceneValues.Add(new BlackboardValue(id));
            return true;
        }

        /// <summary> Queries the argument, scene, and global values.  </summary>
        /// <param name="specificValues"> Unique keys. </param>
        /// <returns> A response. </returns>
        public string Query(BlackboardValue[] specificValues)
        {
            Dictionary<int, float> dict = DictionaryExtensions.Combine(globalValues, sceneValues);
            DictionaryExtensions.Combine(dict, specificValues);

            throw new NotImplementedException("TODO: query against the DynamicDialogSystem");
        }

        protected override void OnAwake()
        {
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            // Initialize the globalProperties.
            globalValues = new BlackboardValue[globalProperties.Length];
            for (int i = 0; i < globalProperties.Length; i++)
                globalValues[i].Id = globalProperties[i].Id;
        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
            StopCoroutine(countdown);
            sceneValues = null;
            IsRegisteringAllowed = true;
            IsQueryingAllowed = false;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            countdown = StartCoroutine(AllowRegistrationPeriod());
        }

        private IEnumerator AllowRegistrationPeriod()
        {
            yield return new WaitForSeconds(TimeBeforeActivation);
            IsRegisteringAllowed = false;
            IsQueryingAllowed = true;

            sceneValues = registeredSceneValues.ToArray();
            registeredSceneValues.Clear();
        }

        private void Poll()
        {
            for (int i = 0; i < globalProperties.Length; i++)
            {
                globalValues[i].Value = globalProperties[i].GetValue();
            }
        }
    }
}
