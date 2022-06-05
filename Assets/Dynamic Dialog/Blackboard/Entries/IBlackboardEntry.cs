namespace GameName.DynamicDialog.Blackboard.Entry
{
    /// <summary>
    /// TODO/Research: make this an abstract class instead?
    /// This would fix issues such as not being able to search and load the object.
    /// </summary>
    public interface IBlackboardEntry
    {
#if UNITY_EDITOR
        /// <summary> Gets the name of the object. EDITOR ONLY. DO NOT USE! </summary>
        public string Name { get; }
#endif

        /// <summary> Gets the id of the object. Used for comparison. </summary>
        public int Id { get; }

        /// <summary> Gets the current value the object holds. </summary>
        public float Value { get; }
    }
}
