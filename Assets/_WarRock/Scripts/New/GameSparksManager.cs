using GameSparks.Api.Responses;
using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSparksManager : Singleton<GameSparksManager> {



    private void Awake() {

    }

    //todo
    internal IRTSession GetRTSession() {
        return null;
    }

    private void OnRTReady(bool isReady) {
        Debug.Log("GSM| RT Session Connected...");
    }

    private void OnPacketReceived(RTPacket packet) {
        switch (packet.OpCode) {
            // op-code 1 refers to any chat-messages being received by a player //
            case 1:
                //if (chatManager == null) { // if the chat manager is not yet set up, then assign the reference in the scene
                //    chatManager = GameObject.Find("Chat Manager").GetComponent<ChatManager>();
                //}
                //chatManager.OnMessageReceived(_packet); // send the whole packet to the chat-manager
                break;

            case 2:
                GameController.Instance.UpdateOpponentPlayers(packet);
                break;

            case 5:
                GameController.Instance.RegisterOpponentHit(packet);
                break;

            case 100:
                // MatchSetup
                Debug.Log("GSM| Loading Level...");
                SceneManager.LoadScene("DefaultMatchScene");
                break;
        }
    }

    #region Login & Registration
    public delegate void AuthCallback(AuthenticationResponse response);
    public delegate void RegCallback(RegistrationResponse response);

    /// <summary>
    /// Sends a registration request to GS.
    /// </summary>
    public void RegisterUser(string userName, string pass, RegCallback regcallback) {
        new GameSparks.Api.Requests.RegistrationRequest()
                  .SetDisplayName(userName)
                  .SetUserName(userName)
                  .SetPassword(pass)
                  .Send((regResp) => {
                      if (!regResp.HasErrors) {
                          Debug.Log("GSM| Registration Successful...");
                          regcallback(regResp);
                      }
                      else {
                          Debug.Log("GSM| Registration Failed...");
                      }
                  });
    }

    /// <summary>
    /// Sends an authentication request to GS.
    /// </summary>
    public void AuthenticateUser(string userName, string pass, AuthCallback authcallback) {
        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(userName)
            .SetPassword(pass)
            .Send((response) => {
                if (!response.HasErrors) {
                    Debug.Log("Authentication Successful...");
                    authcallback(response);
                }
                else {
                    Debug.LogWarning("GSM| Error Authenticating User[" + userName + "] \n" + response.Errors.JSON);
                }
            });
    }
    #endregion

}
