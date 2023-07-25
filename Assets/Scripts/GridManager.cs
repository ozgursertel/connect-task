using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Singleton
    public static GridManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    #endregion
    public GameObject[,] tiles = new GameObject[6,6];

    //Transformlar için Tiles Positionları
    public Vector2[,] tilesPositions = {
        {new Vector2(-2,2),new Vector2(-1.2f,2), new Vector2(-0.4f, 2), new Vector2(0.4f, 2), new Vector2(1.2f, 2), new Vector2(2,2)},
        {new Vector2(-2,1.1f),new Vector2(-1.2f,1.1f), new Vector2(-0.4f, 1.1f), new Vector2(0.4f, 1.1f), new Vector2(1.2f, 1.1f), new Vector2(2,1.1f)},
        {new Vector2(-2,0.2f),new Vector2(-1.2f,0.2f), new Vector2(-0.4f,0.2f), new Vector2(0.4f,0.2f), new Vector2(1.2f,0.2f), new Vector2(2,0.2f)},
        {new Vector2(-2,-0.7f),new Vector2(-1.2f,-0.7f), new Vector2(-0.4f,-0.7f), new Vector2(0.4f,-0.7f), new Vector2(1.2f,-0.7f), new Vector2(2,-0.7f)},
        {new Vector2(-2,-1.6f),new Vector2(-1.2f,-1.6f), new Vector2(-0.4f,-1.6f), new Vector2(0.4f,-1.6f), new Vector2(1.2f,-1.6f), new Vector2(2,-1.6f)},
        {new Vector2(-2,-2.5f),new Vector2(-1.2f,-2.5f), new Vector2(-0.4f,-2.5f), new Vector2(0.4f,-2.5f), new Vector2(1.2f,-2.5f), new Vector2(2,-2.5f)}
    };


    public void Start()
    {
        for(int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                //ObjectPooler ile oluşturup buraya eklenecek
                tiles[i,j] = ObjectPooler.Instance.RandomObjectSpawnFromPool(tilesPositions[i,j]);
                tiles[i, j].GetComponent<Tile>().column = i;
                tiles[i, j].GetComponent<Tile>().row = j;
            }
        }
    }

    [System.Obsolete]
    public void SetPositions()
    {
        for(int k = 0; k < tiles.GetLength(0); k++)
        {
            // Iterate through the grid from bottom to top
            for (int i = tiles.GetLength(0)-1; i >= 0; i--)
            {
                for (int j = tiles.GetLength(1)-1; j >= 0; j--)
                {
                    StartCoroutine(tiles[i, j].GetComponent<Tile>().checkLowerTile());
                }
            }
        }

        StartCoroutine(FillGrid());
        ReshuffleGrid();

    }


    #region Resuffle
    public void ReshuffleGrid()
    {
        while (!HasThreeAdjacentElements())
        {
            ShuffleGrid();
        }
    }

    private bool HasThreeAdjacentElements()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                string tag = tiles[i, j].tag;

                // Check for three same tags in rows.
                if (DFS(i, j, tag, 0, 1, 0) || DFS(i, j, tag, 0, 0, 1))
                {
                    return true;
                }

                // Check for three same tags in right diagonals (bottom to top).
                if (DFS(i, j, tag, 0, 1, 1))
                {
                    return true;
                }

                // Check for three same tags in left diagonals (bottom to top).
                if (DFS(i, j, tag, 0, 1, -1))
                {
                    return true;
                }
            }
        }

        return false;
    }


    private void ShuffleGrid()
    {
        ResuffleAnimation();
        

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i, j] != null && tiles[i, j].activeSelf)
                {
                    tiles[i, j].SetActive(false);

                    GameObject newTile = ObjectPooler.Instance.RandomObjectSpawnFromPool(tilesPositions[i, j]);

                    newTile.transform.position = tilesPositions[i, j];
                    newTile.GetComponent<Tile>().row = j;
                    newTile.GetComponent<Tile>().column = i;

                    tiles[i, j] = newTile;
                }
            }
        }
    }

    public void ResuffleAnimation()
    {
        StartCoroutine(CanvasManager.Instance.ReshuffleAnimation());

    }

    #endregion


    private IEnumerator FillGrid()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if(tiles[i,j].active == false)
                {
                    //ObjectPooler ile oluşturup buraya eklenecek
                    tiles[i, j] = ObjectPooler.Instance.RandomObjectSpawnFromPool(new Vector2(tilesPositions[i, j].x,3));
                    StartCoroutine(tiles[i, j].GetComponent<Tile>().moveGridDown(tilesPositions[i, j]));
                    tiles[i, j].SetActive(true);
                    tiles[i, j].GetComponent<Tile>().column = i;
                    tiles[i, j].GetComponent<Tile>().row = j;
                    
                }

            }
        }
    }

    #region DFS
    private bool DFS(int i, int j, string targetTag, int count, int dx, int dy)
    {
        if (i < 0 || i >= tiles.GetLength(0) || j < 0 || j >= tiles.GetLength(1) || tiles[i, j] == null || tiles[i, j].tag != targetTag)
            return false;

        count++;
        if (count >= 3)
            return true;

        return DFS(i + dx, j + dy, targetTag, count, dx, dy);
    }

    #endregion

    public void SetAllInactive()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i, j].SetActive(false);

            }
        }
    }
}
