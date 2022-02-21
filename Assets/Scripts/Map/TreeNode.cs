using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TreeNode
{
    public Container self;
    public TreeNode leftChild;
    public TreeNode rightChild;
    public TreeNode parent;
    public TreeNode(Container self)
    {
        this.self = self;
        this.leftChild = null;
        this.rightChild = null;
    }

    public List<Container> GetLeafs()
    {
        List<Container> result = new List<Container>();

        if (this.leftChild == null && this.rightChild == null)
        {
            result.Add(this.self);
        }
        else
        {
            result.AddRange(this.leftChild?.GetLeafs());
            result.AddRange(this.rightChild?.GetLeafs());
        }
        return result;
    }

    public void Paint()
    {
        this.self.PaintWall(MapManager.Instance.TileArray);

        if (this.leftChild != null)
            this.leftChild.Paint();
        if (this.rightChild != null)
            this.rightChild.Paint();
    }

    public void InitTileWayType(int minWidth)
    {
        if (this.leftChild == null || this.rightChild == null)
            return;

        int left_center_x = Mathf.RoundToInt(this.leftChild.self.center.x);
        int left_center_y = Mathf.RoundToInt(this.leftChild.self.center.y);
        int right_center_x = Mathf.RoundToInt(this.rightChild.self.center.x);
        int right_center_y = Mathf.RoundToInt(this.rightChild.self.center.y);

        //TODO 자식 노드의 시작위치, 크기를 비교해 길의 너비와 위치를 조절하는 로직이 필요한상태
        int wayWidth = UnityEngine.Random.Range(minWidth, 3);
        if (left_center_x == right_center_x)
        {
            // 수평 분할 된 자식
            for (int i = left_center_y; i < right_center_y; i++)
            {

                for (int j = 0; j < wayWidth; j++)
                {
                    MapManager.Instance.TileArray[i, left_center_x + j].type = Tile.Type.Way_Floor_NotTop;

                }
            }
        }
        else if (left_center_y == right_center_y)
        {
            // 수직 분할 된 자식
            for (int i = left_center_x; i < right_center_x; i++)
            {
                MapManager.Instance.TileArray[left_center_y, i].type = Tile.Type.Way_Floor_Top;
                MapManager.Instance.TileArray[left_center_y - 1, i].type = Tile.Type.Way_Floor_NotTop;
            }
        }

        this.leftChild.InitTileWayType(minWidth);
        this.rightChild.InitTileWayType(minWidth);
    }
}