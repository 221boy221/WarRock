using System;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    #region Vars
    [SerializeField] private LobbyPanel[] _LobbyPanels;
    #endregion
    
    #region Methods
    private void Awake()
    {
        // Entry
        AddEventListeners();
    }

    private void OnEnable()
    {
        // Check if the Player is already connected to a Room.
        if (!PhotonNetwork.inRoom)
        {
            SwitchPanelTo(UIPanelTypes.MainMenu);
        }
        else
        {
            ReconnectWithWaitingRoom();
        }
    }

    /// <summary>
    /// Switches the UI Panel back towards the Waiting Room since the client is already connected, thus skipping the Join/Host panel.
    /// </summary>
    private void ReconnectWithWaitingRoom()
    {
        NetworkManager.Instance.SetDefaultPlayerProperties(false); // Todo: check if necessary
        SwitchPanelTo(UIPanelTypes.WaitingRoom);
    }

    // Event Listeners //
    private void AddEventListeners()
    {
        for (int i = 0; i < _LobbyPanels.Length; i++)
        {
            _LobbyPanels[i].OpenUIPanelEvent += SwitchPanelTo;
        }
    }

    private void RemoveEventListeners()
    {
        for (int i = 0; i < _LobbyPanels.Length; i++)
        {
            _LobbyPanels[i].OpenUIPanelEvent -= SwitchPanelTo;
        }
    }

    /// <summary>
    /// Disables all the UI Panels and enables the given one.
    /// </summary>
    /// <param name="panelType"></param>
    private void SwitchPanelTo(UIPanelTypes panelType) 
    {
        // Todo: Improve this, maybe replace uipaneltypes with the actual type
        LobbyPanel panel = null;
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
                if (_LobbyPanels[i].panelType != panelType)
                {
                    _LobbyPanels[i].gameObject.SetActive(false);
                }
            }   
        }
    }
    #endregion

}
