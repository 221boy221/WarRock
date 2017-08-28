using UnityEngine;
using UnityEngine.UI;

public class MainMenu : LobbyPanel
{

    #region Vars
    [SerializeField] private Button _settingsButton;
    [SerializeField] private InputField _inputField;
    [SerializeField] private Button _joinTokenButton;
    [SerializeField] private Button _joinRandomButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private SettingsMenu _settingsPanel;
    [SerializeField] private Button _backButton;
    private int _token = 0;
    #endregion

    #region Methods
    private void Awake()
    {
        _inputField.onEndEdit.AddListener(OnEndEdit);
        _joinTokenButton.onClick.AddListener(OnClickedJoinWithToken);
        _joinRandomButton.onClick.AddListener(OnClickedJoinRandomRoom);
        _hostButton.onClick.AddListener(OnClickedHost);

        _settingsButton.onClick.AddListener(OnClickedSettings);
        _backButton.onClick.AddListener(OnClickedBack);
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
            _settingsPanel.panelToOpen = clickedPanel;
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
        bool isValidToken = int.TryParse(inputString, out _token);

        if (!isValidToken) {
            _token = -1;
        }
    }

    /// <summary>
    /// Checks if the token value type is valid and calls the NetworkManager.JoinRoomWithToken().
    /// </summary>
    private void OnClickedJoinWithToken() {
        if (_token <= 0)
            return;

        NetworkManager.Instance.JoinRoomWithId(_token);
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
