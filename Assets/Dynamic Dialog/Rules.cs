using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class Rules : Dictionary<System.Enum, object>
{
    [SerializeField]
    private Response response;
    public Response Response => response;

    /// <summary>
    /// The amount of rules required to pass the comparison
    /// </summary>
    public int Score => Count;
}

[Serializable]
public struct Response
{
    [SerializeField]private string text;
    public string Text => text;

    public Response(string text) => this.text = text;
}