using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    #region Fields
    [Header("Tile Prefabs")]
    [SerializeField] private Tile tilePrefab;

    public Texture2D tileset;

    // 타일의 타입별 Sprite를 담고있는 리스트
    public List<Sprite> tilesetList;
    public Dictionary<string, List<Sprite>> tileDictionary;
    #endregion

    void Start()
    {
        tileDictionary = new Dictionary<string, List<Sprite>>();
        Tile t = new Tile();
        ChangeTileSpriteByType(ref t, Tile.Type.Ground_Edge_Left_Bottom);
    }

    public Tile Create(Transform parent, Vector2 position, Color color, int order = 1)
    {
        Tile result = default(Tile);

        result = Instantiate(tilePrefab);
        result.transform.SetParent(parent);
        result.transform.localPosition = position;

        if (result.TryGetComponent<Tile>(out Tile tile))
        {
            tile.type = Tile.Type.Ground;
            // tile.color = color;
            tile.sortingOrder = order;
        }

        return result;
    }

    //TODO 타일 스프라이트 변경
    public void ChangeTileSpriteByType(ref Tile tile, Tile.Type type)
    {
        string prefix = "Dungeon_Tileset";

        string typeStr = type.ToString();
        string[] typeKeywordArr = typeStr.Split('_');

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < typeKeywordArr.Length; i++)
        {
            if (i > 0) sb.Append(typeKeywordArr[i]);
            if (i != typeKeywordArr.Length - 1) sb.Append("_");
        }

        string typeKeyword = sb.ToString();

        string spriteStr = default(string);
        spriteStr = $"{prefix}_{typeKeyword}";

        Debug.Log(spriteStr);

        if (type == Tile.Type.Ground_Edge_Left_Top) spriteStr = $"{prefix}_{typeKeyword}";


    }
}
