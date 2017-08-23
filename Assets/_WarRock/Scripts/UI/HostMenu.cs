using System;
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
    private RoomTypes m_RoomType = RoomTypes.Public;
    #endregion

    #region Methods
    private void OnEnable()
    {
        m_OkButton.onClick.AddListener(CreateRoom);
        // todo
        m_CancelButton.onClick.AddListener(OnClickedBack);
        m_RoomNameInputFd.onValueChanged.AddListener(UpdateRoomName);
        m_SlotSlider.onValueChanged.AddListener(UpdatePlayerSlots);
        m_PrivateToggle.onValueChanged.AddListener(UpdateRoomPrivacy);
    }

    private void OnDisable() {
        m_OkButton.onClick.RemoveListener(CreateRoom);
        // todo
        m_CancelButton.onClick.RemoveListener(OnClickedBack);
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
        if (isPrivate)
            m_RoomType = RoomTypes.Private;
        else
            m_RoomType = RoomTypes.Public;
    }

    /// <summary>
    /// Opens the UI panel MainMenu.
    /// </summary>
    private void OnClickedBack() { OpenUIPanel(UIPanelTypes.MainMenu); }

    /// <summary>
    /// Calls the NetworkManager.CreateRoom() with either RoomType.Public or RoomType.Private
    /// </summary>
    private void CreateRoom()
    {
        //PhotonNetwork.custom
        NetworkManager.Instance.CreateRoom(m_RoomType);
    }

    /// <summary>
    /// Calls the NetworkManager.CreateRoom() with RoomType.Private
    /// </summary>
    private void CreatePrivateRoom()
    {
        NetworkManager.Instance.CreateRoom(RoomTypes.Private);
    }

    /// <summary>
    /// Photon Callback that gets fired once the client successfully joins his newly created room. Once received, we open the Waiting Room UI panel.
    /// </summary>
    public void OnJoinedRoom()
    {
        OpenUIPanel(UIPanelTypes.WaitingRoom);
    }
    #endregion

}
