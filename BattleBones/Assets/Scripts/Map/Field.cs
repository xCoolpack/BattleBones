using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Field : MonoBehaviour
{
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
    private Overlay _overlay;


    private void Awake()
    {
        Coordinates = ConvertPositionToCoordinates(transform.position);
        ThreeAxisCoordinates = CoordinatesConverter.To3Axis(Coordinates);
    }

    private void Start()
    {
        GameMap = GetComponentInParent<GameMap>();
        _overlay = GameObject.Find("Overlay").GetComponent<Overlay>();
    }

    private void OnMouseDown()
    {
        if (Unit != null && Building != null)
        {
            if (Unit == _overlay.PickedUnit)
            {
                Building.HandleOnClick();
                return;
            }
            Unit.HandleOnClick();
            return;
        }

        if (Building != null)
        {
            Building.HandleOnClick();
            return;
        }

        if (Unit != null)
        {
            Unit.HandleOnClick();
            return;
        }

        HandleOnClick();
    }

    public void HandleOnClick()
    {
        _overlay.ClearPicked();
        _overlay.PickedField = this;
        _overlay.FieldInfoBox();
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
    public bool IsBlockingSightFor(Unit unit)
    {
        return unit.MovementScript.IsBlockingSight(this);
    }

    public bool HasUnit()
    {
        return Unit != null;
    }
    public bool HasBuilding()
    {
        return Building != null;
    }

    public int GetMovementPointsCost()
    {
        return Type.MovementPointsCost;
    }

    public List<Building> GetAllowableBuildings()
    {
        return Type.AllowableBuildings;
    }

    public UnitModifiers GetUnitModifiersFromBuilding()
    {
        return Building?.GetUnitModifiers() ?? new UnitModifiers();
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

    public bool IsVisibleFor(Unit unit, Field field)
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
            
            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, 0))?.IsBlockingSightFor(unit) ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, 0, z))?.IsBlockingSightFor(unit) ?? true);
        }
        else if (y + x == 0 && y + z == 0)
        {
            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(0, y, z))?.IsBlockingSightFor(unit) ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, 0))?.IsBlockingSightFor(unit) ?? true);
        }
        else if (z + x == 0 && z + y == 0)
        {
            //Debug.Log(new Vector3Int(x, 0, z)+ " - " + new Vector3Int(0, y, z));
            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, 0, z))?.IsBlockingSightFor(unit) ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(0, y, z))?.IsBlockingSightFor(unit) ?? true);
        }
        else if (x == 0 || y == 0 || z == 0)
        {
            //Debug.Log(new Vector3Int(x, y, z));
            Field next = GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, z));
            return (!next?.IsBlockingSightFor(unit) ?? true) || next == field;
        }

        return false;
    }

    public bool IsVisibleFor(Building building, Field field)
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

            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, 0))?.Type.IsBlockingSight ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, 0, z))?.Type.IsBlockingSight ?? true);
        }
        else if (y + x == 0 && y + z == 0)
        {
            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(0, y, z))?.Type.IsBlockingSight ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, 0))?.Type.IsBlockingSight ?? true);
        }
        else if (z + x == 0 && z + y == 0)
        {
            //Debug.Log(new Vector3Int(x, 0, z)+ " - " + new Vector3Int(0, y, z));
            return (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, 0, z))?.Type.IsBlockingSight ?? true)
                   && (!GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(0, y, z))?.Type.IsBlockingSight ?? true);
        }
        else if (x == 0 || y == 0 || z == 0)
        {
            //Debug.Log(new Vector3Int(x, y, z));
            Field next = GameMap.GetFieldAt(ThreeAxisCoordinates + new Vector3Int(x, y, z));
            return (!next?.Type.IsBlockingSight ?? true) || next == field;
        }

        return false;
    }

    public bool CanConstruct(Player player, string buildingName)
    {
        GameObject buildingPrefab = player.AvailableBuildings.FirstOrDefault(g => g.name == buildingName);
        if (buildingPrefab is null)
            return false;
        return buildingPrefab.GetComponent<Unit>().CanAffordRecruitment(player) && !HasBuilding();
    }

    public void BeginBuildingConstruction(Player player, string buildingName)
    {
        GameObject buildingPrefab = player.AvailableBuildings.FirstOrDefault(g => g.name == buildingName);
        Building building = Instantiate(buildingPrefab, this.transform).GetComponent<Building>();
        building.Player = player;
        building.Field = this;
        Building = building;
        building.BuildingState = BuildingState.UnderConstruction;
        player.AddBuilding(Building);
        player.ResourceManager.RemoveAmount(building.BaseBuildingStats.BaseCost);
        player.PlayerEventHandler.AddStartTurnEvent(new GameEvent(1, building.Construct));
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

