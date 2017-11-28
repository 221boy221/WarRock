using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoom : UiPanel
{

    #region Vars
    [SerializeField] private GameObject m_PlayerSlotPrefab;
    [SerializeField] private RectTransform m_PlayerSlotHolder;
    [SerializeField] private Button m_ReadyButton;
    [SerializeField] private Button m_StartButton;
    [SerializeField] private Button m_LeaveButton;
    [SerializeField] private Text m_RoomIdLabel;

    private Dictionary<int, PlayerSlot> m_Slots = new Dictionary<int, PlayerSlot>();
    #endregion

    #region Methods
 
    private void OnEnable()
    {
        // Event Listeners
        m_ReadyButton.onClick.AddListener(OnClickedReady);
        m_StartButton.onClick.AddListener(OnClickedStart);
        m_LeaveButton.onClick.AddListener(OnClickedExit);

        // Update UI
        CleanUpList();
        SetClientUI();
        UpdateUI();

        if (PhotonNetwork.room != null) {
            m_RoomIdLabel.text = PhotonNetwork.room.CustomProperties[RoomProperties.ID].ToString();
        }
    }
    private void OnDisable() {
        // Event Listeners
        m_ReadyButton.onClick.RemoveListener(OnClickedReady);
        m_StartButton.onClick.RemoveListener(OnClickedStart);
        m_LeaveButton.onClick.RemoveListener(OnClickedExit);
    }

    /// <summary>
    /// Disconnects the client from the room before switching back to the Main Menu UI Panel.
    /// </summary>
    private void OnClickedExit() {
        PhotonNetwork.LeaveRoom();
        OpenUIPanel(UIPanelTypes.RoomListMenu);
    }


    /// <summary>
    /// Changes the Ready button into a Start button if to the player is a master and the other way around.
    /// </summary>
    private void SetClientUI()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            m_StartButton.gameObject.SetActive(true);
            m_ReadyButton.gameObject.SetActive(false);
        }
        else
        {
            m_StartButton.gameObject.SetActive(false);
            m_ReadyButton.gameObject.SetActive(true);
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

        m_Slots[PhotonNetwork.player.ID].RefreshData(PhotonNetwork.player);
    }

    /// <summary>
    /// On clicked Start button behaviour. Checks the states of all players and starts the game.
    /// </summary>
    private void OnClickedStart()
    {
        if (PhotonNetwork.room.PlayerCount > RoomProperties.MIN_SLOTS)
        {
            // Min slot count reached
            if (AllPlayersReady())
            {
                // All players rdy
                StartGame();
            }
            else
            {
                Debug.Log("Not all players are ready!");
                return;
            }
        }
        else
        {
            Debug.Log("You need at least " + RoomProperties.MIN_SLOTS + " players in order to start!");
            return;
        }
    }

    private void StartGame() {
        Debug.Log("Starting game...");

        // Loading level, reset player ready states so we can use them to check if player is done loading
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            Hashtable newProperties = new Hashtable();
            newProperties[PlayerProperties.LOADING_MAP] = true;
            player.SetCustomProperties(newProperties);
        }

        PhotonNetwork.LoadLevel((int)PhotonNetwork.room.CustomProperties[RoomProperties.MAP_ID]); // Todo: replace with list of different maps (scenes) and consts
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
            if (m_Slots.ContainsKey(player.ID))
            {
                // Found ID, refresh data
                m_Slots[player.ID].RefreshData(player);

                // Add ID to processed IDs list
                processedIDs.Add(player.ID);

                Debug.Log("Player " + player.NickName + " already has a playerslot, updating existing slot...");
            }
            // Create new slot
            else
            {
                // Instantiate prefab and set data
                GameObject slot = Instantiate(m_PlayerSlotPrefab, m_PlayerSlotHolder);
                slot.transform.localScale = Vector3.one;
                slot.transform.localPosition = new Vector3(slot.transform.localPosition.x, slot.transform.localPosition.y, 0);

                PlayerSlot playerSlot = slot.GetComponent<PlayerSlot>();
                playerSlot.RefreshData(player);

                // Add to dictionary
                m_Slots.Add(player.ID, playerSlot);

                // Add ID to processed IDs list
                processedIDs.Add(player.ID);

                Debug.Log("Creating a new PlayerSlot for player: " + player.NickName);
            }
        }

        // Remove playerslots of players that are no longer connected to the room
        foreach (var slot in m_Slots.Reverse())
        {
            if (!processedIDs.Contains(slot.Key))
            {
                m_Slots.Remove(slot.Key);
                Destroy(slot.Value.gameObject);
            }
        }
    }
    
    /// <summary>
    /// Replace slots dictionary with a new one and remove the remaining player slot objects
    /// </summary>
    public void CleanUpList()
    {
        m_Slots = new Dictionary<int, PlayerSlot>();
        foreach (RectTransform child in m_PlayerSlotHolder)
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
        OpenUIPanel(UIPanelTypes.RoomListMenu);
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
