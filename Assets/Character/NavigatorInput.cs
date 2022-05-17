using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorInput : Controller 
{
    [SerializeField] private GameObject[] navigationPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        for (int i = 0; i < navigationPoints.Length; i++)
        {
            if (navigationPoints[i] == null)
            {
                var gameObject = new GameObject($"point_{i}");
                gameObject.transform.parent = this.transform;
                gameObject.transform.position = (navigationPoints.Length > 1) ?
                    navigationPoints[navigationPoints.Length - 1].transform.position :
                    transform.position;
                navigationPoints[i] = gameObject;
            }
        }
    }
}
