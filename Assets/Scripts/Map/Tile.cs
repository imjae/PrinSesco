using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Fields
    private SpriteRenderer spriteRenderer;
    private Vector2 _coordinate;
    public Type _type;
    #endregion

    #region Properties
    [field: SerializeField] public Vector2 Coordinate { get; set; }
    // public Color color
    // {
    //     set => spriteRenderer.color = value;
    //     get => spriteRenderer.color;
    // }

    public int sortingOrder
    {
        set => spriteRenderer.sortingOrder = value;
        get => spriteRenderer.sortingOrder;
    }

    public Sprite sprite
    {
        set => spriteRenderer.sprite = value;
        get => spriteRenderer.sprite;
    }

    public Type type
    {
        set => _type = value;
        get => _type;
    }
    #endregion

    #region Enums
    public enum Type
    {
        Ground_Inner,
        Ground_Top,
        Ground_Bottom,
        Ground_Left,
        Ground_Right,
        Ground_Edge_Left_Top,
        Ground_Edge_Left_Bottom,
        Ground_Edge_Right_Top,
        Ground_Edge_Right_Bottom,
        Wall_Edge_Left_Top,
        Wall_Edge_Right_Top,
        Wall_Edge_Left_Bottom,
        Wall_Edge_Right_Bottom,
        Wall_Left,
        Wall_Right,
        Wall_Top,
        Wall_Bottom,
        Way_Bottom,
        Way_Bottom_Floor,
        Way_Top,
        Way_Top_Floor,
        Corner_Left_Top,
        Corner_Left_Bottom,
        Corner_Right_Top,
        Corner_Right_Bottom,
        Corner_Top_Left,
        Corner_Top_Right,
        Corner_Bottom_Left,
        Corner_Bottom_Right,
        RoundDoor_Left,
        RoundDoor_Right,
        SquareDoor_Left,
        SquareDoor_Right,
        VENT,
        Ladder,
        Big_Rock,
        Small_Rock,
        Ceiling,
        Floor,
        Dark
    }
    #endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (!TryGetComponent<SpriteRenderer>(out spriteRenderer))
        {
            Debug.LogError("You need to SpriteRenderer for Block");
        }
    }

    #region Functions
    

    #endregion
}
