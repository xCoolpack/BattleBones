using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Field : MonoBehaviour
{
    //IMPORTANT
    //LEAVE MARK AS FIRST CHILD IN FIELD GAMEOBJECT PLS

    // Graphical Offset
    public static float XOffset = 2.56f, YOffset = 1.93f;

    // Coordinates
    public Vector2Int Coordinates;

    // References
    public FieldType Type;
    public List<Object> SeenBy; //temp - replace Object with Player
    public Building Building; //temp - replace Object with Building
    public Unit Unit;
    public Dictionary<Vector2Int, Field> FieldGrid;

    private void Awake()
    {
        Coordinates = ConvertPositionToCoordinates(transform.position);
    }

    private void Start()
    {
        FieldGrid = GetComponentInParent<GameMap>().FieldGrid;
    }

    private void OnMouseDown()
    {
        // Temporary for testing
        Debug.Log("Clicked on "+Coordinates);
        GameObject.Find("TestUnit").GetComponent<Unit>().Move(this);
        
    }

    /// <summary>
    /// Methods converting transform position to coordinates
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector2Int ConvertPositionToCoordinates(Vector2 position)
    {
        var x = Mathf.CeilToInt((float)System.Math.Round(position.x, 2) / XOffset);
        var y = Mathf.CeilToInt((float)System.Math.Round(position.y, 2) / YOffset);
        return new Vector2Int(x, y);
    }

    public bool IsObstacle()
    {
        return Type.IsObstacle;
    }

    public bool HasUnit()
    {
        return Unit != null;
    }

    public int GetMovementPointsCost()
    {
        return Type.MovementPointsCost;
    }

    /// <summary>
    /// Methods returning list of neighbors of field
    /// </summary>
    /// <returns></returns>
    public List<Field> GetNeighbors()
    {
        List<Field> neighbors = new();

        foreach (var direction in Direction.GetDirectionList(Coordinates.y))
        {
            if (FieldGrid.ContainsKey(Coordinates + direction))
            {
                neighbors.Add(FieldGrid[Coordinates + direction]);
            }
        }

        return neighbors;
    }

    public override string ToString()
    {
        return $"{Type.FieldName} ({Coordinates.x}, {Coordinates.y})";
    }
}

public static class Direction
{
    public static List<Vector2Int> OffsetEven = new()
    {
        new Vector2Int(-1, 1), //NL
        new Vector2Int(0, 1), //NR
        new Vector2Int(1, 0), //E
        new Vector2Int(0, -1), //SR
        new Vector2Int(-1, -1), //SL
        new Vector2Int(-1, 0), //W
    };

    public static List<Vector2Int> OffsetOdd = new()
    {
        new Vector2Int(0, 1), //NL
        new Vector2Int(1, 1), //NR
        new Vector2Int(1, 0), //E
        new Vector2Int(1, -1), //SR
        new Vector2Int(0, -1), //SL
        new Vector2Int(-1, 0), //W
    };

    public static List<Vector2Int> GetDirectionList(int y)
    {
        return y % 2 == 0 ? OffsetEven : OffsetOdd;
    }
}

