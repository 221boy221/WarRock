using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : LobbyPanel
{

    #region Vars
    [SerializeField] private InputField _nickName;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _backButton;
    public UIPanelTypes panelToOpen;
    #endregion

    #region Methods
    private void Awake()
    {
        _nickName.onEndEdit.AddListener(ApplyNickName);
        _okButton.onClick.AddListener(OnClickedBack);
        _backButton.onClick.AddListener(OnClickedBack);
    }

    /// <summary>
    /// Opens the UI panel MainMenu.
    /// </summary>
    private void OnClickedBack() { OpenUIPanel(UIPanelTypes.MainMenu); }

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
