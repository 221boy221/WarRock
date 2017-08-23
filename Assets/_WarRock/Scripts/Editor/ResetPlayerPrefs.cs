using UnityEditor;
using UnityEngine;


/// <summary>
/// Resets all the saved data in the Unity Player Preferences
/// </summary>
public class ResetPlayerPrefs
{

    #region Methods
    [MenuItem("Utils/Reset All PlayerPrefs")]
    static public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion

}