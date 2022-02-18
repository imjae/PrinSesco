using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapManager : Singleton<MapManager>
{
    #region Fields
    [HideInInspector] public int width;
    [HideInInspector] public int height;
    private Tile[,] _tileArray;

    [Header("Tile References")]
    public TileManager tileManager;
    #endregion

    #region Properties
    // 실제 맵의 타일을 담고있는 2차원 배열
    public Tile[,] TileArray
    {
        get => _tileArray;
        set { _tileArray = value; }
    }
    #endregion

    #region Unity Life Cycles ()
    
    #endregion


    public void InitializeTiles()
    {
        int dx = 0;
        int dy = 0;

        // 맵의 높이 크기만큼 배열의 가로축 개수가 정해지는 방식
        TileArray = new Tile[height, width];

        int halfWidth = Mathf.RoundToInt(width * 0.5f);
        int halfHeight = Mathf.RoundToInt(height * 0.5f);

        // 배열의 x축 y축은 실제 타일맵의 좌표 x, y와 반전된 상태로 생각해야 한다.
        for (int y = -halfHeight; y < halfHeight; y++)
        {
            for (int x = -halfWidth; x < halfWidth; x++)
            {
                // 실제로 생성되는 타일의 위치는 0,0을 중심으로 생성되야 하기 때문에 좌표값을 할당하지 않는다.
                var tileObject = tileManager.Create(tileManager.transform, new Vector2(x, y), Color.white);
                tileObject.Coordinate = new Vector2(dy, dx);
                tileObject.name = $"Tile({dx}.{dy})";

                // dx, dy를 사용해 실제 타일의 위치값과 좌표값을 일치시긴다.
                TileArray[dy, dx] = tileObject;
                dx += 1;
            }
            dx = 0;
            dy += 1;
        }
    }
}
