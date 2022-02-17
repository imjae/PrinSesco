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
        GROUND,
        WALL,
        WAY,
        WATER,
        LAVA
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

    void OnMouseDown()
    {
        Debug.Log("click!");
    }
}
