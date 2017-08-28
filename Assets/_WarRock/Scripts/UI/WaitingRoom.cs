using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoom : LobbyPanel
{

    #region Vars
    [SerializeField] private GameObject _playerSlotPrefab;
    [SerializeField] private RectTransform _playerSlotHolder;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Text _roomIdLabel;

    Dictionary<int, PlayerSlot> _slots = new Dictionary<int, PlayerSlot>();
    #endregion

    #region Methods
 
    private void OnEnable()
    {
        // Event Listeners
        _readyButton.onClick.AddListener(OnClickedReady);
        _startButton.onClick.AddListener(OnClickedStart);
        _backButton.onClick.AddListener(OnClickedExit);

        // Update UI
        CleanUpList();
        SetClientUI();
        UpdateUI();

        if (PhotonNetwork.room != null)
        {
            _roomIdLabel.text = PhotonNetwork.room.CustomProperties[RoomProperties.ID].ToString();
        }
    }
    private void OnDisable() {
        // Event Listeners
        _readyButton.onClick.RemoveListener(OnClickedReady);
        _startButton.onClick.RemoveListener(OnClickedStart);
        _backButton.onClick.RemoveListener(OnClickedExit);
    }

    /// <summary>
    /// Disconnects the client from the room before switching back to the Main Menu UI Panel.
    /// </summary>
    private void OnClickedExit() { PhotonNetwork.LeaveRoom(); OpenUIPanel(UIPanelTypes.MainMenu); }


    /// <summary>
    /// Changes the Ready button into a Start button if to the player is a master and the other way around.
    /// </summary>
    private void SetClientUI()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            _startButton.gameObject.SetActive(true);
            _readyButton.gameObject.SetActive(false);
        }
        else
        {
            _startButton.gameObject.SetActive(false);
            _readyButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// On clicked Ready button behaviour. Switches state of player custom properties and refreshes the player slot.
    /// </summary>
    private void OnClickedReady()
    {
        Hashtable newProperties = new Hashtable();
        newProperties[PlayerProperties.READY_STATE] = !(bool)PhotonNetwork.player.CustomProperties[PlayerProperties.READY_STATE];

        PhotonNetwork.player.SetCustomProperties(newProperties);

        _slots[PhotonNetwork.player.ID].RefreshData(PhotonNetwork.player);
    }

    /// <summary>
    /// On clicked Start button behaviour. Checks the states of all players and starts the game.
    /// </summary>
    private void OnClickedStart()
    {
        if (PhotonNetwork.room.PlayerCount < PhotonNetwork.room.MaxPlayers) // Todo: add Minimum start amount
        {
            Debug.Log("You need 4 players in order to start! [" + PhotonNetwork.room.PlayerCount + "/" + PhotonNetwork.room.MaxPlayers+ "]");
            return;
        }
        else if (!AllPlayersReady())
        {
            Debug.Log("Not all players are ready!");
            return;
        }

        Debug.Log("Starting game...");

        // Loading level, reset player ready states so we can use them to check if player is done loading
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            Hashtable newProperties = new Hashtable();
            newProperties[PlayerProperties.LOADING_MAP] = true;
            player.SetCustomProperties(newProperties);
        }

        PhotonNetwork.LoadLevel("Map_01"); // Todo: replace with list of different maps (scenes) and consts
    }

    private bool AllPlayersReady() {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            // If player is host, skip
            if (player.IsLocal)
                continue;

            // If one of the players is not ready
            if (!(bool)player.CustomProperties[PlayerProperties.READY_STATE])
                return false;
        }
        return true;
    }

    /// <summary>
    /// Updates the UI listing, it creates the necessary items not yet listed, update existing items and remove unused entries
    /// </summary>
    public void UpdateUI()
    {
        List<int> processedIDs = new List<int>();

        // Update existing slots and add new ones
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {   
            // Update existing slot
            if (_slots.ContainsKey(player.ID))
            {
                // Found ID, refresh data
                _slots[player.ID].RefreshData(player);

                // Add ID to processed IDs list
                processedIDs.Add(player.ID);

                Debug.Log("Player " + player.NickName + " already has a playerslot, updating existing slot...");
            }
            // Create new slot
            else
            {
                // Instantiate prefab and set data
                GameObject slot = Instantiate(_playerSlotPrefab, _playerSlotHolder);
                slot.transform.localScale = Vector3.one;
                slot.transform.localPosition = new Vector3(slot.transform.localPosition.x, slot.transform.localPosition.y, 0);

                PlayerSlot playerSlot = slot.GetComponent<PlayerSlot>();
                playerSlot.RefreshData(player);

                // Add to dictionary
                _slots.Add(player.ID, playerSlot);

                // Add ID to processed IDs list
                processedIDs.Add(player.ID);

                Debug.Log("Creating a new PlayerSlot for player: " + player.NickName);
            }
        }

        // Remove playerslots of players that are no longer connected to the room
        foreach (var slot in _slots.Reverse())
        {
            if (!processedIDs.Contains(slot.Key))
            {
                _slots.Remove(slot.Key);
                Destroy(slot.Value.gameObject);
            }
        }
    }
    
    /// <summary>
    /// Replace slots dictionary with a new one and remove the remaining player slot objects
    /// </summary>
    public void CleanUpList()
    {
        _slots = new Dictionary<int, PlayerSlot>();
        foreach (RectTransform child in _playerSlotHolder)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion


    #region PHOTON CALLBACKS
    public void OnJoinedRoom()
    {
        Debug.Log("You have joined the room.");
        // - 
    }

    public void OnLeftRoom()
    {
        Debug.Log("You have left the room.");

        // - 
        CleanUpList();
        OpenUIPanel(UIPanelTypes.MainMenu);
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Player [#" + player.ID + "] " + player.NickName + "' connected.");
        UpdateUI();
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Player [#" + player.ID + "] " + player.NickName + "' disconnected.");
        UpdateUI();
    }

    public void OnMasterClientSwitched()
    {
        UpdateUI();
        SetClientUI();
    }

    public void OnPhotonPlayerPropertiesChanged()
    {
        UpdateUI();
    }
    #endregion
}
