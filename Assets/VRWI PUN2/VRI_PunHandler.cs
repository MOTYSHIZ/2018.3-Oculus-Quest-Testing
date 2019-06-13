using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using VRInteraction;

public class VRI_PunHandler : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public VRInteractableItem vRI_item;

    // Start is called before the first frame update
    void Start()
    {
        vRI_item.pickupEvent.AddListener(pickupPUNHelper);
        vRI_item.dropEvent.AddListener(dropPUNHelper);
    }

    void pickupPUNHelper()
    {
        //So this probably won't work because the heldBy VRInteractor probably isn't the gameobject that has a PhotonView attached to it.
        PhotonRoom.SetCustomProperties("" + photonView.ViewID + "-heldStatus", vRI_item.heldBy.GetComponent<PhotonView>().ViewID);
    }

    void dropPUNHelper()
    {
        PhotonRoom.SetCustomProperties("" + photonView.ViewID + "-heldStatus", 0);
    }

    //Player receiving grab event.
    override public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if(propertiesThatChanged.ContainsKey("" + photonView.ViewID + "-heldStatus"))
        {
            if((int)propertiesThatChanged["" + photonView.ViewID + "-heldStatus"] == 0)
            {
                vRI_item.Drop(false);
            }
            else
            {
                //Make the appropriate hand grab the item. However, each client only has their own VR Interactor.
                //So the equivalent would be done via parenting to whatever hand representation object is on the grabbing player.
            }
        }
    }
}
