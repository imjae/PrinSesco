using UnityEngine;
using System;


public class Room
{
    #region Fields (클래스 내부 연산 용도)
    private int x, y, w, h;
    private int size;
    #endregion

    #region Properties (클래스 외부에서 접근 가능한 데이터 용도)
    public Vector2Int Coordinate { get; set; }

    // 방의 가장자리 모서리 좌표 (ex: LB == LeftBottom, RB == RightBottom ..)
    public Vector2Int LB { get; set; }
    public Vector2Int LT { get; set; }
    public Vector2Int RB { get; set; }
    public Vector2Int RT { get; set; }

    // 방의 벽 모서리 좌표 (ex: WLB == WallLeftBottom, WRB == WallRightBottom ..)
    public Vector2Int WLB { get; set; }
    public Vector2Int WLT { get; set; }
    public Vector2Int WRB { get; set; }
    public Vector2Int WRT { get; set; }

    // 방의 가운데 좌표
    public Vector2Int Center { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    public int NumberOfRock { get; set; }
    public int CurrentNumberOfRock { get; set; }
    public int NumberOfBone { get; set; }
    public int CurrentNumberOfBone { get; set; }
    #endregion

    public Room(Container container)
    {
        this.x = container.x + UnityEngine.Random.Range(2, 4);
        this.y = container.y + UnityEngine.Random.Range(2, 4);
        this.w = container.w - (this.x - container.x);
        this.h = container.h - (this.y - container.y);
        this.w -= UnityEngine.Random.Range(2, 4);
        this.h -= UnityEngine.Random.Range(2, 4);

        this.Coordinate = new Vector2Int(this.x, this.y);
        this.Width = w;
        this.Height = h;
        this.size = this.w * this.h;

        this.LB = new Vector2Int(this.x, this.y);
        this.LT = new Vector2Int(this.x, this.y + this.Height - 1);
        this.RB = new Vector2Int(this.x + this.Width - 1, this.y);
        this.RT = new Vector2Int(this.x + this.Width - 1, this.y + this.Height - 1);

        this.WLB = new Vector2Int(this.LB.x - 1, this.LB.y - 1);
        this.WLT = new Vector2Int(this.LT.x - 1, this.LT.y + 1);
        this.WRB = new Vector2Int(this.RB.x + 1, this.RB.y - 1);
        this.WRT = new Vector2Int(this.RT.x + 1, this.RT.y + 1);

        this.Center = new Vector2Int
        {
            x = this.x + Mathf.RoundToInt(this.w * 0.5f),
            y = this.y + Mathf.RoundToInt(this.h * 0.5f)
        };

        // TODO 일단 넓이에서 10를 나눈 몫을 바위의 갯수로 지정
        this.NumberOfRock = this.size / 10;
        this.CurrentNumberOfRock = 0;
        // TODO 일단 넓이에서 12를 나눈 몫을 뼈의 갯수로 지정
        this.NumberOfBone = this.size / 15;
        this.CurrentNumberOfBone = 0;
    }

    #region Room 판별 함수(벽, 바닥, 룸 내부 ...)
    // 방의 바닥의 가장자리 여부를 체크
    public bool IsBorderByFloor(Dir dir, int x, int y)
    {
        bool result = default(bool);

        if (dir == Dir.LEFT)
            result = (x == LB.x && (y >= LB.y && y <= LT.y));
        else if (dir == Dir.RIGHT)
            result = (x == RB.x && (y >= RB.y && y <= RT.y));
        else if (dir == Dir.UP)
            result = (x >= LT.x && x <= RT.x) && y == LT.y;
        else if (dir == Dir.DOWN)
            result = (x >= LT.x && x <= RT.x) && y == LB.y;

        return result;
    }

    // 방의 벽의 가장자리 여부를 체크
    public bool IsBorderByWall(Dir dir, int x, int y)
    {
        bool result = default(bool);

        if (dir == Dir.LEFT)
            result = (x == WLB.x && (y >= WLB.y && y <= WLT.y));
        else if (dir == Dir.RIGHT)
            result = (x == WRB.x && (y >= WRB.y && y <= WRT.y));
        else if (dir == Dir.UP)
            result = (x >= WLT.x && x <= WRT.x) && y == WLT.y;
        else if (dir == Dir.DOWN)
            result = (x >= WLT.x && x <= WRT.x) && y == WLB.y;

        return result;
    }

    public bool IsRoom(Vector2Int coordinate)
    {
        bool result = default(bool);

        int x = coordinate.x;
        int y = coordinate.y;

        // y 좌표가 LB보다 크거나 같고 LT보다 작거나 같고
        bool isY = y >= this.LB.y && y <= this.LT.y;
        // x 좌표가 LB 보다 크거나 같고 RB보다 작거나 같아야 한다.
        bool isX = x >= this.LB.x && x <= this.RB.x;

        result = isX && isY;

        return result;
    }
    #endregion

    public void PaintColor(Color color)
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = x; i < x + w; i++)
        {
            for (int j = y; j < y + h; j++)
            {
                tileArray[j, i].color = color;
            }
        }
    }

    // 방 타일을 순환하하면서 인자로 받은 콜백함수 실행
    public void ExcutesCallbackCirculationRoom(Action<Tile> action)
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = x; i < x + w; i++)
        {
            for (int j = y; j < y + h; j++)
            {
                action(tileArray[j, i]);
            }
        }
    }

    public void InitRoomTileType()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = x; i < x + w; i++)
        {
            for (int j = y; j < y + h; j++)
            {
                // tileArray[j, i].color = Color.cyan;
                tileArray[j, i].type = Tile.Type.Room_Floor_Inner;

                if (IsBorderByFloor(Dir.DOWN, i, j))
                {
                    // 방의 아랫쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Room_Floor_Bottom;
                    tileArray[j - 1, i].type = Tile.Type.Room_Wall_Bottom;

                    // 방의 가장 좌측 아랫쪽 바닥의 경우 여기서 처리
                    // if (i == x) tileArray[j, i - 1].type = Tile.Type.Room_Wall_Left;
                }
                if (IsBorderByFloor(Dir.LEFT, i, j))
                {
                    // 방의 왼쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Room_Floor_Left;
                    tileArray[j, i - 1].type = Tile.Type.Room_Wall_Left;

                    // 방의 가장 좌측 위쪽 바닥의 경우 여기서 처리
                    // if (j == y + h - 1) tileArray[j + 1, i].type = Tile.Type.Room_Wall_Top;
                }
                if (IsBorderByFloor(Dir.UP, i, j))
                {
                    // 방의 윗쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Room_Floor_Top;
                    tileArray[j + 1, i].type = Tile.Type.Room_Wall_Top;
                }
                if (IsBorderByFloor(Dir.RIGHT, i, j))
                {
                    // 방의 오른쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Room_Floor_Right;
                    tileArray[j, i + 1].type = Tile.Type.Room_Wall_Right;
                }

                // 방의 모서리에 해당하는 바닥타일과, 벽을 마지막으로 덮는다.
                if (i == LB.x && j == LB.y)
                {
                    tileArray[j, i].type = Tile.Type.Room_Floor_Edge_Left_Bottom;
                    tileArray[j - 1, i - 1].type = Tile.Type.Room_Wall_Edge_Left_Bottom;
                }
                else if (i == RT.x && j == RT.y)
                {
                    tileArray[j, i].type = Tile.Type.Room_Floor_Edge_Right_Top;
                    tileArray[j + 1, i + 1].type = Tile.Type.Room_Wall_Edge_Right_Top;
                }
                else if (i == LT.x && j == LT.y)
                {
                    tileArray[j, i].type = Tile.Type.Room_Floor_Edge_Left_Top;
                    tileArray[j + 1, i - 1].type = Tile.Type.Room_Wall_Edge_Left_Top;
                }
                else if (i == RB.x && j == RB.y)
                {
                    tileArray[j, i].type = Tile.Type.Room_Floor_Edge_Right_Bottom;
                    tileArray[j - 1, i + 1].type = Tile.Type.Room_Wall_Edge_Right_Bottom;
                }
            }
        }
    }

    #region Inspect 함수 모음 (벽 검사)
    // 방의 왼쪽 벽에서 예외 케이스 검사
    public void InspectedLeftWall()
    {
        // 자주 사용하기 때문에 이름을 간추렸다.
        MapManager mm = MapManager.Instance;
        Tile[,] tileArray = MapManager.Instance.TileArray;

        // 기준 타일 (Standard Tile)
        Tile standardTile = default(Tile);
        // 기준 타일 한칸 왼쪽 타일
        Tile standardTileLeft1 = default(Tile);
        // 기준 타일 한칸 오른쪽 타일
        Tile standardTileRight1 = default(Tile);

        for (int i = WLB.y; i <= WLT.y; i++)
        {
            standardTile = tileArray[i, WLB.x];
            standardTileLeft1 = mm.FindTile(standardTile, Dir.LEFT);
            // standardTileLeft2 = tileArray[i, WLB.x - 2];
            standardTileRight1 = mm.FindTile(standardTile, Dir.RIGHT);

            // 왼쪽 벽의 왼쪽으로 한칸, 두칸이 모두 길 바닥일 경우 입구를 생성
            if (standardTileLeft1.IsContainString("Way_Floor") && mm.FindTile(standardTile, Dir.LEFT, Dir.LEFT).IsContainString("Way_Floor"))
            {
                standardTile.type = Tile.Type.Entrance_Floor_Left;
                standardTileRight1.type = Tile.Type.Room_Floor_Inner;

                mm.FindTile(standardTile, Dir.UP).type = Tile.Type.Entrance_Top;
                mm.FindTile(standardTile, Dir.DOWN).type = Tile.Type.Entrance_Left_Bottom;
            }
            // 왼쪽 벽과 수직 길이 합쳐지는 경우
            else if (standardTileLeft1.IsContainString("Way_Floor"))
            {
                // 방의 왼쪽벽을 바닥으로 변경하고 엣지의 경우 방향에 맞게 돌려줘야함
                if (standardTile.type == Tile.Type.Room_Wall_Edge_Left_Top)
                {
                    standardTile.type = Tile.Type.Entrance_Top;

                }
                else if (standardTile.type == Tile.Type.Room_Wall_Edge_Left_Bottom)
                {
                    standardTile.type = Tile.Type.Entrance_Bottom_Right;
                }
                else
                {
                    standardTile.type = Tile.Type.Room_Floor_Inner;
                    standardTileLeft1.type = Tile.Type.Room_Floor_Inner;
                    standardTileRight1.type = Tile.Type.Room_Floor_Inner;
                }
            }
            else if (standardTileLeft1.type == Tile.Type.Way_Wall_Left)
            {
                if (standardTile.type == Tile.Type.Room_Wall_Edge_Left_Top)
                {
                    standardTile.type = Tile.Type.Way_Floor_NotTop;
                    standardTileRight1.type = Tile.Type.Entrance_Top;
                }
                else if (standardTile.type == Tile.Type.Room_Wall_Edge_Left_Bottom)
                {
                    standardTile.type = Tile.Type.Way_Floor_NotTop;
                    standardTileRight1.type = Tile.Type.Entrance_Bottom_Right;
                }
                else
                {
                    standardTile.type = Tile.Type.Room_Floor_Inner;
                    standardTileRight1.type = Tile.Type.Room_Floor_Inner;
                }
            }
        }
    }
    // 방의 윗쪽 벽에서 예외 케이스 검사
    public void InspectedTopWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;

        // 기준 타일
        Tile standardTile = default(Tile);
        // 기준 타일 한칸 위 타일
        Tile standardTileUp1 = default(Tile);
        // 기준 타일 한칸 아래 타일
        Tile standardTileDown1 = default(Tile);
        Tile standardTileLeft1 = default(Tile);
        Tile standardTileRight1 = default(Tile);

        for (int i = WLT.x; i <= WRT.x; i++)
        {
            standardTile = tileArray[WLT.y, i];
            standardTileUp1 = tileArray[WLT.y + 1, i];
            standardTileDown1 = tileArray[WLT.y - 1, i];
            standardTileLeft1 = tileArray[WLT.y, i - 1];
            standardTileRight1 = tileArray[WLT.y, i + 1];

            // 윗쪽 벽의 윗쪽 한칸, 두칸이 모두 길의 바닥인 경우(수직 길인 경우)
            if (standardTileUp1.IsContainString("Way_Floor") && tileArray[WLT.y + 2, i].IsContainString("Way_Floor"))
            {
                standardTile.type = Tile.Type.Entrance_Floor_Top;
                standardTileLeft1.type = Tile.Type.Entrance_Top;
                standardTileRight1.type = Tile.Type.Entrance_Top;
                standardTileDown1.type = Tile.Type.Room_Floor_Inner;
            }
            // 윗쪽 벽의 윗쪽 한칸만 길의 바닥인 경우(방과 길을 합쳐야 하는 경우)
            else if (standardTileUp1.IsContainString("Way_Floor"))
            {
                if (standardTile.type == Tile.Type.Room_Wall_Edge_Left_Top)
                {
                    standardTile.type = Tile.Type.Entrance_Left_Bottom;
                    standardTileUp1.type = Tile.Type.Way_Floor_Top;
                }
                else if (standardTile.type == Tile.Type.Room_Wall_Edge_Right_Top)
                {
                    standardTile.type = Tile.Type.Entrance_Right_Bottom;
                    standardTileUp1.type = Tile.Type.Way_Floor_Top;
                }
                else
                {
                    standardTile.type = Tile.Type.Room_Floor_Inner;
                    standardTileUp1.type = Tile.Type.Room_Floor_Top;
                    standardTileDown1.type = Tile.Type.Room_Floor_Inner;
                }
            }
            // 윗쪽 벽과 길의 윗 벽이 닿아있는 경우(방과 길을 합침)
            else if (standardTileUp1.type == Tile.Type.Way_Wall_Top)
            {
                if (standardTile.type == Tile.Type.Room_Wall_Edge_Left_Top)
                {
                    standardTile.type = Tile.Type.Way_Floor_Top;
                    standardTileDown1.type = Tile.Type.Entrance_Left_Bottom;
                }
                else if (standardTile.type == Tile.Type.Room_Wall_Edge_Right_Top)
                {
                    standardTile.type = Tile.Type.Way_Floor_Top;
                    standardTileDown1.type = Tile.Type.Entrance_Right_Bottom;
                }
                else
                {
                    standardTile.type = Tile.Type.Room_Floor_Top;
                    standardTileDown1.type = Tile.Type.Room_Floor_Inner;
                }
            }
        }
    }
    // 방의 오른쪽 벽에서 예외 케이스 검사
    public void InspectedRightWall()
    {
        // 기준 타일
        Tile standardTile = default(Tile);
        // 기준 타일 한칸 오른쪽 타일
        Tile standardTileRight1 = default(Tile);
        // 기준 타일 한칸 왼쪽 타일
        Tile standardTileLeft1 = default(Tile);
        // 기준 타일 한칸 위 타일
        Tile standardTileUp1 = default(Tile);
        // 기준 타일 한칸 아래 타일
        Tile standardTileDown1 = default(Tile);

        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = WRB.y; i <= WRT.y; i++)
        {
            standardTile = tileArray[i, WRB.x];
            standardTileRight1 = tileArray[i, WRB.x + 1];
            standardTileLeft1 = tileArray[i, WRB.x - 1];
            standardTileUp1 = tileArray[i + 1, WRB.x];
            standardTileDown1 = tileArray[i - 1, WRB.x];

            // 오른쪽 벽의 오른쪽으로 한칸, 두칸이 모두 길 바닥일 경우 입구를 생성
            if (standardTileRight1.IsContainString("Way_Floor") && tileArray[i, WRB.x + 2].IsContainString("Way_Floor"))
            {
                standardTile.type = Tile.Type.Entrance_Floor_Right;
                standardTileLeft1.type = Tile.Type.Room_Floor_Inner;

                standardTileUp1.type = Tile.Type.Entrance_Top;
                standardTileDown1.type = Tile.Type.Entrance_Right_Bottom;
            }
            // 오른쪽 벽과 수직 길이 합쳐지는 경우
            else if (standardTileRight1.IsContainString("Way_Floor"))
            {
                // 방의 오른쪽벽을 바닥으로 변경하고 엣지의 경우 방향에 맞게 돌려줘야함
                if (standardTile.type == Tile.Type.Room_Wall_Edge_Right_Top)
                {
                    standardTile.type = Tile.Type.Entrance_Top;
                    standardTileRight1.type = Tile.Type.Way_Floor_NotTop;
                }
                else if (standardTile.type == Tile.Type.Room_Wall_Edge_Right_Bottom)
                {
                    standardTile.type = Tile.Type.Entrance_Bottom_Left;
                    standardTileRight1.type = Tile.Type.Way_Floor_NotTop;
                }
                else
                {
                    standardTile.type = Tile.Type.Room_Floor_Inner;
                    standardTileRight1.type = Tile.Type.Room_Floor_Inner;
                    standardTileLeft1.type = Tile.Type.Room_Floor_Inner;
                }
            }
            else if (standardTileRight1.type == Tile.Type.Way_Wall_Right)
            {
                if (standardTile.type == Tile.Type.Room_Wall_Edge_Right_Top)
                {
                    standardTile.type = Tile.Type.Entrance_Top;
                    standardTileRight1.type = Tile.Type.Way_Floor_NotTop;
                }
                else if (standardTile.type == Tile.Type.Room_Wall_Edge_Right_Bottom)
                {
                    standardTile.type = Tile.Type.Entrance_Bottom_Left;
                    standardTileRight1.type = Tile.Type.Way_Floor_NotTop;
                }
                else
                {
                    standardTile.type = Tile.Type.Room_Floor_Inner;
                    standardTileLeft1.type = Tile.Type.Room_Floor_Inner;
                }
            }
        }
    }
    // 방의 아랫쪽 벽에서 예외 케이스 검사
    public void InspectedBottomWall()
    {
        // 기준 타일
        Tile standardTile = default(Tile);
        // 기준 타일 한칸 위 타일
        Tile standardTileUp1 = default(Tile);
        // 기준 타일 한칸 아래 타일
        Tile standardTileDown1 = default(Tile);
        // 기준 타일 한칸 오른쪽 타일
        Tile standardTileRight1 = default(Tile);
        // 기준 타일 한칸 왼쪽 타일
        Tile standardTileLeft1 = default(Tile);

        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = WLB.x; i <= WRB.x; i++)
        {
            standardTile = tileArray[WLB.y, i];
            standardTileUp1 = tileArray[WLB.y + 1, i];
            standardTileDown1 = tileArray[WLB.y - 1, i];
            standardTileRight1 = tileArray[WLB.y, i + 1];
            standardTileLeft1 = tileArray[WLB.y, i - 1];

            // 아래 벽의 아래쪽 한칸, 두칸이 모두 길의 바닥인 경우(수직 길인 경우)
            if (standardTileDown1.IsContainString("Way_Floor") && tileArray[WLB.y - 2, i].IsContainString("Way_Floor"))
            {
                standardTile.type = Tile.Type.Entrance_Floor_Bottom;
                standardTileUp1.type = Tile.Type.Room_Floor_Inner;

                standardTileLeft1.type = Tile.Type.Entrance_Bottom_Left;
                standardTileRight1.type = Tile.Type.Entrance_Bottom_Right;
            }
            // 아래 벽의 아래쪽 한칸만 길의 바닥인 경우 (방과 길을 합쳐야 하는 경우)
            else if (standardTileDown1.IsContainString("Way_Floor"))
            {
                if (standardTile.type == Tile.Type.Room_Wall_Edge_Left_Bottom)
                {
                    standardTile.type = Tile.Type.Entrance_Top;
                    standardTileDown1.type = Tile.Type.Way_Floor_Top;
                }
                else if (standardTile.type == Tile.Type.Room_Wall_Edge_Right_Bottom)
                {
                    standardTile.type = Tile.Type.Entrance_Top;
                    standardTileDown1.type = Tile.Type.Way_Floor_Top;
                }
                else
                {
                    standardTileUp1.type = Tile.Type.Room_Floor_Inner;
                    standardTile.type = Tile.Type.Room_Floor_Inner;
                    standardTileDown1.type = Tile.Type.Room_Floor_Inner;
                }
            }
            else if (standardTileDown1.type == Tile.Type.Way_Wall_Bottom)
            {
                if (standardTile.type == Tile.Type.Room_Wall_Edge_Left_Bottom)
                {
                    standardTileUp1.type = Tile.Type.Entrance_Top;
                    standardTile.type = Tile.Type.Way_Floor_Top;
                }
                else if (standardTile.type == Tile.Type.Room_Wall_Edge_Right_Bottom)
                {
                    standardTileUp1.type = Tile.Type.Entrance_Top;
                    standardTile.type = Tile.Type.Way_Floor_Top;
                }
                else
                {
                    standardTileUp1.type = Tile.Type.Room_Floor_Inner;
                    standardTile.type = Tile.Type.Room_Floor_Inner;
                }
            }
        }
    }
    #endregion
}
