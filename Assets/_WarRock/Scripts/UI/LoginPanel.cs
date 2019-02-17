using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {

    internal event UnityAction<string, string> SignIn = delegate { };
    internal event UnityAction ClickedCancel = delegate { };
    [SerializeField] private InputField m_UsernameField;
    [SerializeField] private InputField m_PasswordField;
    [SerializeField] private Button m_SignIn;
    [SerializeField] private Button m_Cancel;
    private string m_Username;
    private string m_Password;

    private void OnEnable() {
        m_UsernameField.onValueChanged.AddListener(UpdateUsername);
        m_PasswordField.onValueChanged.AddListener(UpdatePassword);
        m_SignIn.onClick.AddListener(ClickedSignIn);
        m_Cancel.onClick.AddListener(ClickedCancel);
    }

    private void ClickedSignIn() {
        SignIn(m_Username, m_Password);
    }

    private void OnDisable() {
        m_SignIn.onClick.RemoveListener(ClickedSignIn);
        m_Cancel.onClick.RemoveListener(ClickedCancel);
    }

    public void UpdateUsername(string user) {
        m_Username = user;
    }

    public void UpdatePassword(string pass) {
        m_Password = pass;
    }

    // Getters
    public string Username {
        get { return m_Username; }
    }

    public string Password {
        get { return m_Password; }
    }
}
