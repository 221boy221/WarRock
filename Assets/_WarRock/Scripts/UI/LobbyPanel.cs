using UnityEngine;
using UnityEngine.Events;

public class LobbyPanel : MonoBehaviour
{
    #region Vars
    internal event UnityAction<UIPanelTypes> OpenUIPanelEvent = delegate { };
    [SerializeField] internal UIPanelTypes panelType;
    public bool isOverlay;
    protected RectTransform rectTransform;
    #endregion

    #region Methods
    // Use this for initialization
    void Awake ()
    {
        rectTransform = this.GetComponent<RectTransform>();
	}
	
    /// <summary>
    /// Fires the OpenUIPanelEvent to which the UIManager listens in order to switch UI Panels.
    /// </summary>
    /// <param name="panel"></param>
    protected void OpenUIPanel(UIPanelTypes panel) 
    {
        OpenUIPanelEvent(panel);
    }
    #endregion
}
