using UnityEngine;

public class Room
{
    #region Fields
    private int x, y, w, h;
    private int size;
    #endregion

    #region Properties
    public Vector2Int Coordinate { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    #endregion

    public Room(Container container)
    {
        this.x = container.x + UnityEngine.Random.Range(2,4);
        this.y = container.y + UnityEngine.Random.Range(2,4);
        this.w = container.w - (this.x - container.x);
        this.h = container.h - (this.y - container.y);
        this.w -= UnityEngine.Random.Range(2,4);
        this.h -= UnityEngine.Random.Range(2,4);

        this.Coordinate = new Vector2Int(this.x, this.y);
        this.Width = w;
        this.Height = h;
        this.size = this.w * this.h;
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

                if (j == y)
                {
                    // 방의 아랫쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Bottom;
                    tileArray[j - 1, i].type = Tile.Type.Room_Wall_Bottom;

                    // 방의 가장 좌측 아랫쪽 바닥의 경우 여기서 처리
                    // if (i == x) tileArray[j, i - 1].type = Tile.Type.Room_Wall_Left;
                }
                if (i == x)
                {
                    // 방의 왼쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Left;
                    tileArray[j, i - 1].type = Tile.Type.Room_Wall_Left;

                    // 방의 가장 좌측 위쪽 바닥의 경우 여기서 처리
                    // if (j == y + h - 1) tileArray[j + 1, i].type = Tile.Type.Room_Wall_Top;
                }
                if (j == y + h - 1)
                {
                    // 방의 윗쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Top;
                    tileArray[j + 1, i].type = Tile.Type.Room_Wall_Top;
                }
                if (i == x + w - 1)
                {
                    // 방의 오른쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Right;
                    tileArray[j, i + 1].type = Tile.Type.Room_Wall_Right;
                }




                // 방의 모서리에 해당하는 바닥타일과, 벽을 마지막으로 덮는다.
                if (i == x && j == y)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Left_Bottom;
                    tileArray[j - 1, i - 1].type = Tile.Type.Room_Wall_Edge_Left_Bottom;
                }
                else if (i == x + w - 1 && j == y + h - 1)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Right_Top;
                    tileArray[j + 1, i + 1].type = Tile.Type.Room_Wall_Edge_Right_Top;
                }
                else if (i == x && j == y + h - 1)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Left_Top;
                    tileArray[j + 1, i - 1].type = Tile.Type.Room_Wall_Edge_Left_Top;
                }
                else if (i == x + w - 1 && j == y)
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

    public void InspectedTopWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = x - 1; i < x + w + 1; i++)
        {
            // tileArray[y + h, i].color = Color.black;
            if (tileArray[y + h + 1, i].type.ToString().Contains("Way_Floor"))
            {
                tileArray[y + h, i].type = Tile.Type.Way_Floor_NotTop;
                tileArray[y + h - 1, i].type = Tile.Type.Ground_Inner;
            }
        }
    }
    public void InspectedBottomWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = x - 1; i < x + w + 1; i++)
        {
            // tileArray[y - 1, i].color = Color.black;
            if (tileArray[y - 2, i].type.ToString().Contains("Way_Floor"))
            {
                tileArray[y, i].type = Tile.Type.Way_Floor_NotTop;

                tileArray[y - 1, i].type = Tile.Type.Ground_Inner;
                tileArray[y - 1, i - 1].type = Tile.Type.Entrance_Bottom_Left;
                tileArray[y - 1, i + 1].type = Tile.Type.Entrance_Bottom_Right;
            }
        }
    }

    public void InspectedLeftWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = y - 1; i < y + h + 1; i++)
        {
            // 왼쪽 벽의 왼쪽으로 한칸, 두칸이 모두 길 바닥일 경우 입구를 생성
            if (tileArray[i, x - 2].type.ToString().Contains("Way_Floor") && tileArray[i, x - 3].type.ToString().Contains("Way_Floor"))
            {
                tileArray[i, x - 1].type = Tile.Type.Way_Floor_Top;
                tileArray[i, x].type = Tile.Type.Ground_Inner;

                tileArray[i + 1, x - 1].type = Tile.Type.Entrance_Top;
                tileArray[i - 1, x - 1].type = Tile.Type.Entrance_Left_Bottom;
            }
            // 왼쪽 벽과 수직 길이 합쳐지는 경우
            else if(tileArray[i, x - 2].type.ToString().Contains("Way_Floor"))
            {
                // 방의 왼쪽벽을 바닥으로 변경하고 엣지의 경우 방향에 맞게 돌려줘야함
                tileArray[i, x - 1].type = Tile.Type.Ground_Inner;

                if(tileArray[i, x - 1].type == Tile.Type.Room_Wall_Edge_Left_Top) tileArray[i, x - 1].type = Tile.Type.Room_Wall_Edge_Left_Bottom;
                if(tileArray[i, x - 1].type == Tile.Type.Room_Wall_Edge_Left_Bottom) tileArray[i, x - 1].type = Tile.Type.Room_Wall_Edge_Left_Top;
            }
        }
    }

    public void InspectedRightWall()
    {
        Tile[,] tileArray = MapManager.Instance.TileArray;
        for (int i = y - 1; i < y + h + 1; i++)
        {
            // 오른쪽 벽의 오른쪽으로 한칸, 두칸이 모두 길 바닥일 경우 입구를 생성
            if (tileArray[i, x + w + 1].type.ToString().Contains("Way_Floor") && tileArray[i, x + w + 2].type.ToString().Contains("Way_Floor"))
            {
                tileArray[i, x + w].type = Tile.Type.Way_Floor_Top;
                tileArray[i, x + w - 1].type = Tile.Type.Ground_Inner;

                tileArray[i + 1, x + w].type = Tile.Type.Entrance_Top;
                tileArray[i - 1, x + w].type = Tile.Type.Entrance_Right_Bottom;
            }
            // 오른쪽 벽과 수직 길이 합쳐지는 경우
            else if(tileArray[i, x + w + 1].type.ToString().Contains("Way_Floor"))
            {
                // 방의 오른쪽벽을 바닥으로 변경하고 엣지의 경우 방향에 맞게 돌려줘야함
                tileArray[i, x + w].type = Tile.Type.Ground_Inner;

                if(tileArray[i, x + w].type == Tile.Type.Room_Wall_Edge_Right_Top) tileArray[i, x + w].type = Tile.Type.Room_Wall_Edge_Right_Bottom;
                if(tileArray[i, x + w].type == Tile.Type.Room_Wall_Edge_Right_Bottom) tileArray[i, x + w].type = Tile.Type.Room_Wall_Edge_Right_Top;
            }
        }
    }
}
