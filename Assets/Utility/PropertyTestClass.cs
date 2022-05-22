using GameName.Utility.Watcher;

using UnityEngine;

public class PropertyTestClass : MonoBehaviour
{
    public PropertyWatcher Watcher;
    public PropertyWatcher[] watchers;

    private void Update() => Poll();

    private void Poll()
    {
        for (int i = 0; i < 10; i ++)
            watchers[i % watchers.Length].GetValue();
    }
}
