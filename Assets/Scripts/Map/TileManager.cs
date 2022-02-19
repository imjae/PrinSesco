using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    #region Fields
    [Header("Tile Prefabs")]
    [SerializeField] private Tile tilePrefab;
    public int tileWidth;
    public int tileHeight;
    #endregion

    public Tile Create(Transform parent, Vector2 position, Color color, int order = 1)
    {
        Tile result = default(Tile);

        result = Instantiate(tilePrefab);
        result.transform.SetParent(parent);
        result.transform.localPosition = position;

        if (result.TryGetComponent<Tile>(out Tile tile))
        {
            tile.type = Tile.Type.GROUND;
            tile.color = color;
            tile.sortingOrder = order;
        }

        return result;
    }

    //TODO 타일 렌더러 변경
}
