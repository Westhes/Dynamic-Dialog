using System.Diagnostics;
using TMPro;
using UnityEngine;

public class StringComparison : MonoBehaviour
{
    public TextMeshPro textMesh;
    public string stringOne;
    public string stringTwo;
    private Stopwatch sw = new Stopwatch();

    void Update()
    {
        sw.Start();
        for (int i = 0; i < 100_000; i++)
        {
            bool test = stringOne == stringTwo;
        }
        sw.Stop();
        textMesh.text = sw.Elapsed.ToString();
        sw.Reset();
    }
}
