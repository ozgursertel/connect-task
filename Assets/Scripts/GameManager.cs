using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Singleton
    private static GameManager ınstance;

    public static GameManager Instance { get => ınstance; set => ınstance = value; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

   

    [Header("Check Game Validty")]
    public bool isLevelComplated;
    public bool isLevelFailed;

    private int levelHighScore;

    [Header ("Point")]
    private int point;
    public TMP_Text scoreText;
    public TMP_Text inGame_highScoreText;
    public TMP_Text endGame_highScoreText;

    [System.Serializable]
    public class CollectedItem
    {
        public string tag;
        public int size;
        public TMP_Text text;
    }

    [Header ("Collected Items")]
    public List<CollectedItem> collectedItems;
    public Dictionary<string, CollectedItem> collectedItemDict;

    public int moveCount;
    public TMP_Text moveCountText;


    private void Start()
    {
        isLevelComplated = false;
        isLevelFailed = false;
        point = 0;
        levelHighScore = PlayerPrefs.GetInt("Level" + SceneManager.sceneCountInBuildSettings, 0);
        inGame_highScoreText.text = levelHighScore.ToString();
        scoreText.text = point.ToString();
        moveCountText.text = moveCount.ToString();
        collectedItemDict = new Dictionary<string, CollectedItem>();

        foreach (CollectedItem collectedItem in collectedItems)
        {
            collectedItemDict.Add(collectedItem.tag, collectedItem);
            collectedItemDict[collectedItem.tag].text.text = collectedItemDict[collectedItem.tag].size.ToString();
        }
    }

    public void Update()
    {
        if (checkIsGameFinished() && (!isLevelComplated || !isLevelFailed))
        {
            isLevelComplated = true;
            GridManager.Instance.SetAllInactive();
            SetHighScore();
            if (!CheckHighScore())
            {
                
                CanvasManager.Instance.LevelComplated();
            }
            else
            {
                CanvasManager.Instance.LevelComplatedWithHighScore(PlayerPrefs.GetInt("Level" + SceneManager.sceneCountInBuildSettings, 0), endGame_highScoreText);
            }
            unlockNewLevel();
        }
        if (checkMoveFinished() && (!isLevelComplated || !isLevelFailed))
        {
            isLevelFailed = true;
            GridManager.Instance.SetAllInactive();
            CanvasManager.Instance.LevelFailed();
        }
    }

    private void unlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void countPoppedItems(List<GameObject> tiles)
    {
        foreach (GameObject tile in tiles)
        {
            string tag = tile.tag;
            if (collectedItemDict.ContainsKey(tag))
            {
                if (collectedItemDict[tag].size != 0)
                {
                    collectedItemDict[tag].size--;
                    collectedItemDict[tag].text.text = collectedItemDict[key: tag].size.ToString();
                } 
            }
        }
    }

    public void removeMoveCount()
    {
        moveCount--;
        moveCountText.text = moveCount.ToString();
    }

    private bool checkIsGameFinished()
    {
        foreach (CollectedItem collectedItem in collectedItems)
        {
            if (collectedItem.size != 0)
            {
                return false;
            }
        }

        return true;
    }
    private bool checkMoveFinished()
    {
        if(moveCount <= 0)
        {
            return true;
        }

        return false;
    }

    public void addPoints(int count)
    {
        if(count == 3)
        {
            point = point + 1;

        }else if(count > 3)
        {
            point++;
            for(int i = 3; i < count; i++)
            {
                point++;
            }
        }
        scoreText.text = point.ToString();

    }

    private bool CheckHighScore()
    {
        if (levelHighScore >= point)
        {
            return false;
        }
        return true;
    }

    private void SetHighScore()
    {
        if (CheckHighScore())
        {
            PlayerPrefs.SetInt("Level" + SceneManager.sceneCountInBuildSettings, point);

        }
    }
}
