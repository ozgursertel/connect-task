using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
     #region Singleton
    private static CanvasManager ınstance;

    public static CanvasManager Instance { get => ınstance; set => ınstance = value; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion


    public GameObject LevelComplatedPanel;
    public GameObject InGamePanel;
    public GameObject LevelFailedPanel;
    public GameObject ReshufflePanel;
    public GameObject LevelComplatedWithHighScorePanel;


    public void LevelComplated()
    {
        InGamePanel.SetActive(false);
        LevelComplatedPanel.SetActive(true);
    }

    public void LevelFailed()
    {
        InGamePanel.SetActive(false);
        LevelFailedPanel.SetActive(true);
    }

    public IEnumerator ReshuffleAnimation()
    {
        ReshufflePanel.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        ReshufflePanel.SetActive(false);
    }

    public void LevelComplatedWithHighScore(int highScore,TMP_Text highScoreText)
    {
        InGamePanel.SetActive(false);
        LevelComplatedWithHighScorePanel.SetActive(true);
        highScoreText.text = highScore.ToString();

    }
}
