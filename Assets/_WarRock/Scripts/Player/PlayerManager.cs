using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance;
    [SerializeField] private Sprite[] m_LvlSprites;
    private int m_Lvl;
    
    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);

        if (m_LvlSprites == null)
            Debug.LogError("Missing lvl atlas");

        // Placeholder
        m_Lvl = Random.Range(1, 50);

        LoadCustomProperties();
    }

    private void LoadCustomProperties() {
        Debug.Log("LoadCustomProperties!");
        ExitGames.Client.Photon.Hashtable currentHash = PhotonNetwork.player.CustomProperties;
        foreach(var i in currentHash)
        {
            Debug.Log("Key: " + i.Key.ToString() + " | Value: " +  i.ToString());
        }


        // Todo, get values from mysql

        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable
        {
            { PlayerProperties.READY_STATE, false },
            { PlayerProperties.LVL, (byte)m_Lvl },
            { PlayerProperties.RATIO_KILLS, 0 },
            { PlayerProperties.RATIO_DEATHS, 0 },
            { PlayerProperties.LOADING_MAP, 0 }
        };
        PhotonNetwork.player.SetCustomProperties(ht);
    }

    public Sprite GetLvlImg() {
        return m_LvlSprites[m_Lvl - 1];
    }

    public Sprite GetImgForLvl(byte i) {
        return m_LvlSprites[i - 1];
    }

}
