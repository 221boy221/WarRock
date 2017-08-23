using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerSlot : MonoBehaviour
{

    [SerializeField] private Button _connectButton;
    [SerializeField] private Text _serverNameText;
    [SerializeField] private Text _pingText;

    private int _ping = 999;
    private uint _regionCode;

    private void OnEnable()
    {
        _connectButton.onClick.AddListener(OnClickedServer);
    }

    private void OnClickedServer()
    {
        ServerManager.Instance.JoinServer(_regionCode);
    }

    private void OnDisable()
    {
        _connectButton.onClick.RemoveAllListeners();
    }


    #region Getters and Setters

    public string ServerName
    {
        get { return _serverNameText.text; }
        set { _serverNameText.text = value; }
    }

    public int Ping
    {
        get { return _ping; }
        set {
            _ping = value;
            _pingText.text = _ping.ToString();
        }
    }

    public uint RegionCode
    {
        set {
            _regionCode = value;
        }
    }

    #endregion

}
