using System.Net.WebSockets;
using UnityEngine;

public enum Dir
{
    LEFT,
    RIGHT,
    TOP,
    BOTTOM
}

public class Room
{
    #region Fields
    private int x, y, w, h;
    private int size;
    #endregion

    #region Properties
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

    public int Width { get; set; }
    public int Height { get; set; }
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
    }

    // 방의 바닥의 가장자리 여부를 체크
    public bool IsBorderByFloor(Dir dir, int x, int y)
    {
        bool result = default(bool);

        if (dir == Dir.LEFT)
            result = (x == LB.x && (y >= LB.y && y <= LT.y));
        else if (dir == Dir.RIGHT)
            result = (x == RB.x && (y >= RB.y && y <= RT.y));
        else if (dir == Dir.TOP)
            result = (x >= LT.x && x <= RT.x) && y == LT.y;
        else if (dir == Dir.BOTTOM)
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
        else if (dir == Dir.TOP)
            result = (x >= WLT.x && x <= WRT.x) && y == WLT.y;
        else if (dir == Dir.BOTTOM)
            result = (x >= WLT.x && x <= WRT.x) && y == WLB.y;

        return result;
    }

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

    public void InitRoomTileType()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = x; i < x + w; i++)
        {
            for (int j = y; j < y + h; j++)
            {
                // tileArray[j, i].color = Color.cyan;
                tileArray[j, i].type = Tile.Type.Ground_Inner;

                if (IsBorderByFloor(Dir.BOTTOM, i, j))
                {
                    // 방의 아랫쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Bottom;
                    tileArray[j - 1, i].type = Tile.Type.Room_Wall_Bottom;

                    // 방의 가장 좌측 아랫쪽 바닥의 경우 여기서 처리
                    // if (i == x) tileArray[j, i - 1].type = Tile.Type.Room_Wall_Left;
                }
                if (IsBorderByFloor(Dir.LEFT, i, j))
                {
                    // 방의 왼쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Left;
                    tileArray[j, i - 1].type = Tile.Type.Room_Wall_Left;

                    // 방의 가장 좌측 위쪽 바닥의 경우 여기서 처리
                    // if (j == y + h - 1) tileArray[j + 1, i].type = Tile.Type.Room_Wall_Top;
                }
                if (IsBorderByFloor(Dir.TOP, i, j))
                {
                    // 방의 윗쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Top;
                    tileArray[j + 1, i].type = Tile.Type.Room_Wall_Top;
                }
                if (IsBorderByFloor(Dir.RIGHT, i, j))
                {
                    // 방의 오른쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Right;
                    tileArray[j, i + 1].type = Tile.Type.Room_Wall_Right;
                }




                // 방의 모서리에 해당하는 바닥타일과, 벽을 마지막으로 덮는다.
                if (i == LB.x && j == LB.y)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Left_Bottom;
                    tileArray[j - 1, i - 1].type = Tile.Type.Room_Wall_Edge_Left_Bottom;
                }
                else if (i == RT.x && j == RT.y)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Right_Top;
                    tileArray[j + 1, i + 1].type = Tile.Type.Room_Wall_Edge_Right_Top;
                }
                else if (i == LT.x && j == LT.y)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Left_Top;
                    tileArray[j + 1, i - 1].type = Tile.Type.Room_Wall_Edge_Left_Top;
                }
                else if (i == RB.x && j == RB.y)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Right_Bottom;
                    tileArray[j - 1, i + 1].type = Tile.Type.Room_Wall_Edge_Right_Bottom;
                }
            }
        }

        // // 방의 각 모서리 바닥 타일 셋팅
        // tileArray[y + 1, x + 1].type = Tile.Type.Ground_Edge_Left_Bottom;
        // tileArray[y + h - 2, x + w - 2].type = Tile.Type.Ground_Edge_Right_Top;
        // tileArray[y + h - 2, x + 1].type = Tile.Type.Ground_Edge_Left_Top;
        // tileArray[y + 1, x + w - 2].type = Tile.Type.Ground_Edge_Right_Bottom;
    }

    // 방의 윗쪽 벽에서 예외 케이스 검사
    public void InspectedTopWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = WLT.x; i <= WRT.x; i++)
        {
            // 윗쪽 벽의 윗쪽 한칸, 두칸이 모두 길의 바닥인 경우(수직 길인 경우)
            if (tileArray[WLT.y + 1, i].type.ToString().Contains("Way_Floor") && tileArray[WLT.y + 2, i].type.ToString().Contains("Way_Floor"))
            {
                tileArray[WLT.y, i].type = Tile.Type.Way_Floor_NotTop;
                tileArray[WLT.y - 1, i].type = Tile.Type.Ground_Inner;
            }
            // 윗쪽 벽의 윗쪽 한칸만 길의 바닥인 경우(방과 길을 합쳐야 하는 경우)
            else if (tileArray[WLT.y + 1, i].type.ToString().Contains("Way_Floor"))
            {
                // tileArray[WLT.y + 1, i].color = Color.black;
                tileArray[WLT.y, i].type = Tile.Type.Ground_Inner;
            }
        }
    }
    // 방의 아랫쪽 벽에서 예외 케이스 검사
    public void InspectedBottomWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = WLB.x; i <= WRB.x; i++)
        {
            // 아래 벽의 아래쪽 한칸, 두칸이 모두 길의 바닥인 경우(수직 길인 경우)
            if (tileArray[WLB.y - 1, i].type.ToString().Contains("Way_Floor") && tileArray[WLB.y - 2, i].type.ToString().Contains("Way_Floor"))
            {
                tileArray[WLB.y + 1, i].type = Tile.Type.Ground_Inner;

                tileArray[WLB.y, i].type = Tile.Type.Way_Floor_NotTop;
                tileArray[WLB.y, i - 1].type = Tile.Type.Entrance_Bottom_Left;
                tileArray[WLB.y, i + 1].type = Tile.Type.Entrance_Bottom_Right;
            }
            // 아래 벽의 아래쪽 한칸만 길의 바닥인 경우 (방과 길을 합쳐야 하는 경우)
            else if (tileArray[y - 2, i].type.ToString().Contains("Way_Floor"))
            {
                tileArray[WLB.y, i].type = Tile.Type.Ground_Bottom;
            }
        }
    }
    // 방의 왼쪽 벽에서 예외 케이스 검사
    public void InspectedLeftWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = WLB.y; i <= WLT.y; i++)
        {
            // 왼쪽 벽의 왼쪽으로 한칸, 두칸이 모두 길 바닥일 경우 입구를 생성
            if (tileArray[i, WLB.x - 1].type.ToString().Contains("Way_Floor") && tileArray[i, WLB.x - 2].type.ToString().Contains("Way_Floor"))
            {
                tileArray[i, WLB.x + 1].type = Tile.Type.Ground_Inner;

                tileArray[i, WLB.x].type = Tile.Type.Way_Floor_Top;

                tileArray[i + 1, WLB.x - 1].type = Tile.Type.Entrance_Top;
                tileArray[i - 1, WLB.x - 1].type = Tile.Type.Entrance_Left_Bottom;
            }
            // 왼쪽 벽과 수직 길이 합쳐지는 경우
            else if (tileArray[i, WLB.x - 1].type.ToString().Contains("Way_Floor"))
            {
                // 방의 왼쪽벽을 바닥으로 변경하고 엣지의 경우 방향에 맞게 돌려줘야함
                tileArray[i, WLB.x - 1].type = Tile.Type.Ground_Inner;
                // if( i == )
                tileArray[i, WLB.x].type = Tile.Type.Ground_Inner;

                if (tileArray[i, WLB.x - 1].type == Tile.Type.Room_Wall_Edge_Left_Top) tileArray[i, WLB.x - 1].type = Tile.Type.Room_Wall_Edge_Left_Bottom;
                if (tileArray[i, WLB.x - 1].type == Tile.Type.Room_Wall_Edge_Left_Bottom) tileArray[i, WLB.x - 1].type = Tile.Type.Room_Wall_Edge_Left_Top;
            }
        }
    }
    // 방의 오른쪽 벽에서 예외 케이스 검사
    public void InspectedRightWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = WRB.y; i <= WRT.y; i++)
        {
            // 오른쪽 벽의 오른쪽으로 한칸, 두칸이 모두 길 바닥일 경우 입구를 생성
            if (tileArray[i, WRB.x + 1].type.ToString().Contains("Way_Floor") && tileArray[i, WRB.x + 2].type.ToString().Contains("Way_Floor"))
            {
                tileArray[i, WRB.x].type = Tile.Type.Ground_Inner;

                tileArray[i, WRB.x].type = Tile.Type.Way_Floor_Top;

                tileArray[i + 1, WRB.x].type = Tile.Type.Entrance_Top;
                tileArray[i - 1, WRB.x].type = Tile.Type.Entrance_Right_Bottom;
            }
            // 오른쪽 벽과 수직 길이 합쳐지는 경우
            else if (tileArray[i, WRB.x + 1].type.ToString().Contains("Way_Floor"))
            {
                // 방의 오른쪽벽을 바닥으로 변경하고 엣지의 경우 방향에 맞게 돌려줘야함
                tileArray[i, WRB.x].type = Tile.Type.Ground_Inner;

                if (tileArray[i, WRB.x].type == Tile.Type.Room_Wall_Edge_Right_Top) tileArray[i, WRB.x].type = Tile.Type.Room_Wall_Edge_Right_Bottom;
                if (tileArray[i, WRB.x].type == Tile.Type.Room_Wall_Edge_Right_Bottom) tileArray[i, WRB.x].type = Tile.Type.Room_Wall_Edge_Right_Top;
            }
        }
    }
}
