namespace GameName.DynamicDialog
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary> Mutable to avoid unnecessary allocation. </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct BlackboardValue
    {
        public int Id;
        public float Value;

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