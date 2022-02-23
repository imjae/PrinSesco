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
        // 실제로 맵에 사용되는 부분은 Room이기 때문에 홀수로 크기가 주어질 경우 여분으로 1을 늘려 인덱스 범위를 벗어나지 않게한다.
        TileArray = new Tile[height + 1, width + 1];

        int halfWidth = Mathf.RoundToInt(width * 0.5f);
        int halfHeight = Mathf.RoundToInt(height * 0.5f);

        // 배열의 x축 y축은 실제 타일맵의 좌표 x, y와 반전된 상태로 생각해야 한다.
        for (int y = -halfHeight; y < halfHeight; y++)
        {
            for (int x = -halfWidth; x < halfWidth; x++)
            {
                // 실제로 생성되는 타일의 위치는 0,0을 중심으로 생성되야 하기 때문에 좌표값을 할당하지 않는다.
                var tileObject = tileManager.Create(tileManager.transform, new Vector2(x, y), Color.white);
                tileObject.Coordinate = new Vector2Int(dx, dy);
                tileObject.name = $"Tile({dx}.{dy})";
                tileObject.type = Tile.Type.Dark;

                // dx, dy를 사용해 실제 타일의 위치값과 좌표값을 일치시긴다.
                TileArray[dy, dx] = tileObject;
                dx += 1;
            }
            dx = 0;
            dy += 1;
        }
    }

    public void InspectedWay()
    {
        // 기준 타일
        Tile standardTile = default(Tile);
        Tile standardTileUp1 = default(Tile);
        Tile standardTileDown1 = default(Tile);

        Tile standardTileLeft1 = default(Tile);
        Tile standardTileLeft1Up1 = default(Tile);
        Tile standardTileLeft1Down1 = default(Tile);

        Tile standardTileRight1 = default(Tile);
        Tile standardTileRight1Up1 = default(Tile);
        Tile standardTileRight1Down1 = default(Tile);


        for (int x = 1; x < height - 1; x++)
        {
            for (int y = 1; y < width - 1; y++)
            {
                standardTile = TileArray[x, y];
                standardTileUp1 = TileArray[x + 1, y];
                standardTileDown1 = TileArray[x - 1, y];

                standardTileLeft1 = TileArray[x, y - 1];
                standardTileLeft1Up1 = TileArray[x + 1, y - 1];
                standardTileLeft1Down1 = TileArray[x - 1, y - 1];

                standardTileRight1 = TileArray[x, y + 1];
                standardTileRight1Up1 = TileArray[x + 1, y + 1];
                standardTileRight1Down1 = TileArray[x - 1, y + 1];

                // 수평 길
                if (standardTile.IsContainString("Way_Floor") && standardTileUp1.type == Tile.Type.Way_Wall_Top && standardTileDown1.type == Tile.Type.Way_Wall_Bottom)
                {
                    if (standardTileLeft1.type == Tile.Type.Way_Wall_Right)
                    {
                        standardTileLeft1.type = Tile.Type.Way_Floor_Top;
                        standardTileLeft1Up1.type = Tile.Type.Entrance_Top;
                        standardTileLeft1Down1.type = Tile.Type.Entrance_Right_Bottom;
                    }
                    else if (standardTileRight1.type == Tile.Type.Way_Wall_Left)
                    {
                        standardTileRight1.type = Tile.Type.Way_Floor_Top;
                        standardTileRight1Up1.type = Tile.Type.Entrance_Top;
                        standardTileRight1Down1.type = Tile.Type.Entrance_Left_Bottom;
                    }
                }
                // 수직 길
                else if (standardTile.IsContainString("Way_Floor") && standardTileLeft1.type == Tile.Type.Way_Wall_Top && standardTileRight1.type == Tile.Type.Way_Wall_Bottom)
                {
                    if (standardTileUp1.type == Tile.Type.Way_Wall_Bottom)
                    {
                        standardTileUp1.type = Tile.Type.Way_Floor_NotTop;
                        standardTileLeft1Up1.type = Tile.Type.Entrance_Bottom_Left;
                        standardTileRight1Up1.type = Tile.Type.Entrance_Bottom_Right;
                    }
                    else if (standardTileDown1.type == Tile.Type.Way_Wall_Top)
                    {
                        standardTileDown1.type = Tile.Type.Way_Floor_NotTop;
                        standardTileLeft1Down1.type = Tile.Type.Entrance_Top;
                        standardTileRight1Down1.type = Tile.Type.Entrance_Top;
                    }
                }
            }
        }
    }
}
