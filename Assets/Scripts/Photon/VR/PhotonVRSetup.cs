using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Prefabs.CameraRig.TrackedAlias;
using Photon.Pun;
using RootMotion.FinalIK;

/*
 * This class is assigned to a proxy GameObject, not the actual CameraRig for the VR headset. It syncs the position and rotation of the proxy
 * with the headset every frame. 
 * It also tells the server to instantiate hands for players, and initializes their proxy tracking as well.
 * 
 * This was scripted according to FusedVR's video here: https://www.youtube.com/watch?v=-uqSyZIpbfY&t=3584s
 * However, a number of corrections were made to address authority issues that resulted in hand tracking flickering and head tracking issues.
 */
public class PhotonVRSetup : MonoBehaviourPunCallbacks {
    TrackedAliasFacade trackedAliasFacade;

    public GameObject handPositionSetterPrefab;
    private int thisViewID;
    public VRIK vrikChar;
    public Transform vRSet;

    public void Start()
    {   
        /*
         * Starts the proxy tracking to the Headset. Makes sure the proxy is being controlled by the right client that has authority.
         */
        if (photonView.IsMine)
        {
            trackedAliasFacade = GameObject.Find("TrackedAlias").GetComponent<TrackedAliasFacade>();
            thisViewID = photonView.ViewID;
            StartCoroutine(InstantiateHandsForConnectingClient());
        }
    }

    IEnumerator InstantiateHandsForConnectingClient()
    {
        //CHANGE THIS LATER. IT WAS LIVESTREAM CODE. Instead of waiting one second, should really be waiting until "If we know we can set commands right now."
        yield return new WaitForSeconds(1f);
        vRSet = GameObject.Find("Base Tracker").transform;

        Debug.Log("Instantiating Hands");
        var head = PhotonNetwork.Instantiate(System.IO.Path.Combine("PhotonPrefabs", "PhotonHead"), vRSet.position, Quaternion.identity, 0);
        var left = PhotonNetwork.Instantiate(System.IO.Path.Combine("PhotonPrefabs", "PhotonHandLeft"), transform.position, Quaternion.identity, 0);
        var right = PhotonNetwork.Instantiate(System.IO.Path.Combine("PhotonPrefabs", "PhotonHandRight"), transform.position, Quaternion.identity, 0);

        head.name = "playerHead";
        left.name = "hand_l";
        right.name = "hand_r";

        Debug.Log("Handstantiated!");

        //CHANGE THIS LATER. IT WAS LIVESTREAM CODE. Instead of waiting one second, should really be waiting until "If we know we can set commands right now."
        yield return new WaitForSeconds(1f);

        int headId = head.GetComponent<PhotonView>().ViewID;
        int leftViewId = left.GetComponent<PhotonView>().ViewID;
        int rightViewId = right.GetComponent<PhotonView>().ViewID;


        GetComponent<PhotonRPCObjectTracker>().startTracking(vRSet.gameObject);
        head.GetComponent<PhotonRPCObjectTracker>().startTracking(trackedAliasFacade.HeadsetAlias.gameObject);
        left.GetComponent<PhotonRPCObjectTracker>().startTracking(trackedAliasFacade.LeftControllerAlias.gameObject);
        right.GetComponent<PhotonRPCObjectTracker>().startTracking(trackedAliasFacade.RightControllerAlias.gameObject);

        photonView.RPC("RpcSetHandNetworkIdentities", RpcTarget.AllBuffered, headId,leftViewId, rightViewId);
    }

    //This basically connects the local VRControllers to their network tracking model counterparts.
    [PunRPC]
    void RpcSetHandNetworkIdentities(int headID, int leftViewID, int rightViewID)
    {
        var head = PhotonView.Find(headID).gameObject;
        var leftHand = PhotonView.Find(leftViewID).gameObject;
        var rightHand = PhotonView.Find(rightViewID).gameObject;

        //Setting VRIK Solver Targets
        vrikChar.solver.spine.headTarget = head.transform.Find("HeadTrackingOffset");
        vrikChar.solver.leftArm.target = leftHand.transform.Find("HandTrackingOffset");
        vrikChar.solver.rightArm.target = rightHand.transform.Find("HandTrackingOffset");

        Debug.Log("Setting Hand Identities");
        //if (VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<ReferencesForNetwork>().hand == null ||
        //    VRTK_DeviceFinder.GetControllerRightHand().GetComponent<ReferencesForNetwork>().hand == null ||
        //        SingletonReferences.singleton.network_leftHand == null ||
        //        SingletonReferences.singleton.network_rightHand == null)
        //{
        //    VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<ReferencesForNetwork>().hand = leftHand;
        //    VRTK_DeviceFinder.GetControllerRightHand().GetComponent<ReferencesForNetwork>().hand = rightHand;
        //    SingletonReferences.singleton.network_leftHand = leftHand;
        //    SingletonReferences.singleton.network_rightHand = rightHand;
        //}
    }
}
