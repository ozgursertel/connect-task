using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

    #region Singleton
    public static LineManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion


    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void addPosition(Vector2 position)
    {
        lr.positionCount = lr.positionCount + 1;
        lr.SetPosition(lr.positionCount - 1, position);
    }

    public void deleteLines()
    {
        lr.positionCount = 0;
    }
}
