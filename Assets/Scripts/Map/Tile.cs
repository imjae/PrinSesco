using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Fields
    private SpriteRenderer spriteRenderer;
    private Vector2Int _realCoordinate;
    public Type _type;
    #endregion

    #region Properties
    [field: SerializeField] public Vector2Int Coordinate { get; set; }
    [field: SerializeField] public Vector2Int RealCoordinate { get; set; }
    public bool IsStructure { get; set; }
    public Color color
    {
        set => spriteRenderer.color = value;
        get => spriteRenderer.color;
    }

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
        Room_Wall_Edge_Left_Top,
        Room_Wall_Edge_Right_Top,
        Room_Wall_Edge_Left_Bottom,
        Room_Wall_Edge_Right_Bottom,
        Room_Wall_Left,
        Room_Wall_Right,
        Room_Wall_Top,
        Room_Wall_Bottom,
        Way_Bottom_Floor,
        Way_Top_Floor,
        Way_Wall_Top,
        Way_Wall_Bottom,
        Way_Wall_Left,
        Way_Wall_Right,
        Way_Floor_Top,              // 길(Way)을 두 타일을 사용하기로 정함 위쪽과 아래쪽 타일의 종류가 다름
        Way_Floor_NotTop,           // 길(Way)은 위쪽 바닥과 나머지 바닥으로 나뉘어짐(수직 방향의 길은 전부 이 타일을 사용)
        Entrance_Floor_Top,
        Entrance_Floor_Bottom,
        Entrance_Floor_Left,
        Entrance_Floor_Right,
        Entrance_Top,               // 수평, 수직 위치의 입구에서 윗부분(Top)에 해당하는 타일은 전부 같은 종류의 타일을 사용하기 때문에 하나로 합침(Left_Top, Right_Top, Top_Left, Top_Right)
        Entrance_Left_Bottom,
        Entrance_Right_Bottom,
        Entrance_Bottom_Left,
        Entrance_Bottom_Right,
        Door_Vertical_Left,
        Door_Vertical_Right,
        Door_Horizontal_Center_Up,
        Door_Horizontal_Center_Down,
        Door_Horizontal_Left_Up,
        Door_Horizontal_Left_Down,
        Door_Horizontal_Right_Up,
        Door_Horizontal_Right_Down,
        VENT,
        Ladder,
        Big_Rock,
        Small_Rock,
        Ceiling,
        Floor,
        Dark
    }
    public enum Layer
    {
        Background = 1,
        Structure = 2
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
    // 두 번째 인자로 받은 타입이 맞는지 확인하는 메소드
    public bool IsContainString(string typePrefix)
    {
        bool result = false;

        result = this.type.ToString().Contains(typePrefix);

        return result;
    }

    #endregion
}
