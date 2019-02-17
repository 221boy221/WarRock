using GameSparks.Api.Responses;
using System;
using UnityEngine;

public class LoginManager : Singleton<LoginManager> {

    internal Action<RegistrationResponse> AccountRegisteredEvent;
    internal Action<AuthenticationResponse> AccountAuthenticatedEvent;

    internal void SignIn(string user, string pass) {
        GameSparksManager.Instance().AuthenticateUser(user, pass, OnAuthentication);
    }

    internal void RegisterUser(string user, string pass) {
        GameSparksManager.Instance().RegisterUser(user, pass, OnRegistration);
    }

    #region Callbacks

    /// <summary>
    /// this is called when a player is registered
    /// </summary>
    /// <param name="response">Resp.</param>
    private void OnRegistration(RegistrationResponse response) {
        if (response == null) {
            Debug.Log("registration response error");
            return;
        }

        if (AccountRegisteredEvent != null)
            AccountRegisteredEvent(response);
    }

    /// <summary>
    /// This is called when a player is authenticated
    /// </summary>
    /// <param name="response">Resp.</param>
    private void OnAuthentication(AuthenticationResponse response) {
        if (response == null) {
            Debug.Log("registration response error!");
            return;
        }
        
        if (AccountAuthenticatedEvent != null)
            AccountAuthenticatedEvent(response);

        //for now we do it here:
        PhotonNetwork.player.NickName = response.DisplayName;
    }

    #endregion

}
