using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkOwnershipHandler : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks {

    public static NetworkOwnershipHandler singleton;

    public delegate void OwnershipTransferDelegate(PhotonView someTarget, Player somePlayer);
    public event OwnershipTransferDelegate ownershipTransferred;

    private void Start()
    {
        singleton = this;
    }

    //When a client requests ownership, transfer ownership to them.
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        targetView.TransferOwnership(requestingPlayer);
    }

    //Call ownershipTransferred event.
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if(ownershipTransferred != null)
        {
            ownershipTransferred(targetView, previousOwner);
        }
    }
}
