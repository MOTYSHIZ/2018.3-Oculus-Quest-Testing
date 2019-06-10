using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InteractableNetworkHandler : MonoBehaviourPunCallbacks
{
    public void transferOwnership()
    {
        if(!photonView.IsMine)photonView.RequestOwnership();
        NetworkOwnershipHandler.singleton.ownershipTransferred += OnOwnershipTransferedGrabCaller;
    }

    public void OnOwnershipTransferedGrabCaller(PhotonView targetView, Player previousOwner)
    {
        //if (targetView.gameObject.GetComponent<VRWI_NetworkHandler>().handRefViewID == handRefViewID)
        //{
        Debug.Log("Ownership transferred!");
        NetworkOwnershipHandler.singleton.ownershipTransferred -= OnOwnershipTransferedGrabCaller;
        //}
    }
}
