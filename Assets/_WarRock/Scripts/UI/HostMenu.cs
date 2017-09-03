using UnityEngine;
using UnityEngine.UI;

public class HostMenu : LobbyPanel
{

    #region Vars
    [SerializeField] private Toggle m_PrivateToggle;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private Button m_OkButton;
    [SerializeField] private Button m_CancelButton;
    [SerializeField] private Slider m_SlotSlider;
    [SerializeField] private InputField m_RoomNameInputFd;

    // Data
    private string m_RoomName = "Go, Go, Go!";
    private int m_PlayerSlots = 2;
    private bool m_IsPrivate = false;
    private bool m_IsInvisible = false;
    #endregion

    #region Methods
    private void OnEnable() {
        m_OkButton.onClick.AddListener(CreateRoom);
        m_CancelButton.onClick.AddListener(OnClickedCancel);
        m_RoomNameInputFd.onValueChanged.AddListener(UpdateRoomName);
        m_SlotSlider.onValueChanged.AddListener(UpdatePlayerSlots);
        m_PrivateToggle.onValueChanged.AddListener(UpdateRoomPrivacy);
    }
    private void OnDisable() {
        m_OkButton.onClick.RemoveListener(CreateRoom);
        m_CancelButton.onClick.RemoveListener(OnClickedCancel);
        m_RoomNameInputFd.onValueChanged.RemoveListener(UpdateRoomName);
        m_SlotSlider.onValueChanged.RemoveListener(UpdatePlayerSlots);
        m_PrivateToggle.onValueChanged.RemoveListener(UpdateRoomPrivacy);
    }

    private void UpdateRoomName(string input) {
        m_RoomName = input;
    }

    private void UpdatePlayerSlots(float amountOfSlots) {
        m_PlayerSlots = Mathf.RoundToInt(amountOfSlots);
    }

    private void UpdateRoomPrivacy(bool isPrivate) {
        m_IsPrivate = isPrivate;
    }

    /// <summary>
    /// Opens the UI panel MainMenu.
    /// </summary>
    private void OnClickedCancel() { OpenUIPanel(UIPanelTypes.MainMenu); }

    /// <summary>
    /// Calls the NetworkManager.CreateRoom() with either RoomType.Public or RoomType.Private
    /// </summary>
    private void CreateRoom()
    {
        NetworkManager.Instance.CreateRoom(m_RoomName, m_IsPrivate, m_PlayerSlots, m_IsInvisible);
    }
    
    #endregion

}
