using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonReferences : MonoBehaviour {

    public static SingletonReferences singleton;

    public GameObject network_leftHand;

    public GameObject network_rightHand;

    private void Start()
    {
        singleton = this;
    }
}
