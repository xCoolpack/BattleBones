using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static float xOffset = 2.56f, yOffset = 1.93f;

    // Coordinates
    public Vector2Int coordinates;

    // References
    public FieldType type;
    public List<Object> seeenBy; //temp - replace Object with Player
    public Object building; //temp - replace Object with Building
    public Object unit; //temp - replace Object with Unit

    private void Awake()
    {
        coordinates = ConvertPositionToCoordinates(transform.position);
    }

    private Vector2Int ConvertPositionToCoordinates(Vector2 position)
    {
        int x = Mathf.CeilToInt((float)System.Math.Round(position.x, 2) / xOffset);
        int y = Mathf.CeilToInt((float)System.Math.Round(position.y, 2) / yOffset);
        return new Vector2Int(x, y);
    }

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

