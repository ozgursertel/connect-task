using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject howToPlayPanel;
    public GameObject LevelPanel;
    public GameObject MainMenuPanel;

    public void howToPlayPanelOpen()
    {
        MainMenuPanel.SetActive(false);
        LevelPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }
    public void MainMenuOpenPanel()
    {
        MainMenuPanel.SetActive(true);
        LevelPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }
    public void LevelMenuOpenPanel()
    {
        MainMenuPanel.SetActive(false);
        LevelPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }
}
