using GameName.DynamicDialog.DataTypes;

using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class Rules : Dictionary<int, ICriteria>
{
    [SerializeField]
    private Response response;

    public Response Response { get => response; set => response = value; }

    /// <summary> Gets the score based off the amount of rules required to pass the comparison. </summary>
    public int Score => Count;
}

[Serializable]
public struct Response
{
    [SerializeField]private string text;
    public string Text => text;

    public Response(string text) => this.text = text;
}