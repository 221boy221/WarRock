using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {

    internal event UnityAction OnClickedSignIn = delegate { };
    internal event UnityAction OnClickedCancel = delegate { };
    [SerializeField] private InputField m_UsernameField;
    [SerializeField] private InputField m_PasswordField;
    [SerializeField] private Button m_SignIn;
    [SerializeField] private Button m_Cancel;
    private string m_Username;
    private string m_Password;

    private void OnEnable() {
        m_UsernameField.onValueChanged.AddListener(UpdateUsername);
        m_PasswordField.onValueChanged.AddListener(UpdatePassword);
        m_SignIn.onClick.AddListener(OnClickedSignIn);
        m_Cancel.onClick.AddListener(OnClickedCancel);
    }

    private void OnDisable() {
        m_SignIn.onClick.RemoveListener(OnClickedSignIn);
        m_Cancel.onClick.RemoveListener(OnClickedCancel);
    }

    public void UpdateUsername(string user) {
        m_Username = user;
    }

    public void UpdatePassword(string pass) {
        m_Password = pass;
    }
    
    // Getters
    public string Username
    {
        get { return m_Username; }
    }

    public string Password
    {
        get { return m_Password; }
    }
}
