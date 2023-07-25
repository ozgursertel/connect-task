using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopManager : MonoBehaviour
{

    #region Singleton
    public static PopManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    #endregion


    private List<GameObject> selectedTiles = new List<GameObject>();

    public void addTile(GameObject obj)
    {
        Handheld.Vibrate();
        selectedTiles.Add(obj);
        /*Debug.Log("----Selected Items-----");
        selectedTiles.ForEach(delegate (GameObject obj)
        {
            Debug.Log(obj.name);
        });*/
    }

    [System.Obsolete]
    public void checkPop()
    {
        if (isSameElement(selectedTiles))
        {
            selectedTiles.ForEach(delegate (GameObject obj)
            {
                StartCoroutine(destroyAnim(obj));
            });
            GameManager.Instance.addPoints(selectedTiles.Count);
            GridManager.Instance.SetPositions();
            GameManager.Instance.countPoppedItems(selectedTiles);
            GameManager.Instance.removeMoveCount();
        }
        selectedTiles.Clear();

    }

    private IEnumerator destroyAnim(GameObject obj)
    {
        GameObject temp =ObjectPooler.Instance.SpawnFromPool("DestoyObject", obj.transform.position, Quaternion.identity);
        obj.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        temp.SetActive(false);

    }

    public bool isSameElement(List<GameObject> list)
    {
        int count = 0;
        GameObject firstObj = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            if (firstObj.tag != list[i].tag)
            {
                return false;
            }
            count++;
        }
        if(count < 3)
        {
            return false;
        }
        return true;
    }

}
