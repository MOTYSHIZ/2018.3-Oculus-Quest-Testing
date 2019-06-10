using UnityEngine;
using Photon.Pun;

public class PhotonOfflineMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("RoomController") == null)
        {
            PhotonNetwork.OfflineMode = true;
        }  
    }
}
