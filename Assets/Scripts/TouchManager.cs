using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{

    #region Singleton
    public static TouchManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    #endregion
    public Touch touch;
    public bool touchStarted = false;
    private void Update()
    {
        if(GameManager.Instance.isLevelComplated || GameManager.Instance.isLevelFailed)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if(touch.phase != TouchPhase.Ended || touch.phase != TouchPhase.Canceled)
            {
                touchStarted = true;
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D collider = Physics2D.OverlapPoint(touchPosition);
                if (collider != null)
                {
                    collider.gameObject.GetComponent<Tile>().makeSelectedSprite();
                }
            }
            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                touchStarted = false;
                LineManager.Instance.deleteLines();
                PopManager.Instance.checkPop();
            }
        }
    }
}
