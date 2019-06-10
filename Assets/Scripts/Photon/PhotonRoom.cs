using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.IO;
using Bolt;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks {

    public static PhotonRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    //Player info
    private Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;

    //Delayed Start Timers and bools
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float timer_lessThanMaxPlayers;
    private float timer_atMaxPlayers;
    private float timeToStart;


    private void Awake()
    {
        //Set up singleton.
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        //Subscribe to functions
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        //Unsubscribe from functions
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void Start()
    {
        //Set private variables
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        timer_lessThanMaxPlayers = startingTime;
        timer_atMaxPlayers = 6;
        timeToStart = startingTime;
    }

    private void Update()
    {
        //For delay start only, count down to start. Timer stuff.
        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            if(playersInRoom == 1)
            {
                RestartTimer();
            }
            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    timer_atMaxPlayers -= Time.deltaTime;
                    timer_lessThanMaxPlayers = timer_atMaxPlayers;
                    timeToStart = timer_atMaxPlayers;
                }
                else if (readyToCount)
                {
                    timer_lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = timer_lessThanMaxPlayers;
                }
                Debug.Log("Display time to start to the players " + timeToStart);
                if(timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
    }

    //Handles timer on delay start games
    private void RestartTimer()
    {
        timer_lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        timer_atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    public override void OnJoinedRoom()
    {
        //Sets player data when we join the room.
        base.OnJoinedRoom();
        Debug.Log("We are now in a room.");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        //For delay start only.
        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            //Displays players / maxplayers, this should show up in UI somewhere later on.
            Debug.Log("Displayer players in room out of max players possible (" + playersInRoom + " : " + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        //For non delay start.
        else
        {
            StartGame();
        }
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //Called when multiplayer scene is loaded.
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)
        {
            isGameLoaded = true;

            //For delay start game.
            if (MultiplayerSettings.multiplayerSettings.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            //For non delay start game.
            else
            {
                RPC_CreatePlayer();
            }
        }
    }

    private void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient) return;
        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
    }


    //Called whenever another person joins our server.
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room.");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            Debug.Log("Displayer players in room out of max players possible (" + playersInRoom + " : " + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
            if(playersInRoom > 1)
            {
                readyToCount = true;
            }
            if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    [PunRPC]
    void RPC_LoadedGameScene()
    {
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length)    //Makes sure we aren't creating any duplicate player objects.
        {
            PV.RPC("RPC_CreatePlayer",RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_CreatePlayer()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkVRIKPlayer"), transform.position, Quaternion.identity, 0);

    }

    public static void SetCustomProperties(string PropertyToSet, string PropertyValue)
    {
        Hashtable hash = new Hashtable();

        hash[PropertyToSet] = PropertyValue;
        //hash.Add(PropertyToSet, PropertyValue);
        
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    public static void SetCustomProperties(string PropertyToSet, int PropertyValue)
    {
        Hashtable hash = new Hashtable();

        hash[PropertyToSet] = PropertyValue;
        //hash.Add(PropertyToSet, PropertyValue);

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    public static string GetCustomProperties(string PropertyToGet)
    {
        return (string)PhotonNetwork.CurrentRoom.CustomProperties[PropertyToGet];
    }

    public static int GetCustomPropertiesInt(string PropertyToGet)
    {
        return (int)PhotonNetwork.CurrentRoom.CustomProperties[PropertyToGet];
    }

    override public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        //Can probably make this more performant in the future by making it only "Find" if GameManager object is null
        //Also, for some reason, this callback is being called three times initially.
        CustomEvent.Trigger(GameObject.Find("GameManager"), "RoomPropertiesUpdated", propertiesThatChanged);
    }

    
}
