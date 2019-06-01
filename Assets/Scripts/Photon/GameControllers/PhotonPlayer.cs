using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour {

    private PhotonView PV;
    public GameObject myAvatar;

	// Use this for initialization
	void Start () {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(System.IO.Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation);
        }
	}
}
