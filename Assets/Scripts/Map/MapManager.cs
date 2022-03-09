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
    private Transform emptyTileBucket;
    #endregion

    #region Properties
    // 실제 맵의 타일을 담고있는 2차원 배열
    public Tile[,] TileArray
    {
        get => _tileArray;
        set { _tileArray = value; }
    }
    // 문 역할을 하는 타일 담고있는 리스트
    public List<Tile> DoorList { get; set; }
    public List<Tile> RockList { get; set; }
    public List<Room> RoomList { get; set; }
    #endregion

    #region Unity Life Cycles ()
    public override void Awake()
    {
        base.Awake();

        emptyTileBucket = tileManager.transform.Find("EmptyTileBucket");

        DoorList = new List<Tile>();
        RockList = new List<Tile>();
        RoomList = new List<Room>();
    }
    #endregion


    public void CreateBeginTiles()
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
                var tileObject = tileManager.Create(tileManager.transform, new Vector2(x, y), (int)Tile.Layer.Background);

                // 초기에는 빈공간 버킷에 타일을 분류
                tileObject.transform.SetParent(emptyTileBucket);

                tileObject.Coordinate = new Vector2Int(dx, dy);
                tileObject.RealCoordinate = new Vector2Int(x, y);
                tileObject.IsStructure = false;
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

    // 인자로 받은 좌표가 맵의 범위 내에 있는지 체크 (true : 내부, false : 외부)
    public bool IsMapInside(int x, int y)
    {
        bool result = false;

        result = (x >= 0 && x < width) && (y >= 0 && y < height);

        return result;
    }

    // 방향을 순서대로 입력하여 기준 타일로부터 이동된 거리의 타일 좌표 반남
    public Tile FindTile(Tile standardTile, params Dir[] dir)
    {
        Tile result = default(Tile);

        int standardX = standardTile.Coordinate.x;
        int standardY = standardTile.Coordinate.y;

        for (int i = 0; i < dir.Length; i++)
        {
            if (dir[i] == Dir.UP) standardY++;
            else if (dir[i] == Dir.DOWN) standardY--;
            else if (dir[i] == Dir.LEFT) standardX--;
            else if (dir[i] == Dir.RIGHT) standardX++;
        }

        // 이동된 거리의 타일이 맵 범위를 벗어날 경우 null을 반환한다.
        if (MapManager.Instance.IsMapInside(standardX, standardY))
        {
            result = TileArray[standardY, standardX];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // 구조물(문)이 생성될 위치의 바닥 타일을 받아, 문 생성
    public void CreateDoorTiles(Tile tile)
    {
        // 기본 타일 생성(해당 타일 위치로 이동)
        Tile tileObject = tileManager.Create(tileManager.transform, new Vector2(tile.RealCoordinate.x, tile.RealCoordinate.y), (int)Tile.Layer.Structure);
        tileObject.transform.SetParent(tileManager.doorTileBucket);

        tileObject.gameObject.AddComponent<Door>();
        tileObject.gameObject.AddComponent<BoxCollider2D>();

        tileObject.IsStructure = true;
        tileObject.name = "DoorTile";

        //입구의 바닥 타일이면
        if (tile.type == Tile.Type.Entrance_Floor_Top || tile.type == Tile.Type.Entrance_Floor_Bottom)
        {
            // 위, 아래 문이면 Vertical Door 타입 설정
            tileObject.type = Tile.Type.Door_Vertical_Right;
        }
        else if (tile.type == Tile.Type.Entrance_Floor_Left)
        {
            tileObject.type = Tile.Type.Door_Horizontal_Right_Up;

        }
        else if (tile.type == Tile.Type.Entrance_Floor_Right)
        {
            tileObject.type = Tile.Type.Door_Horizontal_Left_Up;

        }

        DoorList.Add(tileObject);

        TileManager.Instance.ChangeTileSpriteByType(ref tileObject);
    }

    // 구조물(바위)이 생성될 위치의 바닥 타일을 받아, 문 생성
    public void CreateRockTiles(Tile tile)
    {
        Tile tileObject = default(Tile);
        // 해당 타일이 위치한 방 객체를 얻어와야 함.
        this.RoomList.ForEach(room =>
        {
            // 해당 바닥 타일이 현재 방에 위치하는 경우 (방에 할당된 바위의 갯수를 넘지 않게하기위해 방을 참조해야한다.)
            if (room.IsRoom(tile.Coordinate))
            {
                // 제한된 바위의 갯수만큼 바퀴 생성하는 조건
                if (room.NumberOfRock > room.CurrentNumberOfRock && Utils.RandomByCase(30))
                {
                    tileObject = tileManager.Create(tileManager.transform, new Vector2(tile.RealCoordinate.x, tile.RealCoordinate.y), (int)Tile.Layer.Structure);
                    tileObject.transform.SetParent(tileManager.rockTileBucket);

                    // 바위의 종류 랜덤하게 생성
                    if (Utils.RandomByCase(3)) tileObject.type = Tile.Type.Structure_Small_Rock;
                    else tileObject.type = Tile.Type.Structure_Big_Rock;

                    tileObject.gameObject.AddComponent<Rock>();
                    tileObject.gameObject.AddComponent<BoxCollider2D>();

                    tileObject.IsStructure = true;
                    tileObject.name = "RockTile";

                    RockList.Add(tileObject);
                    TileManager.Instance.ChangeTileSpriteByType(ref tileObject);

                    room.CurrentNumberOfRock++;
                }
            }
        });
    }

    public void CreateBoneTiles(Tile tile)
    {
        Tile tileObject = default(Tile);
        // 해당 타일이 위치한 방 객체를 얻어와야 함.
        this.RoomList.ForEach(room =>
        {
            // 해당 바닥 타일이 현재 방에 위치하는 경우 (방에 할당된 바위의 갯수를 넘지 않게하기위해 방을 참조해야한다.)
            if (room.IsRoom(tile.Coordinate))
            {
                // 제한된 바위의 갯수만큼 바퀴 생성하는 조건
                if (room.NumberOfRock > room.CurrentNumberOfRock && Utils.RandomByCase(50))
                {
                    tileObject = tileManager.Create(tileManager.transform, new Vector2(tile.RealCoordinate.x, tile.RealCoordinate.y), (int)Tile.Layer.Structure);
                    tileObject.transform.SetParent(tileManager.rockTileBucket);

                    // 바위의 종류 랜덤하게 생성
                    if (Utils.RandomByCase(3)) tileObject.type = Tile.Type.Structure_Small_Rock;
                    else tileObject.type = Tile.Type.Structure_Big_Rock;

                    tileObject.gameObject.AddComponent<Rock>();
                    tileObject.gameObject.AddComponent<BoxCollider2D>();

                    tileObject.IsStructure = true;
                    tileObject.name = "RockTile";

                    RockList.Add(tileObject);
                    TileManager.Instance.ChangeTileSpriteByType(ref tileObject);

                    room.CurrentNumberOfRock++;
                }
            }
        });
    }

    public void CreateTorchlightTiles(Tile tile)
    {

    }

    // 길 검사
    public void InspectedWay()
    {
        MapManager mm = MapManager.Instance;
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
                if ((standardTile.IsContainString("Way_Floor") || standardTile.IsContainString("Entrance_Floor"))
                        && (standardTileUp1.type == Tile.Type.Way_Wall_Top || standardTileUp1.type == Tile.Type.Entrance_Top)
                        && (standardTileDown1.type == Tile.Type.Way_Wall_Bottom || standardTileDown1.IsContainString("Entrance")))
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

                    // 수평 길에서 타일의 두번째 위 타일이 땅(Ground)일 경우 방에 합친다.
                    if (mm.FindTile(standardTile, Dir.UP, Dir.UP).IsContainString("Ground"))
                    {
                        standardTile.type = Tile.Type.Ground_Inner;
                        standardTileUp1.type = Tile.Type.Ground_Inner;
                    }
                    // 수평 길에서 타일의 두번째 위 타일이 벽일 경우 해당 타일을 입구(Entrance)로 변경하고 바로 위 타일을 입구 타일로 변경한다.
                    else if (mm.FindTile(standardTile, Dir.UP, Dir.UP).type == Tile.Type.Way_Wall_Left)
                    {
                        standardTile.type = Tile.Type.Entrance_Floor_Left;
                        standardTileUp1.type = Tile.Type.Entrance_Top;
                    }
                    else if (mm.FindTile(standardTile, Dir.UP, Dir.UP).type == Tile.Type.Way_Wall_Right)
                    {
                        standardTile.type = Tile.Type.Entrance_Floor_Right;
                        standardTileUp1.type = Tile.Type.Entrance_Top;
                    }

                    if (mm.FindTile(standardTile, Dir.DOWN, Dir.DOWN).IsContainString("Ground"))
                    {
                        standardTile.type = Tile.Type.Ground_Inner;
                        standardTileDown1.type = Tile.Type.Ground_Inner;
                    }
                    else if (mm.FindTile(standardTile, Dir.DOWN, Dir.DOWN).type == Tile.Type.Way_Wall_Left)
                    {
                        standardTile.type = Tile.Type.Entrance_Floor_Left;
                        standardTileDown1.type = Tile.Type.Entrance_Left_Bottom;
                    }
                    else if (mm.FindTile(standardTile, Dir.DOWN, Dir.DOWN).type == Tile.Type.Way_Wall_Right)
                    {
                        standardTile.type = Tile.Type.Entrance_Floor_Right;
                        standardTileDown1.type = Tile.Type.Entrance_Right_Bottom;
                    }
                }

                // 수직 길
                if ((standardTile.IsContainString("Way_Floor") || standardTile.IsContainString("Entrance_Floor"))
                        && (standardTileLeft1.type == Tile.Type.Way_Wall_Left || standardTileLeft1.IsContainString("Entrance"))
                        && (standardTileRight1.type == Tile.Type.Way_Wall_Right || standardTileRight1.IsContainString("Entrance")))
                {
                    if (standardTileUp1.type == Tile.Type.Way_Wall_Bottom)
                    {
                        standardTile.type = Tile.Type.Way_Floor_NotTop;
                        standardTileUp1.type = Tile.Type.Way_Floor_NotTop;
                        standardTileLeft1Up1.type = Tile.Type.Entrance_Bottom_Left;
                        standardTileRight1Up1.type = Tile.Type.Entrance_Bottom_Right;
                    }
                    else if (standardTileDown1.type == Tile.Type.Way_Wall_Top)
                    {
                        standardTile.type = Tile.Type.Way_Floor_NotTop;
                        standardTileDown1.type = Tile.Type.Way_Floor_NotTop;
                        standardTileLeft1Down1.type = Tile.Type.Entrance_Top;
                        standardTileRight1Down1.type = Tile.Type.Entrance_Top;
                    }

                    // 수직 길에서 왼쪽왼쪽 타일 체크해 벽 생성 로직
                    if (mm.FindTile(standardTile, Dir.LEFT, Dir.LEFT).IsContainString("Ground")
                            || mm.FindTile(standardTile, Dir.LEFT, Dir.LEFT).IsContainString("Way_Floor"))
                    {
                        standardTile.type = Tile.Type.Ground_Inner;
                        standardTileLeft1.type = Tile.Type.Ground_Inner;
                    }
                    if (mm.FindTile(standardTile, Dir.LEFT, Dir.LEFT).type == Tile.Type.Way_Wall_Bottom)
                    {
                        standardTile.type = Tile.Type.Entrance_Floor_Bottom;
                        standardTileLeft1.type = Tile.Type.Entrance_Bottom_Left;
                    }
                    else if (mm.FindTile(standardTile, Dir.LEFT, Dir.LEFT).type == Tile.Type.Way_Wall_Top)
                    {
                        standardTile.type = Tile.Type.Entrance_Floor_Top;
                        standardTileLeft1.type = Tile.Type.Entrance_Top;
                    }
                    else if (mm.FindTile(standardTile, Dir.LEFT, Dir.LEFT).type == Tile.Type.Entrance_Bottom_Right)
                    {
                        standardTileLeft1.type = Tile.Type.Entrance_Bottom_Left;
                    }

                    // 수직 길에서 오른쪽오른쪽 타일 체크해 벽 생성 로직
                    if (mm.FindTile(standardTile, Dir.RIGHT, Dir.RIGHT).IsContainString("Ground") || mm.FindTile(standardTile, Dir.RIGHT, Dir.RIGHT).IsContainString("Way_Floor"))
                    {
                        standardTile.type = Tile.Type.Ground_Inner;
                        standardTileRight1.type = Tile.Type.Ground_Inner;
                    }
                    if (mm.FindTile(standardTile, Dir.RIGHT, Dir.RIGHT).type == Tile.Type.Way_Wall_Bottom)
                    {
                        standardTile.type = Tile.Type.Entrance_Floor_Bottom;
                        standardTileRight1.type = Tile.Type.Entrance_Bottom_Right;
                    }
                    else if (mm.FindTile(standardTile, Dir.RIGHT, Dir.RIGHT).type == Tile.Type.Way_Wall_Top)
                    {
                        standardTile.type = Tile.Type.Entrance_Floor_Top;
                        standardTileRight1.type = Tile.Type.Entrance_Top;
                    }
                    else if (mm.FindTile(standardTile, Dir.RIGHT, Dir.RIGHT).type == Tile.Type.Entrance_Bottom_Left)
                    {
                        standardTileRight1.type = Tile.Type.Entrance_Bottom_Right;
                    }
                }


                // 길에서 입구 바닥의 옆옆, 뒤뒤, 아래아래 타일이 Dark 일 경우 타일 벽으로 변경(문은 입구 바닥 위치로 놓으면 됨)
                if (standardTile.IsContainString("Entrance_Floor"))
                {
                    if (mm.FindTile(standardTile, Dir.LEFT, Dir.LEFT).type == Tile.Type.Dark)
                    {
                        standardTileLeft1.type = Tile.Type.Way_Wall_Left;
                    }
                    else if (mm.FindTile(standardTile, Dir.RIGHT, Dir.RIGHT).type == Tile.Type.Dark)
                    {
                        standardTileRight1.type = Tile.Type.Way_Wall_Right;
                    }
                    else if (mm.FindTile(standardTile, Dir.UP, Dir.UP).type == Tile.Type.Dark)
                    {
                        standardTileUp1.type = Tile.Type.Way_Wall_Top;
                    }
                    else if (mm.FindTile(standardTile, Dir.DOWN, Dir.DOWN).type == Tile.Type.Dark)
                    {
                        standardTileDown1.type = Tile.Type.Way_Wall_Bottom;
                    }
                }
            }
        }
    }
}
