using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : UiPanel
{

    #region Vars
    [SerializeField] private Button m_ApplyButton;
    [SerializeField] private Button m_OkButton;
    [SerializeField] private Button m_CloseButton;
    #endregion

    #region Methods
    private void Awake()
    {
        m_ApplyButton.onClick.AddListener(OnClickedApply);
        m_OkButton.onClick.AddListener(OnClickedOk);
        m_CloseButton.onClick.AddListener(OnClickedCancel);
    }

    private void OnClickedApply() {
        // Todo: Apply
    }

    private void OnClickedOk() {
        // Todo: Apply
        // Todo: Close overlay
    }

    /// <summary>
    /// Opens the UI panel MainMenu.
    /// </summary>
    private void OnClickedCancel() {
        // Todo: Close overlay
    }

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
    #endregion

}
