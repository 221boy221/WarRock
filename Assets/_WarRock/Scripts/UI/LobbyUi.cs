using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUi : UiPanel {

    [SerializeField] private Button m_InventoryButton;
    [SerializeField] private Button m_HelpButton;
    [SerializeField] private Button m_SettingsButton;
    [SerializeField] private Button m_QuitButton;

    private void OnEnable() {
        m_InventoryButton.onClick.AddListener(OnClickedInventory);
        m_HelpButton.onClick.AddListener(OnClickedHelp);
        m_SettingsButton.onClick.AddListener(OnClickedSettings);
        m_QuitButton.onClick.AddListener(OnClickedQuit);
    }

    private void OnDisable() {
        m_InventoryButton.onClick.RemoveListener(OnClickedSettings);
        m_HelpButton.onClick.RemoveListener(OnClickedSettings);
        m_SettingsButton.onClick.RemoveListener(OnClickedSettings);
        m_QuitButton.onClick.RemoveListener(OnClickedSettings);
    }

    private void OnClickedQuit() {
        // Todo: Open confirmation popup (give callback of App.Quit)
        Application.Quit();
    }

    private void OnClickedHelp() {
        // Todo: Open help popup
    }

    private void OnClickedInventory() {
        // Todo: Open Inventory panel
    }

    private void OnClickedSettings() {
        OpenUIPanel(UIPanelTypes.SettingsPanel);
    }
    
}
