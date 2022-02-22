using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    #region Fields
    private readonly string tile_prefix = "Dungeon_Tileset";

    [Header("Tile Prefabs")]
    [SerializeField] private Tile tilePrefab;

    public Sprite[] tileset;
    public Dictionary<string, List<Sprite>> tileDictionary;
    #endregion

    public override void Awake()
    {
        base.Awake();

        tileset = Resources.LoadAll<Sprite>("Map/DungeonTileset/Dungeon_Tileset");
        tileDictionary = InitializeTileset(tileset);

    }

    public Tile Create(Transform parent, Vector2 position, Color color, int order = 1)
    {
        Tile result = default(Tile);

        result = Instantiate(tilePrefab);
        result.transform.SetParent(parent);
        result.transform.localPosition = position;

        if (result.TryGetComponent<Tile>(out Tile tile))
        {
            tile.type = Tile.Type.Dark;
            // tile.color = color;
            tile.sortingOrder = order;
        }

        return result;
    }

    public Dictionary<string, List<Sprite>> InitializeTileset(Sprite[] tileset)
    {
        Dictionary<string, List<Sprite>> result = new Dictionary<string, List<Sprite>>();

        // 
        foreach (string type in Enum.GetNames(typeof(Tile.Type)))
        {
            List<Sprite> tmpList = new List<Sprite>();
            for (int i = 0; i < tileset.Length; i++)
            {
                if (tileset[i].name.Contains(type))
                {
                    tmpList.Add(tileset[i]);
                }
            }

            result.Add(type, tmpList);
        }

        return result;
    }

    //타일의 타입에 해당하는 스프라이트 리스트에서 랜덤 스프라이트를 설정
    public void ChangeTileSpriteByType(ref Tile tile)
    {
        List<Sprite> spriteListByType = tileDictionary[tile.type.ToString()];

        int index = 0;
        if (spriteListByType.Count < 2)
            index = 0;
        else
            index = UnityEngine.Random.Range(0, spriteListByType.Count);

        // Debug.Log($"{tile.type.ToString()} -> {spriteListByType.Count} -> {index}");
        tile.sprite = spriteListByType[index];
    }
}
