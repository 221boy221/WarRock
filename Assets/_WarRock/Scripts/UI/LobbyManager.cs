using GameSparks.Api.Responses;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{

    #region Vars
    [SerializeField] private UiPanel[] _LobbyPanels;
    [SerializeField] private ServerList m_ServerList;
    [SerializeField] private LoginPanel m_LoginPanel;
    [SerializeField] private Text m_UserId;
    [SerializeField] private Text m_ConnectionStatus;
    #endregion

    #region Methods

    void Start() {
        
        m_UserId.text = "No User Logged In...";
        m_ConnectionStatus.text = "No Connection...";

        //GS.GameSparksAvailable += (isAvailable) => {
        //    if (isAvailable) {
        //        m_ConnectionStatus.text = "GameSparks Connected...";
        //    }
        //    else {
        //        m_ConnectionStatus.text = "GameSparks Disconnected...";
        //    }
        //};
    }

    private void OnEnable() {
        // Add Eventlisteners
        for (int i = 0; i < _LobbyPanels.Length; i++)
            _LobbyPanels[i].OpenUIPanelEvent += SwitchPanelTo;
        m_LoginPanel.SignIn += OnSignIn;

        // Check if the Player is already connected to a Room.
        if (!PhotonNetwork.inRoom) SwitchPanelTo(UIPanelTypes.RoomListMenu);
        else ReconnectWithWaitingRoom();
    }

    private void OnDisable() {
        // Remove Eventlisteners
        for (int i = 0; i < _LobbyPanels.Length; i++)
            _LobbyPanels[i].OpenUIPanelEvent -= SwitchPanelTo;
        m_LoginPanel.SignIn -= OnSignIn;
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

    private void RegisterUser(string _user, string _pass) {
        GameSparksManager.Instance().RegisterUser(
            m_LoginPanel.Username,
            m_LoginPanel.Password,
            OnRegistration
        );
    }

    private void OnSignIn(string _user, string _pass) {
        GameSparksManager.Instance().AuthenticateUser(
            m_LoginPanel.Username, 
            m_LoginPanel.Password, 
            OnAuthentication
        );
    }
    #endregion
    
    #region Callbacks
    /// <summary>
    /// this is called when a player is registered
    /// </summary>
    /// <param name="_resp">Resp.</param>
    private void OnRegistration(RegistrationResponse _resp) {
        m_UserId.text = "User ID: " + _resp.UserId;
        m_ConnectionStatus.text = "New User Registered...";
    }
    /// <summary>
    /// This is called when a player is authenticated
    /// </summary>
    /// <param name="_resp">Resp.</param>
    private void OnAuthentication(AuthenticationResponse _resp) {
        m_UserId.text = "User ID: " + _resp.UserId;
        m_ConnectionStatus.text = "User Authenticated...";

        PhotonNetwork.player.NickName = m_LoginPanel.Username;
        m_LoginPanel.gameObject.SetActive(false);
        m_ServerList.gameObject.SetActive(true);
    }
    #endregion
    #endregion

}
