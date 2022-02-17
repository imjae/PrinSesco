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


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        manager.width = width;
        manager.height = height;
        manager.InitializeTiles();

        Container mainContainer = new Container(0, 0, width, height);
        TreeNode containerTree = mainContainer.SplitContainer(mainContainer, iterationNumber, widthRatio, heightRatio);

        containerTree.Paint();
        containerTree.PaintWay();

        containerTree.GetLeafs().ForEach(node =>
        {
            new Room(node).PaintGround(MapManager.Instance.TileArray);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}

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
                MapManager.Instance.TileArray[left_x, i].color = Color.cyan;
                MapManager.Instance.TileArray[left_x, i].type = Tile.Type.WAY;
            }
        }
        else if (left_y == right_y)
        {
            // 수직 분할 된 자식
            for (int i = left_x; i < right_x; i++)
            {
                MapManager.Instance.TileArray[i, left_y].color = Color.cyan;
                MapManager.Instance.TileArray[left_x, i].type = Tile.Type.WAY;
            }
        }

        this.leftChild.PaintWay();
        this.rightChild.PaintWay();
    }
}
public class Container
{
    public int x, y, w, h;
    public Vector2 center;

    public Container(int x, int y, int w, int h)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        this.center = new Vector2
        {
            x = this.x + Mathf.RoundToInt(this.w * 0.5f),
            y = this.y + Mathf.RoundToInt(this.h * 0.5f)
        };
    }

    public void PaintWall(Tile[,] tileArray)
    {
        bool isPainting = false;
        for (int i = x; i < x + w; i++)
        {
            for (int j = y; j < y + h; j++)
            {
                if (i == x) isPainting = true;
                else if (i == x + w - 1) isPainting = true;
                else if (j == y) isPainting = true;
                else if (j == y + h - 1) isPainting = true;

                if (isPainting)
                    tileArray[i, j].color = Color.gray;

                isPainting = false;
            }
        }
    }

    public TreeNode SplitContainer(Container container, int count, float widthRatio, float heightRatio)
    {
        TreeNode root = new TreeNode(container);
        if (count > 0)
        {
            Container[] sr = RandomSplit(container, widthRatio, heightRatio);
            root.leftChild = SplitContainer(sr[0], count - 1, widthRatio, heightRatio);
            root.rightChild = SplitContainer(sr[1], count - 1, widthRatio, heightRatio);
        }
        return root;
    }

    private Container[] RandomSplit(Container container, float widthRatio, float heightRatio)
    {
        Container[] result = new Container[2];

        Container r1, r2;

        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0)
        {
            // 난수가 0 이면 수직 분할
            int tmp_r1_witdh = Random.Range(2, container.w);
            float r1_width_ratio = (float)tmp_r1_witdh / (float)container.h;
            float r2_width_ratio = (float)(container.w - tmp_r1_witdh) / (float)container.h;

            r1 = new Container(
                container.x, container.y,
                tmp_r1_witdh, container.h
            );
            r2 = new Container(
                container.x + r1.w, container.y,
                container.w - r1.w, container.h
            );

            if (r1_width_ratio < widthRatio || r2_width_ratio < widthRatio)
                return RandomSplit(container, widthRatio, heightRatio);
        }
        else
        {
            // 난수가 0이 아니면 수평 분할
            int tmp_r1_height = Random.Range(2, container.h);
            float r1_height_ratio = (float)tmp_r1_height / (float)container.w;
            float r2_height_ratio = (float)(container.h - tmp_r1_height) / (float)container.w;

            r1 = new Container(
                container.x, container.y,
                container.w, tmp_r1_height
            );
            r2 = new Container(
                container.x, container.y + r1.h,
                container.w, container.h - r1.h
            );


            if (r1_height_ratio < heightRatio || r2_height_ratio < heightRatio)
                return RandomSplit(container, widthRatio, heightRatio);
        }

        result[0] = r1;
        result[1] = r2;

        return result;
    }
}

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
                tileArray[i, j].color = Color.cyan;
                tileArray[i, j].type = Tile.Type.GROUND;
            }
        }
    }
}
