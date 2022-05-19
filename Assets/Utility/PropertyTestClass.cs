using GameName.Utility.Watcher;

using UnityEngine;

public class PropertyTestClass : MonoBehaviour
{
    public PropertyWatcher Watcher;
    public PropertyWatcher[] watchers;

    void Start()
    {
        Watcher.Initialize();
        foreach (var watcher in watchers)
            watcher.Initialize();
    }


    private void Update()
    {
        Poll();
    }

    private void Poll()
    {
        for (int i = 0; i < 10; i ++)
            watchers[i % watchers.Length].GetValue();
    }
}
