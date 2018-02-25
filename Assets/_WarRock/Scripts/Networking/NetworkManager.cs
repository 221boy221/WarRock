using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{
    #region Vars
    public static NetworkManager Instance;
    #endregion

    #region Constructor
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Creates a room in the photon network with the given room parameters. 
    /// Also sets the lobby room properties.
    /// </summary>
    internal void CreateRoom(string roomName, bool isPrivate, int roomSlots, bool isInvisible)
    {
        // Create and set Room Options
        RoomOptions roomOptions = new RoomOptions() {
            CustomRoomProperties = new Hashtable {
                { RoomProperties.ID, GenerateRoomId() },
                { RoomProperties.ROOM_TYPE, isPrivate },
                { RoomProperties.GAME_MODE, 0 },
                { RoomProperties.MAP_ID, 0 }
            },
            CustomRoomPropertiesForLobby = new string[] {
                RoomProperties.ID,
                RoomProperties.ROOM_TYPE,
                RoomProperties.GAME_MODE,
                RoomProperties.MAP_ID
            },
            MaxPlayers = (byte)roomSlots,
            PublishUserId = false, // Enable for Find Friend feature
            CleanupCacheOnLeave = true,
            IsOpen = !isPrivate,
            IsVisible = !isInvisible
        };

        // Create a room using the custom Room Options
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    /// <summary>
    /// Connects the user to the room that matched the given token. If no room is found, nothing will happen.
    /// </summary>
    internal bool JoinRoomWithId(int roomId)
    {
        RoomInfo room = null;

        // Find room that matches given id
        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            if ((int)roomInfo.CustomProperties[RoomProperties.ID] == roomId)
            {
                Debug.Log("Found room with ID [" + roomId + "]...");
                if (roomInfo.IsOpen)
                {
                    room = roomInfo;
                    Debug.Log("Room is public, joining!");
                }
                else
                {
                    Debug.Log("Cannot join room. Room is private!");
                }
            }
        }

        // If room is found, attempt connection
        if (room != null)
        {
            if (JoinRoom(room.Name))
            {
                Debug.Log("Successfully connected to room with id [" + roomId + "].");
                return true;
            }
            else
            {
                Debug.Log("Failed to connect to room with id [" + roomId + "]...");
                // Todo: Give player feedback of connection failure
                return false;
            }
        }
        else
        {
            Debug.Log("Invalid room id [" + roomId + "]...");
            // Todo: Give player feedback of invalid id
            return false;
        }
    }

    /// <summary>
    /// Connect the user to a random (public) room.
    /// </summary>
    public bool JoinRandomRoom()
    {
        List<RoomInfo> publicRooms = new List<RoomInfo>();

        // Add all public rooms to the list
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            if (room.IsOpen)
            {
                publicRooms.Add(room);
            }
        }

        // Select a random room from the list and attempt connection
        if (publicRooms.Count > 0)
        {
            // Todo: randomize between the 5 lowest ping rooms only
            if (JoinRoom(publicRooms[Random.Range(0, publicRooms.Count)].Name )) 
            {
                Debug.Log("Successfully connected to random public room.");
                return true;
            }
            else
            {
                Debug.Log("Failed to connect to random public room...");
                // Todo: Give player feedback of connection failure
                return false;
            }
        }
        else
        {
            Debug.Log("No public rooms found.");
            // Todo: Give player feedback of no rooms found
            return false;
        }
    }

    /// <summary>
    /// Connects to the room of the given roomID. Returns if succeed or failed.
    /// </summary>
    private bool JoinRoom(string roomID)
    {
        return PhotonNetwork.JoinRoom(roomID);
    }

    /// <summary>
    /// Prepares the custom properties with the default values so that they are not null.
    /// </summary>
    internal void ResetPlayerRoomPrefs(bool resetAllUsersInRoom)
    {
        Hashtable curProperties = new Hashtable();
        curProperties[PlayerProperties.READY_STATE] = false;

        if (resetAllUsersInRoom)
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                player.SetCustomProperties(curProperties);
            }
        }
        else
        {
            PhotonNetwork.player.SetCustomProperties(curProperties);
        }
    }

    /// <summary>
    /// Generates and returns a room ID. 
    /// Excludes room IDs that are already in use by the server.
    /// </summary>
    public int GenerateRoomId()
    {
        HashSet<int> exclusionList = new HashSet<int>() { };
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();

        foreach (RoomInfo roomInfo in roomList)
        {
            exclusionList.Add((int)roomInfo.CustomProperties[RoomProperties.ID]);
        }

        // if i is not in excl list
        int id = Enumerable.Range(0, 1000).Where(i => !exclusionList.Contains(i)).ElementAt(0);
        return id;
    }

    /// <summary>
    /// Disconnects the local user from the room it is in.
    /// </summary>
    public void LeaveRoom()
    {
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// Kicks the given playerID from the room he is in.
    /// </summary>
    public void KickPlayer(int id)
    {
        if (PhotonNetwork.isMasterClient && PhotonNetwork.inRoom)
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.ID == id)
                {
                    PhotonNetwork.CloseConnection(player);
                }
            }
        }
    }

    /// <summary>
    /// Kicks the given PhotonPlayer from the room he is in.
    /// </summary>
    public void KickPlayer(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient && PhotonNetwork.inRoom)
        {
            PhotonNetwork.CloseConnection(player);
        }
    }
    
    #endregion


    #region PHOTON CALLBACKS
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Lobby joined");
        ResetPlayerRoomPrefs(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");

        // Enable once joined game scene
        PhotonNetwork.isMessageQueueRunning = true;
    }

    public override void OnLeftRoom()
    {
        
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {

    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        
    }

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log("OnMasterClientSwitched");   
    }

    #endregion
}