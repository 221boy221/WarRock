using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : LobbyPanel
{

    #region Vars
    [SerializeField] private InputField m_NickName;
    [SerializeField] private Button m_ApplyButton;
    [SerializeField] private Button m_OkButton;
    [SerializeField] private Button m_CloseButton;
    public UIPanelTypes panelToOpen;
    #endregion

    #region Methods
    private void Awake()
    {
        m_NickName.onEndEdit.AddListener(ApplyNickName);
        m_ApplyButton.onClick.AddListener(OnClickedApply);
        m_OkButton.onClick.AddListener(OnClickedOk);
        m_CloseButton.onClick.AddListener(OnClickedCancel);
    }

    private void OnClickedApply() {
        // Todo
    }

    private void OnClickedOk() {
        // Todo
    }

    /// <summary>
    /// Opens the UI panel MainMenu.
    /// </summary>
    private void OnClickedCancel() { OpenUIPanel(UIPanelTypes.MainMenu); }

    /// <summary>
    /// Saves the given username from the InputField into the PlayerPrefs.
    /// Also sets the PhotonNetwork.Player.NickName.
    /// </summary>
    /// <param name="input"></param>
    private void ApplyNickName(string input)
    {
        PlayerPrefs.SetString("username", input);
        PhotonNetwork.player.NickName = PlayerPrefs.GetString("username");
    }

    /// <summary>
    /// Fires the event to open the queue'd UI Panel.
    /// </summary>
    private void OkButtonClick()
    {
        OpenUIPanel(panelToOpen);
    }
    #endregion

}
