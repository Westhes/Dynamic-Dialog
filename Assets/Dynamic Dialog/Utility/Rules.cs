namespace GameName.DynamicDialog
{
    using System;
    using System.Collections.Generic;
    using GameName.DynamicDialog.Criteria;
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
        [field: SerializeField]
        public string Text { get; }

        public Response(string text) => Text = text;
    }
}