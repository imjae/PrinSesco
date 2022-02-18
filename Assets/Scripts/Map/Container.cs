using UnityEngine;
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
                    tileArray[j, i].color = Color.gray;

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