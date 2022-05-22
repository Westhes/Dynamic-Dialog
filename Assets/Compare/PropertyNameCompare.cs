using System.Diagnostics;
using TMPro;
using UnityEngine;

public class PropertyNameCompare : MonoBehaviour
{
    public TextMeshPro textMesh;
    public PropertyName propNameOne;
    public PropertyName propNameTwo;
    private Stopwatch sw = new Stopwatch();

    void Update()
    {
        sw.Start();
        for (int i = 0; i < 100_000; i++)
        {
            bool test = propNameOne == propNameTwo;
        }
        sw.Stop();
        textMesh.text = sw.Elapsed.ToString();
        sw.Reset();
    }
}
