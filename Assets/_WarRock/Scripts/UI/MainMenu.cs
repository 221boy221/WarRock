using UnityEngine;
using UnityEngine.UI;

public class MainMenu : LobbyPanel
{

    #region Vars
    [SerializeField] private InputField m_InputField;
    [SerializeField] private Button m_SettingsButton; // Maybe have this in some Lobby.cs
    [SerializeField] private Button m_JoinRoomIdButton;
    [SerializeField] private Button m_JoinRandomButton;
    [SerializeField] private Button m_HostRoomButton;
    [SerializeField] private SettingsMenu m_SettingsPanel;
    [SerializeField] private Button m_ExitButton;
    private int m_RoomId = 0;
    #endregion

    #region Methods
    private void Awake()
    {
        m_InputField.onEndEdit.AddListener(OnEndEdit);
        m_JoinRoomIdButton.onClick.AddListener(OnClickedJoinWithToken);
        m_JoinRandomButton.onClick.AddListener(OnClickedJoinRandomRoom);
        m_HostRoomButton.onClick.AddListener(OnClickedHost);

        m_SettingsButton.onClick.AddListener(OnClickedSettings);
        m_ExitButton.onClick.AddListener(OnClickedBack);
    }

    // Button listeners
    private void OnClickedJoin() { OnClickedAnyButton(UIPanelTypes.JoinMenu); }
    private void OnClickedHost() { OnClickedAnyButton(UIPanelTypes.HostMenu); }
    private void OnClickedSettings() { OpenUIPanel(UIPanelTypes.SettingsMenu); }
    private void OnClickedBack() { /* OpenUIPanel(UIPanelTypes.LoginMenu); */ } // Todo: Add logout

    /// <summary>
    /// Checks if the client has set its username yet before opening the given UI panel.
    /// If the username is not set, it opens the Settings Menu first.
    /// </summary>
    /// <param name="clickedPanel"></param>
    private void OnClickedAnyButton(UIPanelTypes clickedPanel)
    {
        if (PhotonNetwork.player.NickName == "")
        {
            Debug.Log("Player nickname is: " + PhotonNetwork.player.NickName);
            m_SettingsPanel.panelToOpen = clickedPanel;
            OpenUIPanel(UIPanelTypes.SettingsMenu);
        }
        else
        {
            OpenUIPanel(clickedPanel);
        }
    }
    
    /// <summary>
    /// Saves the user-inserted token from the InputField into the private token variable.
    /// </summary>
    /// <param name="inputString"></param>
    private void OnEndEdit(string inputString) {
        // Check if token is Int only
        bool isValidToken = int.TryParse(inputString, out m_RoomId);

        if (!isValidToken) {
            m_RoomId = -1;
        }
    }

    /// <summary>
    /// Checks if the token value type is valid and calls the NetworkManager.JoinRoomWithToken().
    /// </summary>
    private void OnClickedJoinWithToken() {
        if (m_RoomId <= 0)
            return;

        NetworkManager.Instance.JoinRoomWithId(m_RoomId);
    }

    /// <summary>
    /// Calls the NetworkManager.JoinRandomRoom().
    /// </summary>
    private void OnClickedJoinRandomRoom() {
        NetworkManager.Instance.JoinRandomRoom();
    }

    /// <summary>
    /// Photon Callback that gets fired once the client successfully joins his selected room. Once received, we open the Waiting Room UI panel.
    /// </summary>
    public void OnJoinedRoom() {
        OpenUIPanel(UIPanelTypes.WaitingRoom);
    }
    #endregion

}
