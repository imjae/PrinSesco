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

    public void PaintWay()
    {
        if (this.leftChild == null || this.rightChild == null)
            return;

        int left_x = Mathf.RoundToInt(this.leftChild.self.center.x);
        int left_y = Mathf.RoundToInt(this.leftChild.self.center.y);
        int right_x = Mathf.RoundToInt(this.rightChild.self.center.x);
        int right_y = Mathf.RoundToInt(this.rightChild.self.center.y);

        if (left_x == right_x)
        {
            // 수평 분할 된 자식
            for (int i = left_y; i < right_y; i++)
            {
                // MapManager.Instance.TileArray[i, left_x].color = Color.cyan;
                // MapManager.Instance.TileArray[i, left_x].type = Tile.Type.WAY;
            }
        }
        else if (left_y == right_y)
        {
            // 수직 분할 된 자식
            for (int i = left_x; i < right_x; i++)
            {
                // MapManager.Instance.TileArray[left_y, i].color = Color.cyan;
                // MapManager.Instance.TileArray[i, left_x].type = Tile.Type.WAY;
            }
        }

        this.leftChild.PaintWay();
        this.rightChild.PaintWay();
    }
}