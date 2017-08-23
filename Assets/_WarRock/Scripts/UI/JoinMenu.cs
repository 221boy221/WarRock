using UnityEngine;
using UnityEngine.UI;

public class JoinMenu : LobbyPanel
{

    #region Vars
    [SerializeField] private Button _joinTokenButton;
    [SerializeField] private Button _joinRandomButton;
    [SerializeField] private InputField _inputField;
    [SerializeField] private Button _backButton;
    private int _token = 0;
    #endregion

    #region Methods
    private void Awake()
    {
        _joinTokenButton.onClick.AddListener(JoinRoomWithToken);
        _joinRandomButton.onClick.AddListener(JoinRandomRoom);
        _inputField.onEndEdit.AddListener(OnEndEdit);
        _backButton.onClick.AddListener(OnClickedBack);
    }

    /// <summary>
    /// Opens the UI panel MainMenu.
    /// </summary>
    private void OnClickedBack() { OpenUIPanel(UIPanelTypes.MainMenu); }

    /// <summary>
    /// Saves the user-inserted token from the InputField into the private token variable.
    /// </summary>
    /// <param name="inputString"></param>
    private void OnEndEdit(string inputString)
    {
        // Check if token is Int only
        bool isValidToken = int.TryParse(inputString, out _token);

        if (!isValidToken)
        {
            _token = -1;
        }
    }

    /// <summary>
    /// Checks if the token value type is valid and calls the NetworkManager.JoinRoomWithToken().
    /// </summary>
    private void JoinRoomWithToken()
    {
        if (_token <= 0)
        {
            return;
        }

        NetworkManager.Instance.JoinRoomWithToken(_token);
    }

    /// <summary>
    /// Calls the NetworkManager.JoinRandomRoom().
    /// </summary>
    private void JoinRandomRoom()
    {
        NetworkManager.Instance.JoinRandomRoom();
    }

    /// <summary>
    /// Photon Callback that gets fired once the client successfully joins his selected room. Once received, we open the Waiting Room UI panel.
    /// </summary>
    public void OnJoinedRoom()
    {
        OpenUIPanel(UIPanelTypes.WaitingRoom);
    }
    #endregion

}
