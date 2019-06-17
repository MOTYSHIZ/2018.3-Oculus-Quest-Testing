using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using VRInteraction;
using UnityEngine.Events;


#if VRInteraction
namespace VRWeaponInteractor
{
    public class ObjectEvent : UnityEvent<object[]>
    { }

    public class VRWI_GunPunHandler : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        public VRGunHandler vRGunHandler;

        // Start is called before the first frame update
        void Start()
        {
            vRGunHandler.pickupEvent.AddListener(pickupPUNHelper);
            vRGunHandler.dropEvent.AddListener(dropPUNHelper);
            VREvent.Listen("Shoot", shootPUNHelper);
        }

        //
        void shootPUNHelper(params object[] args)
        {

        }

        void pickupPUNHelper()
        {
            //So this probably won't work because the heldBy VRInteractor probably isn't the gameobject that has a PhotonView attached to it.
            PhotonRoom.SetCustomProperties("" + photonView.ViewID + "-heldStatus", vRGunHandler.heldBy.GetComponent<PhotonView>().ViewID);
        }

        void dropPUNHelper()
        {
            PhotonRoom.SetCustomProperties("" + photonView.ViewID + "-heldStatus", 0);
        }

        //Player receiving grab event.
        override public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey("" + photonView.ViewID + "-heldStatus"))
            {
                if ((int)propertiesThatChanged["" + photonView.ViewID + "-heldStatus"] == 0)
                {
                    vRGunHandler.Drop(false);
                }
                else
                {
                    //Make the appropriate hand grab the item. However, each client only has their own VR Interactor.
                    //So the equivalent would be done via parenting to whatever hand representation object is on the grabbing player.
                }
            }
        }
    }
}
#endif