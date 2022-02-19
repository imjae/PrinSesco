using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Fields
    private SpriteRenderer spriteRenderer;
    private Vector2 _coordinate;
    Type _type;
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
        set
        {
            //TODO 설정되는 value에 따라 spriteRenderer의 Sprite를 알맞게 변경
        }
        get => _type;
    }
    #endregion

    #region Enums
    public enum Type
    {
        Ground,
        Ground_Edge_Left_Top,
        Ground_Edge_Right_Top,
        Ground_Edge_Left_Bottom,
        Ground_Edge_Right_Bottom,
        WALL_LEFT,
        WALL_RIGHT,
        WALL_TOP,
        WALL_BOTTOM,
        WALL_LEFT_TOP,
        WALL_RIGHT_TOP,
        WALL_LEFT_BOTTOM,
        WALL_RIGHT_BOTTOM,
        WAY_BOTTOM,
        WAY_TOP,
        WAY_RIGHT_TOP_START,
        WAY_RIGHT_BOTTOM_START,
        WAY_LEFT_TOP_START,
        WAY_LEFT_BOTTOM_START,
        WAY_BOTTOM_LEFT_START,
        WAY_BOTTOM_RIGHT_START,
        WAY_TOP_LEFT_START,
        WAY_TOP_RIGHT_START,
        STRUCTURE_ROUNDDOOR_LEFT,
        STRUCTURE_ROUNDDOOR_RIGHT,
        STRUCTURE_SQUAREDOOR_LEFT,
        STRUCTURE_SQUAREDOOR_RIGHT,
        STRUCTURE_VENT,
        STRUCTURE_LADDER,
        STRUCTURE_BIG_ROCK,
        STRUCTURE_CEILLING,
        STRUCTURE_FLOOR,
        STRUCTURE_DARK,
        STRUCTURE_SMALL_ROCK
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
    public void ChangeSpriteByType(Type type)
    {
        string stringType = type.ToString();
        string category = stringType.Split('_')[0];

        if(category.Equals("GROUND"))
        {

        }
        else if(category.Equals("WALL"))
        {

        }
        else if(category.Equals("WAY"))
        {
            
        }
        else if(category.Equals("STRUCTURE"))
        {
            
        }
    }

    #endregion
}
