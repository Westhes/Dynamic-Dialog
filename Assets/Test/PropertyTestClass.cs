using GameName.Utility.Watcher;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyTestClass : MonoBehaviour
{
    public PropertyWatcherObject Watcher;

    public PropertyWatcherObject[] watchers;

    List<object> wtf = new ();

    void Start()
    {
        //Watcher.Initialize();
        //Debug.Log(Watcher.GetValue());
        foreach (var watcher in watchers)
        {
            watcher.Initialize();
            Debug.Log(watcher.GetValue());
        }
    }


    private void Update()
    {
        Poll();
    }

    private void Poll()
    {
        foreach (var watcher in watchers)
        {
            wtf.Add(watcher.GetValue());
        }
    }
}
