using UnityEngine;

public class Room
{
    private int x, y, w, h;
    private int size;

    public Room(Container container)
    {
        this.x = container.x + Random.Range(1, container.w / 3);
        this.y = container.y + Random.Range(1, container.h / 3);
        this.w = container.w - (this.x - container.x);
        this.h = container.h - (this.y - container.y);
        this.w -= Random.Range(1, this.w / 4);
        this.h -= Random.Range(1, this.h / 4);
        this.size = this.w * this.h;
    }

    public void InitTileType(Tile[,] tileArray)
    {
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
                    tileArray[j - 1, i].type = Tile.Type.Wall_Bottom;

                    // 방의 가장 좌측 아랫쪽 바닥의 경우 여기서 처리
                    // if (i == x) tileArray[j, i - 1].type = Tile.Type.Wall_Left;
                }
                if (i == x)
                {
                    // 방의 왼쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Left;
                    tileArray[j, i - 1].type = Tile.Type.Wall_Left;

                    // 방의 가장 좌측 위쪽 바닥의 경우 여기서 처리
                    // if (j == y + h - 1) tileArray[j + 1, i].type = Tile.Type.Wall_Top;
                }
                if (j == y + h - 1)
                {
                    // 방의 윗쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Top;
                    tileArray[j + 1, i].type = Tile.Type.Wall_Top;
                }
                if (i == x + w - 1)
                {
                    // 방의 오른쪽 벽, 바닥 타일 타입 셋팅
                    tileArray[j, i].type = Tile.Type.Ground_Right;
                    tileArray[j, i + 1].type = Tile.Type.Wall_Right;
                }




                // 방의 모서리에 해당하는 바닥타일과, 벽을 마지막으로 덮는다.
                if (i == x && j == y)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Left_Bottom;
                    tileArray[j - 1, i - 1].type = Tile.Type.Wall_Edge_Left_Bottom;
                }
                else if (i == x + w - 1 && j == y + h - 1)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Right_Top;
                    tileArray[j + 1, i + 1].type = Tile.Type.Wall_Edge_Right_Top;
                }
                else if (i == x && j == y + h - 1)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Left_Top;
                    tileArray[j + 1, i - 1].type = Tile.Type.Wall_Edge_Left_Top;
                }
                else if (i == x + w - 1 && j == y)
                {
                    tileArray[j, i].type = Tile.Type.Ground_Edge_Right_Bottom;
                    tileArray[j - 1, i + 1].type = Tile.Type.Wall_Edge_Right_Bottom;
                }

            }
        }
    }
}
