namespace GameName.DynamicDialog.Blackboard.Entry
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary> Mutable to avoid unnecessary allocation. </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct BlackboardValue : IBlackboardEntry
    {
#if UNITY_EDITOR
        public string Name => "RuntimeVariable";
#endif

        public int Id { get; set; }

        public float Value { get; set; }

        public BlackboardValue(int id, float value)
        {
            this.Id = id;
            this.Value = value;
        }

        public BlackboardValue(int id)
        {
            this.Id = id;
            this.Value = 0f;
        }
    }

}