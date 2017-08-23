using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Todo
public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance;
    [SerializeField] private Sprite[] m_LvlSprites;
    public int lvl = 1;
    
    private void Awake() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);

        if (m_LvlSprites == null)
        {
            Debug.LogError("Missing lvl atlas");
        }
    }

    public Sprite GetLvlImg() {
        return m_LvlSprites[lvl - 1];
    }

}
