using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Field : MonoBehaviour
{
    // Graphical Offset
    public static float XOffset = 2.56f, YOffset = 1.93f;

    // Coordinates
    public Vector2Int Coordinates;
    public Vector3Int ThreeAxisCoordinates;

    // References
    public FieldType Type;
    public Building Building;
    public Unit Unit;
    public GameMap GameMap;
    private SpriteRenderer _spriteRenderer;
    private Overlay _overlay;
    public Mark Mark_;
    public GameObject ChosenMark;

    public HashSet<Player> DiscoveredBy;
    // int stand for number of entities that see that field
    public Dictionary<Player, int> SeenBy;
    [SerializeField] private Sprite _fogOfWarSprite;
    private Sprite _fieldSprite;

    private void Awake()
    {
        DiscoveredBy = new HashSet<Player>();
        SeenBy = new Dictionary<Player, int>();
        var turnHandler = GameObject.Find("TurnHandler").GetComponent<TurnHandler>();
        SeenBy[turnHandler.HumanPlayer] = 0;
        SeenBy[turnHandler.ComputerPlayerObj.GetComponent<Player>()] = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _fieldSprite = _spriteRenderer.sprite;
        _spriteRenderer.sprite = _fogOfWarSprite;
        Coordinates = ConvertPositionToCoordinates(transform.position);
        ThreeAxisCoordinates = CoordinatesConverter.To3Axis(Coordinates);
        GameMap = GameObject.Find("GameMap").GetComponent<GameMap>();
        ChosenMark = GetChosenMark();
    }

    private void Start()
    {
        _overlay = GameObject.Find("Overlay").GetComponent<Overlay>();
    }

    private void OnMouseOver()
    {
        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            if (_overlay.IsPointerOverUI)
            {
                return;
            }

            if (Mark_ == Mark.Movable)
            {
                _overlay.PickedUnit.TurnOffChosenMark();
                _overlay.PickedUnit.Move(this);
                _overlay.UnitInfoBox(true);
                _overlay.PickedUnit.ToggleOffAllMarks();
                //_overlay.PickedUnit.UpdateAndDisplayMarks();
                //_overlay.PickedUnit.TurnOnChosenMark();
            }
            else if (Mark_ == Mark.Attackable)
            {
                _overlay.PickedUnit.TurnOffChosenMark();
                _overlay.PickedUnit.Attack(this);
                _overlay.UnitInfoBox(true);
                _overlay.PickedUnit.ToggleOffAllMarks();
                //_overlay.PickedUnit.UpdateAndDisplayMarks();
                //_overlay.PickedUnit.TurnOnChosenMark();
            }
        }
    }

    // Left click
    private void OnMouseDown()
    {
        if (_overlay.IsPointerOverUI)
        {
            return;
        }

        if (Unit != null && Building != null)
        {
            if (Unit == _overlay.PickedUnit)
            {
                Building.HandleOnClick();
                TurnOnChosenMark();
                return;
            }
            Unit.HandleOnClick();
            TurnOnChosenMark();
            return;
        }

        if (Building != null)
        {
            Building.HandleOnClick();
            TurnOnChosenMark();
            return;
        }

        if (Unit != null)
        {
            Unit.HandleOnClick();
            TurnOnChosenMark();
            return;
        }

        HandleOnClick();
        TurnOnChosenMark();
    }

    public void TurnOnChosenMark()
    {
        if (IsSeenByCurrentPlayer())
        {
            Mark_ = Mark.Chosen;
            ChosenMark.SetActive(true);
        }
    }

    public void TurnOffChosenMark()
    {
        Mark_ = Mark.Unmarked;
        ChosenMark.SetActive(false);
    }

    private GameObject GetChosenMark()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "ChosenMark")
                return child.gameObject;
        }

        return null;
    }

    private bool IsSeenByCurrentPlayer()
    {
        return IsSeenBy(_overlay.TurnHandler.CurrentPlayer);
    }

    /// <summary>
    /// Handles on click for this field
    /// </summary>
    public void HandleOnClick()
    {
        if (IsSeenByCurrentPlayer())
        {
            _overlay.ClearPicked();
            _overlay.PickedField = this;
            _overlay.FieldInfoBox();
            return;
        }

        _overlay.ClearPicked();
        _overlay.RemoveInfoBox();
    }

    /// <summary>
    /// Method converting transform position to coordinates
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector2Int ConvertPositionToCoordinates(Vector2 position)
    {
        var x = (int)Math.Ceiling(Math.Round(Math.Round(transform.position.x, 2) / XOffset, 2));
        var y = (int)Math.Ceiling(Math.Round(Math.Round(transform.position.y, 2) / YOffset, 2));
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

    #region FogOfWar

    public bool IsDiscoveredBy(Player player)
    {
        return DiscoveredBy.Contains(player);
    }

    public void Discover(Player player)
    {
        DiscoveredBy.Add(player);
        if (player == Player.HumanPlayer)
            _spriteRenderer.sprite = _fieldSprite;
        
        Show(player);
    }

    public void Show(Player player)
    {
        Building?.Discover(player);

        if (SeenBy.Increase(player))
        {
            _spriteRenderer.color = Color.white;
            Unit?.Show(player);
        }
    }

    public void Hide(Player player)
    {
        if (SeenBy.Decrease(player))
        {
            _spriteRenderer.color = Color.grey;
            Unit?.Hide(player);
        }
    }

    public bool IsSeenBy(Player player)
    {
        return SeenBy.ContainsKey(player) && SeenBy[player] > 0;
    }

    public bool IsBuildingDiscoveredBy(Player player)
    {
        return Building?.DiscoveredBy.Contains(player) ?? false;
    }

    #endregion

    public int GetMovementPointsCost()
    {
        return Type.MovementPointsCost;
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
        GameObject buildingPrefab = player.AvailableBuildings.FirstOrDefault(g => g.name == $"{player.Faction} {buildingName}");
        //if (buildingName == "Defensive tower")
        //{
        //    Debug.Log(buildingName);
        //    Debug.Log(buildingPrefab != null);
        //    Debug.Log(IsSeenBy(player));
        //    Debug.Log(!HasBuilding());
        //    Debug.Log(buildingPrefab.GetComponent<Building>().CanAffordConstruction(player));
        //    Debug.Log((buildingName != "Farm" || !GetNeighbors().Any(field => field.HasBuilding() && field.Building.BaseBuildingStats.BuildingName == "Farm")));
        //}

        return buildingPrefab != null 
               && IsSeenBy(player) 
               && !HasBuilding() 
               && buildingPrefab.GetComponent<Building>().CanAffordConstruction(player)
               && (buildingName != "Farm" || !GetNeighbors().Any(field => field.HasBuilding() && field.Building.BaseBuildingStats.BuildingName == "Farm"));
    }

    public void BeginBuildingConstruction(Player player, string buildingName)
    {
        GameObject buildingPrefab = player.AvailableBuildings.FirstOrDefault(g => g.name == $"{player.Faction} {buildingName}");
        Building building = Instantiate(buildingPrefab, transform).GetComponent<Building>();
        building.Player = player;
        building.Field = this;
        building.CurrentHealth = 0;
        Building = building;
        building.PreviousBuildingState = BuildingState.None;
        building.BuildingState = BuildingState.UnderConstruction;
        player.AddBuilding(Building);
        player.ResourceManager.RemoveAmount(building.BaseBuildingStats.BaseCost);

        building.BuildingGameEvent = new GameEvent(1, building.Construct);
        player.PlayerEventHandler.AddStartTurnEvent(building.BuildingGameEvent);

        Logger.Log($"{player.name} has begun construction of {buildingName} at {ThreeAxisCoordinates}");
    }

    public override string ToString()
    {
        return $"{Type.FieldName} ({Coordinates.x}, {Coordinates.y})";
    }
    public enum Mark
    {
        Unmarked,
        Attackable,
        Movable,
        Chosen
    }
}

public static class Direction
{
    public static List<Vector2Int> OffsetEven = new()
    {
        new Vector2Int(1, 1), //NR
        new Vector2Int(1, 0), //E
        new Vector2Int(1, -1), //SR
        new Vector2Int(0, -1), //SL
        new Vector2Int(-1, 0), //W
        new Vector2Int(0, 1), //NL
    };

    public static List<Vector2Int> OffsetOdd = new()
    {
        new Vector2Int(0, 1), //NR
        new Vector2Int(1, 0), //E
        new Vector2Int(0, -1), //SR
        new Vector2Int(-1, -1), //SL
        new Vector2Int(-1, 0), //W
        new Vector2Int(-1, 1), //NL
    };

    public static List<Vector2Int> GetDirectionList(int y)
    {
        return y % 2 == 0 ? OffsetEven : OffsetOdd;
    }
}
