using System;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    #region Vars
    [SerializeField] private UiPanel[] _LobbyPanels;
    #endregion
    
    #region Methods

    private void OnEnable()
    {
        for (int i = 0; i < _LobbyPanels.Length; i++)
            _LobbyPanels[i].OpenUIPanelEvent += SwitchPanelTo;

        // Check if the Player is already connected to a Room.
        if (!PhotonNetwork.inRoom)
            SwitchPanelTo(UIPanelTypes.RoomListMenu);
        else
            ReconnectWithWaitingRoom();
    }

    private void OnDisable() {
        for (int i = 0; i < _LobbyPanels.Length; i++)
            _LobbyPanels[i].OpenUIPanelEvent -= SwitchPanelTo;
    }


    /// <summary>
    /// Switches the UI Panel back towards the Waiting Room since the client is already connected, thus skipping the Join/Host panel.
    /// </summary>
    private void ReconnectWithWaitingRoom()
    {
        NetworkManager.Instance.ResetPlayerRoomPrefs(false); // Todo: check if necessary
        SwitchPanelTo(UIPanelTypes.WaitingRoomMenu);
    }

    /// <summary>
    /// Disables all the UI Panels and enables the given one.
    /// </summary>
    /// <param name="panelType"></param>
    private void SwitchPanelTo(UIPanelTypes panelType) 
    {
        // Todo: Improve this, maybe replace uipaneltypes with the actual type
        UiPanel panel = null;
        for (int i = 0; i < _LobbyPanels.Length; i++)
        {
            if (_LobbyPanels[i].panelType == panelType)
            {
                panel = _LobbyPanels[i];
                panel.gameObject.SetActive(true);
                break;
            }
        }

        if (!panel.isOverlay)
        {
            for (int i = 0; i < _LobbyPanels.Length; i++)
            {
                if (_LobbyPanels[i].panelType != panelType && _LobbyPanels[i].panelType != UIPanelTypes.None)
                {
                    _LobbyPanels[i].gameObject.SetActive(false);
                }
            }   
        }
    }

    /// <summary>
    /// Photon Callback that gets fired once the client successfully joins his newly created room. Once received, we open the Waiting Room UI panel.
    /// </summary>
    public void OnJoinedRoom() {
        SwitchPanelTo(UIPanelTypes.WaitingRoomMenu);
    }
    #endregion

}
