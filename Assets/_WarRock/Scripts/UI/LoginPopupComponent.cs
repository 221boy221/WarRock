using UnityEngine;
using UnityEngine.UI;

public class LoginPopupComponent : PopupBase {

    [SerializeField] private InputField usernameField;
    [SerializeField] private InputField passwordField;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button cancelButton;

    public string Username { get; set; }
    public string Password { get; set; }


    protected override void OnOpen() {
        AddListeners();
        base.OnOpen();
    }

    public void ClickedSignIn() {
        LoginManager.Instance.SignIn(Username, Password);
    }

    public void ClickedCancel() {
        Close();
    }

    protected override void Close() {
        RemoveListeners();
        base.Close();
    }

}
