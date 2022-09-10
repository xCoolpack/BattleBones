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
    public Vector3Int ThreeAxisCoordinates;

    // References
    public FieldType Type;
    public List<Player> SeenBy; 
    public Building Building; 
    public Unit Unit;
    public GameMap GameMap;

    private void Awake()
    {
        Coordinates = ConvertPositionToCoordinates(transform.position);
        ThreeAxisCoordinates = CoordinatesConverter.To3Axis(Coordinates);
    }

    private void Start()
    {
        GameMap = GetComponentInParent<GameMap>();
    }

    private void OnMouseDown()
    {
        // Temporary for testing
        GameObject.Find("TestUnit").GetComponent<Unit>().Move(this);
        //Debug.Log(IsVisibleFor(GameObject.Find("TestUnit").GetComponent<Unit>().Field));
        //EventHandler eventHandler = GameObject.Find("GlobalEventHandler").GetComponent<EventHandler>();

        //GameEvent gameEvent = new GameEvent(1, () =>
        //{
        //    var unitO = GameObject.Find("TestUnit");
        //    if (unitO is null)
        //    {
        //        Debug.Log("Is null!");
        //        return;
        //    }
        //    var unit = unitO.GetComponent<Unit>();
        //    unit.Move(this);
        //});
        //eventHandler.AddStartTurnEvent(gameEvent);
    }

    /// <summary>
    /// Method converting transform position to coordinates
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
    public bool IsBlockingSight()
    {
        return Type.IsBlockingSight;
    }

    public bool HasUnit()
    {
        return Unit != null;
    }
    public bool HasBuidling()
    {
        return Building != null;
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
            if (GameMap.FieldGrid.ContainsKey(Coordinates + direction))
            {
                neighbors.Add(GameMap.FieldGrid[Coordinates + direction]);
            }
        }

        return neighbors;
    }

    public bool IsVisibleFor(Field field)
    {
        int x = System.Math.Sign(ThreeAxisCoordinates.x - field.ThreeAxisCoordinates.x) * -1;
        int y = System.Math.Sign(ThreeAxisCoordinates.y - field.ThreeAxisCoordinates.y) * -1;
        int z = System.Math.Sign(ThreeAxisCoordinates.z - field.ThreeAxisCoordinates.z) * -1;

        //Debug.Log($"x:{x}, y:{y}, z:{z}");
        if (x == 0 && y == 0 && z == 0)
        {
            return true;
        }
        else if (x + y == 0 && x + z == 0)
        {
            
            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, 0))?.IsBlockingSight() ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, 0, z))?.IsBlockingSight() ?? true);
        }
        else if (y + x == 0 && y + z == 0)
        {
            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(0, y, z))?.IsBlockingSight() ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, 0))?.IsBlockingSight() ?? true);
        }
        else if (z + x == 0 && z + y == 0)
        {
            //Debug.Log(new Vector3Int(x, 0, z)+ " - " + new Vector3Int(0, y, z));
            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, 0, z))?.IsBlockingSight() ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(0, y, z))?.IsBlockingSight() ?? true);
        }
        else if (x == 0 || y == 0 || z == 0)
        {
            //Debug.Log(new Vector3Int(x, y, z));
            Field next = GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, z));
            return (!next?.IsBlockingSight() ?? true) || next == field;
        }

        return false;
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

