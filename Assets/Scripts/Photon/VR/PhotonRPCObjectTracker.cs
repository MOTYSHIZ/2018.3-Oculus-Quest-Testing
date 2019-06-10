using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using VRTK;

public class PhotonRPCObjectTracker : MonoBehaviourPun {
    
    public GameObject trackedObject = null;
    public bool trackPositionOnly = false;


    public void startTracking(GameObject trackedObject)
    {
        this.trackedObject = trackedObject;
        StartCoroutine(TrackNodeCoroutine());
    }

    /*
     *  All this coroutine does is copy the objectToTrack's position and rotation to the object that holds this script.
     * */
    IEnumerator TrackNodeCoroutine()
    {
        while (true)
        {
            if(!trackPositionOnly)transform.rotation = trackedObject.transform.rotation;
            transform.position = trackedObject.transform.position;
            yield return null;
        }
    }
}
