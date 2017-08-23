using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    [SerializeField] private ServerList m_ServerList;
    [SerializeField] private LoginPanel m_LoginPanel;

    private void OnEnable() {
        m_LoginPanel.OnClickedSignIn += OnClickedSignIn;
    }

    private void OnDisable() {
        m_LoginPanel.OnClickedSignIn -= OnClickedSignIn;
    }

    private void OnClickedSignIn() {
        // Todo: Add custom authentication system.
        OnSignedIn();
    }

    private void OnSignedIn() {
        Debug.Log("User: " + m_LoginPanel.Username);

        PhotonNetwork.player.NickName = m_LoginPanel.Username; // Todo: replace with login username

        m_LoginPanel.gameObject.SetActive(false);
        m_ServerList.gameObject.SetActive(true);
    }

}
