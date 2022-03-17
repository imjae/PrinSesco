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

    #endregion
    // Start is called before the first frame update
    void Start()
    {

        manager.width = width;
        manager.height = height;
        manager.CreateBeginTiles();

        Container mainContainer = new Container(0, 0, width, height);
        TreeNode containerTree = mainContainer.SplitContainer(mainContainer, iterationNumber, widthRatio, heightRatio);

        // Paint 함수는 테스트용
        // containerTree.Paint();
        containerTree.InitTileWayType(wayMinRange);

        containerTree.GetLeafs().ForEach(node =>
        {
            Room tmpRoom = new Room(node);

            //방의 가로, 세로 길이가 1인 경우 방을 추가하지 않는다. (타일 구조상 한칸짜리 방은 타일의 규칙을 흐트러트린다.)
            if (!(tmpRoom.Width == 1 || tmpRoom.Height == 1))
                manager.RoomList.Add(tmpRoom);
        });

        manager.RoomList.ForEach(room =>
        {
            // 기본 방 타일 타입 변경
            room.InitRoomTileType();

            // 방과 길의 예외케이스 검사 (최대한 방의 벽에서 체크를 해야 비용을 줄일 수 있다)
            room.InspectedLeftWall();
            room.InspectedTopWall();
            room.InspectedRightWall();
            room.InspectedBottomWall();
        });

        // 컨테이너를 이어준 길이 교차하는지를 검사하는 로직
        manager.InspectedWay();


        Tile targetTile = default(Tile);
        // 타일에 설정된 타입에 맞게 스프라이트 한번에 변경
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                //TODO 환경 구조물 타일 생성
                targetTile = manager.TileArray[x, y];

                // 타일에 구조물이 없는 상태에서, Entrance_Floor인 조건
                if (!targetTile.IsStructure && targetTile.IsContainString("Entrance_Floor")) manager.CreateDoorTiles(targetTile);
                if (!targetTile.IsStructure) manager.CreateRockTiles(targetTile);
                if (!targetTile.IsStructure) manager.CreateBoneTiles(targetTile);

                TileManager.Instance.ChangeTileSpriteByType(ref targetTile);    // 타입에 맞게 스프라이트 변경
                TileManager.Instance.ChangeTileParentByType(ref targetTile);    // 타입에 맞게 부모오브젝트 분류
            }
        }

        // WallTileBucket에 임포트된 CompositeCollider2D 컴포넌트 실행 (벽 타일의 box콜라이더들 합쳐주는 역할)
        TileManager.Instance.wallTileBucket.GetComponent<CompositeCollider2D>().GenerateGeometry();
    }
}