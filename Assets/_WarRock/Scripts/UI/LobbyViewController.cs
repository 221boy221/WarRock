using GameSparks.Api.Responses;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyViewController : Singleton<LobbyViewController> {

    #region Vars
    [SerializeField] private UiPanel[] _LobbyPanels;
    [SerializeField] private ServerList m_ServerList;
    [SerializeField] private LoginPopupComponent m_LoginPanel;
    [SerializeField] private Text m_UserId;
    [SerializeField] private Text m_ConnectionStatus;
    #endregion

    #region Methods

    private void Awake() {
        LoginManager.Instance.AccountAuthenticatedEvent += OnAccountAuthenticatedEvent;
        LoginManager.Instance.AccountRegisteredEvent += OnAccountRegisteredEvent;
    }

    private void OnAccountRegisteredEvent(RegistrationResponse obj) {
        SetNickName(obj.DisplayName);
        SetConnectionStatus("New User Registered...");
    }

    private void OnAccountAuthenticatedEvent(AuthenticationResponse obj) {
        SetNickName(obj.DisplayName);
        SetConnectionStatus("User Authenticated!");
    }

    private void OnDestroy() {
        LoginManager.Instance.AccountAuthenticatedEvent -= OnAccountAuthenticatedEvent;
        LoginManager.Instance.AccountRegisteredEvent -= OnAccountRegisteredEvent;
    }

    void Start() {
        SetConnectionStatus("No Connection...");
        SetNickName("");

        //GS.GameSparksAvailable += (isAvailable) => {
        //    if (isAvailable) {
        //        m_ConnectionStatus.text = "GameSparks Connected...";
        //    }
        //    else {
        //        m_ConnectionStatus.text = "GameSparks Disconnected...";
        //    }
        //};
    }

    internal void SetConnectionStatus(string status) {
        m_ConnectionStatus.text = status;
    }

    internal void SetNickName(string nickname) {
        m_UserId.text = nickname;
    }

    private void OnEnable() {
        // Add Eventlisteners
        for (int i = 0; i < _LobbyPanels.Length; i++)
            _LobbyPanels[i].OpenUIPanelEvent += SwitchPanelTo;

        // Check if the Player is already connected to a Room.
        if (!PhotonNetwork.inRoom) SwitchPanelTo(UIPanelTypes.RoomListMenu);
        else ReconnectWithWaitingRoom();
    }

    private void OnDisable() {
        // Remove Eventlisteners
        for (int i = 0; i < _LobbyPanels.Length; i++)
            _LobbyPanels[i].OpenUIPanelEvent -= SwitchPanelTo;
    }

    

    /// <summary>
    /// Disables all the UI Panels and enables the given one.
    /// </summary>
    /// <param name="panelType"></param>
    private void SwitchPanelTo(UIPanelTypes panelType) {
        // Todo: Improve this, maybe replace uipaneltypes with the actual type
        UiPanel panel = null;
        for (int i = 0; i < _LobbyPanels.Length; i++) {
            if (_LobbyPanels[i].panelType == panelType) {
                panel = _LobbyPanels[i];
                panel.gameObject.SetActive(true);
                break;
            }
        }
        if (panel == null) return;
        if (!panel.isOverlay) {
            for (int i = 0; i < _LobbyPanels.Length; i++) {
                if (_LobbyPanels[i].panelType != panelType && _LobbyPanels[i].panelType != UIPanelTypes.None) {
                    _LobbyPanels[i].gameObject.SetActive(false);
                }
            }
        }
    }
    
    /// <summary>
    /// Switches the UI Panel back towards the Waiting Room since the client is already connected, thus skipping the Join/Host panel.
    /// </summary>
    private void ReconnectWithWaitingRoom() {
        NetworkManager.Instance.ResetPlayerRoomPrefs(false); // Todo: check if necessary
        SwitchPanelTo(UIPanelTypes.WaitingRoomMenu);
    }

    #region Event handlers
    /// <summary>
    /// Photon Callback that gets fired once the client successfully joins his newly created room. Once received, we open the Waiting Room UI panel.
    /// </summary>
    public void OnJoinedRoom() {
        SwitchPanelTo(UIPanelTypes.WaitingRoomMenu);
    }
    
    #endregion
    
    #endregion

}
