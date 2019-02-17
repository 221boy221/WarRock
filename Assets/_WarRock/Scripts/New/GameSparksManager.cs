using GameSparks.Api.Responses;
using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSparksManager : MonoBehaviour {

    private static GameSparksManager instance = null;
    public static GameSparksManager Instance() {
        if (instance != null) {
            return instance;
        }
        else {
            Debug.LogError("GSM| GameSparksManager Not Initialized...");
        }
        return null;
    }

    void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnPacketReceived(RTPacket _packet) {

        switch (_packet.OpCode) {
            // op-code 1 refers to any chat-messages being received by a player //
            // from here, we will send them to the chat-manager //
            case 1:
                //if (chatManager == null) { // if the chat manager is not yet set up, then assign the reference in the scene
                //    chatManager = GameObject.Find("Chat Manager").GetComponent<ChatManager>();
                //}
                //chatManager.OnMessageReceived(_packet); // send the whole packet to the chat-manager
                break;

            case 2:

                GameController.Instance().UpdateOpponentPlayers(_packet);
                break;

            case 5:
                GameController.Instance().RegisterOpponentHit(_packet);
                break;

        }
    }

    #region Login & Registration
    public delegate void AuthCallback(AuthenticationResponse _authresp2);
    public delegate void RegCallback(RegistrationResponse _authResp);

    public void RegisterUser(string _userName, string _password, RegCallback _regcallback) {
        new GameSparks.Api.Requests.RegistrationRequest()
                  // this login method first attempts a registration //
                  // if the player is not new, we will be able to tell as the registrationResponse has a bool 'NewPlayer' which we can check
                  // for this example we use the user-name was the display name also //
                  .SetDisplayName(_userName)
                  .SetUserName(_userName)
                  .SetPassword(_password)
                  .Send((regResp) => {
                      if (!regResp.HasErrors) { // if we get the response back with no errors then the registration was successful
                          Debug.Log("GSM| Registration Successful...");
                          _regcallback(regResp);
                      }
                  });
    }

    /// <summary>
    /// Sends an authentication request or registration request to GS.
    /// </summary>
    /// <param name="_callback1">Auth-Response</param>
    /// <param name="_callback2">Registration-Response</param>
    public void AuthenticateUser(string _userName, string _password, AuthCallback _authcallback) {
        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(_userName)
            .SetPassword(_password)
            .Send((authResp) => {
                if (!authResp.HasErrors) {
                    Debug.Log("Authentication Successful...");
                    _authcallback(authResp);
                }
                else {
                    Debug.LogWarning("GSM| Error Authenticating User \n" + authResp.Errors.JSON);
                }
            });
    }
    #endregion

}
