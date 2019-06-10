using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTrackingOnly : MonoBehaviour
{
    public GameObject trackedObject;

    public void Start()
    {
        //StartCoroutine(TrackNodeCoroutine());
    }

    public void Update()
    {
        transform.position = new Vector3(trackedObject.transform.position.x, transform.position.y, trackedObject.transform.position.z);
        //transform.localRotation = Quaternion.Euler(trackedObject.transform.rotation.x, trackedObject.transform.rotation.y, trackedObject.transform.rotation.z);
    }

    /*
     *  All this coroutine does is copy the objectToTrack's position and rotation to the object that holds this script.
     * */
    IEnumerator TrackNodeCoroutine()
    {
        while (true)
        {
            transform.position = new Vector3(trackedObject.transform.position.x, transform.position.y, trackedObject.transform.position.z);
            transform.localRotation = Quaternion.Euler(trackedObject.transform.rotation.x, trackedObject.transform.rotation.y, trackedObject.transform.rotation.z);
            yield return null;
        }
    }
}
