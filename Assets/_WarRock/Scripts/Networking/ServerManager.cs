using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.Events;

public class ServerManager : PunBehaviour
{

    public static ServerManager Instance;
    public event UnityAction OnConnectedToServer = delegate { };

    [SerializeField] private ServerList _serverList;
    private CloudRegionCode _activeRegion;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void LoadServerList()
    {
        StartCoroutine(GetServerList());
    }

    private IEnumerator GetServerList()
    {
        yield return StartCoroutine(ConnectToNameServer());
        yield return StartCoroutine(RefreshServerList());
        yield break;
    }

    private IEnumerator ConnectToNameServer()
    {
        Debug.Log("> Waiting for connection...");
        
        PhotonNetwork.networkingPeer.AppId = PhotonNetwork.PhotonServerSettings.AppID;
        yield return new WaitUntil(() => PhotonNetwork.networkingPeer.ConnectToNameServer() == true);
        yield return new WaitUntil(() => PhotonNetwork.networkingPeer.State == ClientState.ConnectedToNameServer);

        Debug.Log("> Connection Established!");

        yield break;
    }

    public IEnumerator RefreshServerList()
    {
        // Get list of Available Regions
        // Debug.Log("Getting list of available regions...");
        // yield return new WaitUntil(() => PhotonNetwork.networkingPeer.OpGetRegions(PhotonNetwork.networkingPeer.AppId) == true);
        // Debug.Log("Done.");
        Debug.Log("Pinging available regions...");
        yield return PhotonHandler.SP.PingAvailableRegionsCoroutine(false);
        Debug.Log("Done.");

        if (PhotonNetwork.networkingPeer.AvailableRegions != null && PhotonNetwork.networkingPeer.AvailableRegions.Count > 0)
        {
            _serverList.Refresh();
        }

        yield break;
    }

    public void JoinServer(uint region)
    {
        //if (PhotonNetwork.networkingPeer.State == ClientState.ConnectedToNameServer)
        //    PhotonNetwork.networkingPeer.Disconnect();

        _activeRegion = (CloudRegionCode)region;
        PhotonNetwork.networkingPeer.ConnectToRegionMaster((CloudRegionCode)region);

        //PhotonNetwork.ConnectToRegion((CloudRegionCode)region, GameSettings.ClientVersion);
    }

    // On Connected to a Region
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to the Master Server on Region " + _activeRegion.ToString());

        OnConnectedToServer();
    }

    // On Connected to the Photon Nameserver by AppID
    public override void OnConnectedToPhoton() {
        base.OnConnectedToPhoton();
        Debug.Log("Connected to Photon NameServer.");
    }

    public override void OnConnectionFail(DisconnectCause cause) {
        base.OnConnectionFail(cause);
        Debug.LogError(cause);
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause) {
        base.OnFailedToConnectToPhoton(cause);
        Debug.LogError(cause);
    }

    public CloudRegionCode CurrentRegion
    {
        get { return _activeRegion; }
    }
}
