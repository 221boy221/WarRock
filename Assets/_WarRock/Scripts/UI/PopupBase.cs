using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupBase : MonoBehaviour {
    
    protected virtual void AddListeners() {
        //
    }

    protected virtual void RemoveListeners() {
        //
    }

    protected virtual void OnOpen() {
        Debug.Log("PopupBase: OnOpen", this.gameObject);
        AddListeners();
    }

    protected virtual void Close() {
        Debug.Log("PopupBase: Close", this.gameObject);
        RemoveListeners();
        gameObject.SetActive(false);
    }
    
}
