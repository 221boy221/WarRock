using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

public class LoginManager : MonoBehaviour {

    [SerializeField] private ServerList m_ServerList;
    [SerializeField] private LoginPanel m_LoginPanel;

    private void OnEnable() {
        m_LoginPanel.OnClickedSignIn += OnClickedSignIn;
    }

    private void OnDisable() {
        m_LoginPanel.OnClickedSignIn -= OnClickedSignIn;
    }

    private void CheckIfExists() {

    }
    
    private void RegisterPlayer() { 
        new RegistrationRequest()
            .SetDisplayName("Randy")
            .SetPassword("test_password_123456")
            .SetUserName("Test User 1")
            .Send((response) => {
                  if (!response.HasErrors) {
                      Debug.Log("Player Registered");
                  }
                  else {
                      Debug.Log("Error Registering Player");
                  }
              });
    }

    private void OnClickedSignIn() {
        new AuthenticationRequest()
            .SetUserName(m_LoginPanel.Username)
            .SetPassword(m_LoginPanel.Password)
            .Send((response) => {
                if (!response.HasErrors) {
                    Debug.Log("Player authenticated");
                    OnSignedIn();
                }
                else {
                    Debug.Log("Error authenticating Player");
                }
            });
    }

    private void OnSignedIn() {
        Debug.Log("User: " + m_LoginPanel.Username);

        PhotonNetwork.player.NickName = m_LoginPanel.Username;

        m_LoginPanel.gameObject.SetActive(false);
        m_ServerList.gameObject.SetActive(true);
    }

}
