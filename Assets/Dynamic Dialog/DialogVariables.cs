using System;

[Serializable]
public struct DialogQuery
{
    public DialogContext context;
    public DialogWorldVariables worldVariables;
    public DialogVariables variables;
}

[Serializable]
public struct DialogQueryResult
{
    public DialogContext context;
    public DialogWorldVariablesKeyValue worldVariables;
    public DialogVariables variables;
}

[Serializable]
public struct DialogWorldVariablesKeyValue
{
    public DialogWorldVariables worldVariables;
    public Func<object> obj;
    //public Func<out> obj;
}
// Describes the situation
//[Flags]
public enum DialogContext
{
    Everything = int.MaxValue,
    InteractedConversation = 1 << 0,
    Comment = 1 << 1,
    Random = 1 << 2,
}
// Variables that should always be present!
[Flags]
public enum DialogWorldVariables
{
    Everything = int.MaxValue,
    /// <summary> bool </summary>
    IsAlive = 1 << 0,
    /// <summary> int </summary>
    DeathCount = 1 << 1,
    /// <summary> int </summary>
    ScoreCount = 1 << 2,
}

// Optional flags
[Flags]
public enum DialogVariables
{
    Everything = int.MaxValue,
    /// <summary> int </summary>
    Interruptions = 1 << 0,
    /// <summary> bool </summary>
    HasJumped = 1 << 1,
    /// <summary> bool </summary>
    ReachedObjective = 1 << 2,
}
