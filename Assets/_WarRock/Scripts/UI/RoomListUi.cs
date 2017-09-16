using UnityEngine;
using UnityEngine.UI;

public class RoomListUi : UiPanel
{

    #region Vars
    [SerializeField] private InputField m_InputField;
    [SerializeField] private Button m_JoinRoomIdButton;
    [SerializeField] private Button m_JoinRandomButton;
    [SerializeField] private Button m_HostRoomButton;
    [SerializeField] private Button m_ExitButton;
    private int m_RoomId = 0;
    #endregion

    #region Methods
    private void OnEnable() {
        m_InputField.onEndEdit.AddListener(OnEndEdit);
        m_JoinRoomIdButton.onClick.AddListener(OnClickedJoinWithToken);
        m_JoinRandomButton.onClick.AddListener(OnClickedJoinRandomRoom);
        m_HostRoomButton.onClick.AddListener(OnClickedCreateRoom);
        m_ExitButton.onClick.AddListener(OnClickedExit);
    }

    private void OnDisable() {
        m_InputField.onEndEdit.RemoveListener(OnEndEdit);
        m_JoinRoomIdButton.onClick.RemoveListener(OnClickedJoinWithToken);
        m_JoinRandomButton.onClick.RemoveListener(OnClickedJoinRandomRoom);
        m_HostRoomButton.onClick.RemoveListener(OnClickedCreateRoom);
        m_ExitButton.onClick.RemoveListener(OnClickedExit);
    }

    /// <summary>
    /// Saves the user-inserted token from the InputField into the private token variable.
    /// </summary>
    /// <param name="inputString"></param>
    private void OnEndEdit(string inputString) {
        // Check if token is Int only
        bool isValidToken = int.TryParse(inputString, out m_RoomId);

        if (!isValidToken)
            m_RoomId = -1;
    }

    private void OnClickedCreateRoom() {
        OpenUIPanel(UIPanelTypes.CreateRoomPanel);
    }
    
    /// <summary>
    /// Checks if the token value type is valid and calls the NetworkManager.JoinRoomWithToken().
    /// </summary>
    private void OnClickedJoinWithToken() {
        if (m_RoomId < 0)
            return;

        NetworkManager.Instance.JoinRoomWithId(m_RoomId);
    }

    /// <summary>
    /// Calls the NetworkManager.JoinRandomRoom().
    /// </summary>
    private void OnClickedJoinRandomRoom() {
        NetworkManager.Instance.JoinRandomRoom();
    }

    private void OnClickedExit() {
        // Todo: Add logout
    }

    /// <summary>
    /// Photon Callback that gets fired once the client successfully joins his selected room. Once received, we open the Waiting Room UI panel.
    /// </summary>
    public void OnJoinedRoom() {
        OpenUIPanel(UIPanelTypes.WaitingRoomMenu);
    }
    #endregion

}
