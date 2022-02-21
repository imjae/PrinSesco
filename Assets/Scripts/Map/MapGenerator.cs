using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    #region Fields
    [Header("Map Manager")]
    [SerializeField] private MapManager manager;
    [Header("Map Settings")]

    [Range(5, 100)]
    [SerializeField] private int width;
    [Range(5, 100)]
    [SerializeField] private int height;
    [SerializeField] private int iterationNumber;
    [SerializeField] private float widthRatio;
    [SerializeField] private float heightRatio;

    [Header("Way Settings")]
    [Range(2, 5)]
    [SerializeField] private int wayMinRange;

    private List<Room> roomList;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        roomList = new List<Room>();

        manager.width = width;
        manager.height = height;
        manager.InitializeTiles();

        Container mainContainer = new Container(0, 0, width, height);
        TreeNode containerTree = mainContainer.SplitContainer(mainContainer, iterationNumber, widthRatio, heightRatio);

        containerTree.Paint();
        containerTree.InitTileWayType(wayMinRange);

        containerTree.GetLeafs().ForEach(node =>
        {
            Room tmpRoom = new Room(node);
            roomList.Add(tmpRoom);
            tmpRoom.InitTileType();
        });





        // 타일에 설정된 타입에 맞게 스프라이트 한번에 변경
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                TileManager.Instance.ChangeTileSpriteByType(ref manager.TileArray[x, y]);
            }
        }
    }
}