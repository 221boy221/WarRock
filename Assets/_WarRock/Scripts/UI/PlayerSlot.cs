using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{

    #region Vars
    [SerializeField] private Text _username;
    [SerializeField] private Text _readyState;
    [SerializeField] private Button _kickButton;
    [SerializeField] private Image _imgComp;
    private PhotonPlayer _player;
    #endregion

    #region Methods
    public void Awake()
    {
        _kickButton.onClick.AddListener(OnClickKick);
    }

    /// <summary>
    /// Fired when the MasterClient clicks Kick on this PlayerSlot.
    /// Calls the NetworkManager to kick the attached player.
    /// </summary>
    private void OnClickKick()
    {
        if (_player != null)
        {
            NetworkManager.Instance.KickPlayer(_player);
        }
    }

    /// <summary>
    /// Attaches, refreshes and sets all the PhotonPlayer data into the UI PlayerSlot prefab.
    /// </summary>
    /// <param name="player"></param>
    public void RefreshData(PhotonPlayer player)
    {
        // Update Player
        if (_player == null)
        {
            _player = player;
        }

        // Update Nickname
        if (player.NickName != "")
        {
            _username.text = player.NickName;
        } 
        else
        {
            _username.text = "Player #" + player.ID;
        }

        // Update Kick Button
        if (player.IsMasterClient) // If player of this playerSlot is room master
        {
            _kickButton.gameObject.SetActive(false);
        }
        else if (PhotonNetwork.player.IsMasterClient) // If player of this client is room master
        {
            _kickButton.gameObject.SetActive(true);
        }
        else
        {
            _kickButton.gameObject.SetActive(false);
        }

        // Update Ready State
        if (player.IsMasterClient)
        {
            _readyState.text = "Master";
        }
        else if ((bool)player.CustomProperties[PlayerProperties.READY_STATE])
        {
            _readyState.text = "Ready";
        }
        else
        {
            _readyState.text = "";
        }

        // Todo : fix null error!
        Debug.Log("Lvl: " + player.CustomProperties[PlayerProperties.LVL]);
        byte  lvl = (byte)player.CustomProperties[PlayerProperties.LVL];
        _imgComp.sprite = PlayerManager.Instance.GetImgForLvl(lvl);
    }
    #endregion

}
