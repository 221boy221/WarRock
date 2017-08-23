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
    /// Creates a room in the photon network with the given roomtype (private/public). 
    /// Also generates a room specific token that is used by other players to join your room.
    /// </summary>
    public void CreateRoom(RoomTypes roomType)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties.Add(RoomProperties.TOKEN, GenerateRoomToken());
        roomOptions.CustomRoomProperties.Add(RoomProperties.ROOM_TYTPE, (int)roomType);
        roomOptions.CustomRoomPropertiesForLobby = new string[]{RoomProperties.TOKEN, RoomProperties.ROOM_TYTPE};
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom("", roomOptions, TypedLobby.Default);
    }

    /// <summary>
    /// Connects the user to the room that matched the given token. If no room is found, nothing will happen.
    /// </summary>
    public bool JoinRoomWithToken(int token)
    {
        RoomInfo room = null;

        // Find room that matches given token
        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            if ((int)roomInfo.CustomProperties["TO"] == token && roomInfo.IsOpen)
            {
                room = roomInfo;
                Debug.Log("Room token match found!");
            }
        }

        // If room is found, attempt connection
        if (room != null)
        {
            if (JoinRoom(room.Name))
            {
                Debug.Log("Successfully connected to room with token [" + token + "].");
                return true;
            }
            else
            {
                Debug.Log("Failed to connect to room with token [" + token + "]...");
                // Todo: Give player feedback of connection failure
                return false;
            }
        }
        else
        {
            Debug.Log("Invalid token [" + token + "]...");
            // Todo: Give player feedback of invalid token
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
            if ((int)room.CustomProperties["TY"] == 0 && room.IsOpen)
            {
                publicRooms.Add(room);
            }
        }

        // Select a random room from the list and attempt connection
        if (publicRooms.Count > 0)
        {
            if (JoinRoom(publicRooms[Random.Range(0, publicRooms.Count)].Name))
            {
                Debug.Log("Successfully connected to random public room.");
                return true;
            }
            else
            {
                Debug.Log("Failed to connect to public room...");
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
    internal void SetDefaultPlayerProperties(bool ofAllPlayersInRoom)
    {
        ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable
        {
            { PlayerProperties.READY_STATE, false },
            { PlayerProperties.ROLE, PlayerRoles.None }
        };

        if (ofAllPlayersInRoom)
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                player.SetCustomProperties(propertiesToSet);
            }
        }
        else
        {
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }

    /// <summary>
    /// Generates and returns a token that exists of 5 numbers. 
    /// Excludes tokens that are already in use by the server.
    /// </summary>
    public int GenerateRoomToken()
    {
        HashSet<int> exclude = new HashSet<int>() { };
        
        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            exclude.Add((int)roomInfo.CustomProperties[RoomProperties.TOKEN]);
        }

        IEnumerable<int> range = Enumerable.Range(0, 99999).Where(i => !exclude.Contains(i));
        System.Random rand = new System.Random();

        int index = rand.Next(10000, 99999 - exclude.Count);

        return range.ElementAt(index);
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
        SetDefaultPlayerProperties(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        
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