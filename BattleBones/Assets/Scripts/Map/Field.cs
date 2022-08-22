using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    //IMPORTANT
    //LEAVE MARK AS FIRST CHILD IN FIELD GAMEOBJECT PLS

    // Graphical Offset
    public static float xOffset = 2.56f, yOffset = 1.93f;

    // Coordinates
    public Vector2Int coordinates;

    // References
    public FieldType type;
    public List<Object> seenBy; //temp - replace Object with Player
    public GameObject building; //temp - replace Object with Building
    public Unit unit; 

    private void Awake()
    {
        coordinates = ConvertPositionToCoordinates(transform.position);
    }

    private void OnMouseDown()
    {
        // Temporary for testing
        Debug.Log("Clicked on "+coordinates);
        GameObject.Find("TestUnit").GetComponent<Unit>().Move(this);
        
    }

    /// <summary>
    /// Methods converting transform position to coordinates
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector2Int ConvertPositionToCoordinates(Vector2 position)
    {
        int x = Mathf.CeilToInt((float)System.Math.Round(position.x, 2) / xOffset);
        int y = Mathf.CeilToInt((float)System.Math.Round(position.y, 2) / yOffset);
        return new Vector2Int(x, y);
    }

    public bool IsObstacle()
    {
        return type.isObstacle;
    }

    public bool HasUnit()
    {
        return unit != null ? true : false;
    }

    public int GetMovementPointsCost()
    {
        return type.movementPointsCost;
    }

    /// <summary>
    /// Methods returning list of neighbours of field
    /// </summary>
    /// <returns></returns>
    public List<Field> GetNeighbours()
    {
        List<Field> neighbours = new();
        Dictionary<Vector2Int, Field> fieldGrid = GetComponentInParent<GameMap>().fieldGrid;

        foreach (Vector2Int direction in Direction.GetDirectionList(coordinates.y))
        {
            if (fieldGrid.ContainsKey(coordinates + direction))
            {
                neighbours.Add(fieldGrid[coordinates + direction]);
            }
        }

        return neighbours;
    } 
}

public static class Direction
{
    public static List<Vector2Int> offsetEven = new()
    {
        new Vector2Int(-1, 1), //NL
        new Vector2Int(0, 1), //NR
        new Vector2Int(1, 0), //E
        new Vector2Int(0, -1), //SR
        new Vector2Int(-1, -1), //SL
        new Vector2Int(-1, 0), //W
    };

    public static List<Vector2Int> offsetOdd = new()
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
        return y % 2 == 0 ? offsetEven : offsetOdd;
    }
}

