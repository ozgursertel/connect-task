using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public bool isSelected;
    public Sprite unselected;
    public Sprite selected;
    private SpriteRenderer sr;
    // Start is called before the first frame update

    public int row = -1;
    public int column = -1;


    void Start()
    {
        isSelected = false;
        sr = GetComponent<SpriteRenderer>();
        //Debug.Log(this.gameObject.name + "This tile index is [" + row + "][" + column + "]");
    }



    // Update is called once per frame
    void Update()
    {
        if(!TouchManager.Instance.touchStarted)
        {
            makeUnselectedSprite();
        }
    }

    public void makeSelectedSprite()
    {
        if (!isSelected)
        {
            isSelected = true;
            sr.sprite = selected;
            LineManager.Instance.addPosition(transform.position);
            PopManager.Instance.addTile(this.gameObject);
        }
    }

    public void makeUnselectedSprite()
    {
        if (isSelected)
        {
            isSelected = false;
            sr.sprite = unselected;
        }

    }

    public IEnumerator moveGridDown(Vector2 pos)
    {
        //Debug.Log(this.gameObject.tag + " Moving down " + "Row => " + row + "Column => " + column);
        transform.DOMove(pos, 0.1f);
        yield return new WaitForSeconds(0.1f);
        
    }

    [System.Obsolete]
    public IEnumerator checkLowerTile()
    {
        if (column < GridManager.Instance.tiles.GetLength(0) - 1 && GridManager.Instance.tiles[column + 1, row] != null && this.gameObject.active == true)
        {
            GameObject lowerTile = GridManager.Instance.tiles[column + 1,row].GetComponent<Tile>().gameObject;
            if(lowerTile.active == false)
            {
                GridManager.Instance.tiles[column, row] = lowerTile;
                GridManager.Instance.tiles[column + 1, row] = this.gameObject;
                this.column++;
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(moveGridDown(GridManager.Instance.tilesPositions[column,row]));

            }
        }

    }
    
}
