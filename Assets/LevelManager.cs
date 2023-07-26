using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public Sprite lockImage;
    public Sprite unlockImage;
    public Button[] buttons;


    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
            buttons[i].GetComponent<Image>().sprite = lockImage;
            buttons[i].GetComponentInChildren<TMP_Text>().enabled = false;
        }
        for(int i = 0;i<unlockedLevel; i++)
        {
            buttons[i].interactable = true;
            buttons[i].GetComponent<Image>().sprite = unlockImage;
            buttons[i].GetComponentInChildren<TMP_Text>().enabled = true;

        }
    }
    public void OpenLevel(int id)
    {
        string levelName = "Level " + id;
        SceneManager.LoadScene(levelName);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
