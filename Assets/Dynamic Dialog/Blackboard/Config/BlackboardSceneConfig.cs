namespace GameName.DynamicDialog.Blackboard.Config
{
    using GameName.DynamicDialog.Blackboard.Entry;
    using GameName.Utility;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    [CreateAssetMenu(fileName = "Blackboard Scene config", menuName = "ScriptableObjects/Blackboard Scene config", order = 1)]
    public class BlackboardSceneConfig : ScriptableObject
    {
        [field: SerializeField]
        public SceneField Scene { get; private set; }

        public BlackboardEntry[] Entries;

    }
}