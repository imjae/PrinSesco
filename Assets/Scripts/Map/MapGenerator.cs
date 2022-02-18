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

        // containerTree.Paint();
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