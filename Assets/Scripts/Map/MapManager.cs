using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapManager : Singleton<MapManager>
{
    #region Fields
    [Header("Map Settings")]
    public int width;
    public int height;
    private Tile[,] _tileArray;

    [Header("Tile References")]
    public TileManager tileManager;
    public Tile tileResource;
    #endregion

    #region Properties
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

        TileArray = new Tile[height, width];

        int halfWidth = Mathf.RoundToInt(width * 0.5f);
        int halfHeight = Mathf.RoundToInt(height * 0.5f);

        for (int y = -halfHeight; y < halfHeight; y++)
        {
            for (int x = -halfWidth; x < halfWidth; x++)
            {
                Vector2 tmpCoordinate = new Vector2(dx, dy);
                var tileObject = tileManager.Create(tileManager.transform, new Vector2(x, y), Color.white);
                tileObject.Coordinate = new Vector2(dx, dy);
                tileObject.name = $"Tile({tileObject.Coordinate.x}.{tileObject.Coordinate.y})";

                TileArray[dy, dx] = tileObject;
                dy += 1;
            }
            dy = 0;
            dx += 1;
        }
    }
}
