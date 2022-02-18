using UnityEngine;

public class Room
{
    private int x, y, w, h;

    public Room(Container container)
    {
        this.x = container.x + Random.Range(1, container.w / 3);
        this.y = container.y + Random.Range(1, container.h / 3);
        this.w = container.w - (this.x - container.x);
        this.h = container.h - (this.y - container.y);
        this.w -= Random.Range(1, this.w / 3);
        this.h -= Random.Range(1, this.h / 3);
    }

    public void PaintGround(Tile[,] tileArray)
    {
        for (int i = x; i < x + w; i++)
        {
            for (int j = y; j < y + h; j++)
            {
                tileArray[j, i].color = Color.cyan;
                tileArray[j, i].type = Tile.Type.GROUND;
            }
        }
    }
}
