using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    #region Fields
    [Header("Tile Prefabs")]
    [SerializeField] private Tile tilePrefab;

    // 타일의 타입별 Sprite를 담고있는 리스트
    public Sprite[] tileset;
    public Dictionary<string, List<Sprite>> tileDictionary;
    #endregion

    void Start()
    {
        tileset = Resources.LoadAll<Sprite>("Map/DungeonTileset/Dungeon_Tileset");
        InitializeTileset(tileset);

        tileDictionary = new Dictionary<string, List<Sprite>>();


    }

    public Tile Create(Transform parent, Vector2 position, Color color, int order = 1)
    {
        Tile result = default(Tile);

        result = Instantiate(tilePrefab);
        result.transform.SetParent(parent);
        result.transform.localPosition = position;

        if (result.TryGetComponent<Tile>(out Tile tile))
        {
            tile.type = Tile.Type.Ground_Inner;
            // tile.color = color;
            tile.sortingOrder = order;
        }

        return result;
    }

    public Dictionary<string, List<Sprite>> InitializeTileset(Sprite[] tileset)
    {
        Dictionary<string, List<Sprite>> result = new Dictionary<string, List<Sprite>>();

        for (int i = 0; i < tileset.Length; i++)
        {
            Debug.Log(tileset[i].name);
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

        string spriteStr = default(string);
        spriteStr = $"{prefix}_{sb.ToString()}";

        // tile.sprite = new Sprite(spriteStr);
    }
}
