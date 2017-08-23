using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FlorisTestButton : MonoBehaviour, IPointerClickHandler {

    public event UnityAction OnClickedHitbox = delegate { };

    public void OnPointerClick(PointerEventData eventData) {
        OnClickedHitbox();
    }
    
}
