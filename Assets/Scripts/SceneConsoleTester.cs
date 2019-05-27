using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConsoleTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Scene Loaded");
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void printTestString()
    {
        Debug.Log("Test String");
    }
}
