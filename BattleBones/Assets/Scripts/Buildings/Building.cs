using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public enum BuildingState
{
    UnderConstruction,
    Fine,
    Plundered,
    UnderRepair,
    None
} 


public class Building : MonoBehaviour
{
    // Max stats
    public int MaxHealth;
    public readonly int BaseRepairCooldown = 1; 

    // Current stats
    public int CurrentHealth;
    public int SightRange;

    // References 
    public BaseBuildingStats BaseBuildingStats;
    public Player Player;
    public Field Field;
    public BuildingState BuildingState;
    public BuildingState PreviousBuildingState;
    public GameEvent BuildingGameEvent;
    private Overlay _overlay;
    public List<Field> VisibleFields;

    public HashSet<Player> DiscoveredBy;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        VisibleFields = new List<Field>();
        DiscoveredBy = new HashSet<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        SetStats();
        if (Player != Player.HumanPlayer) Hide(Player.HumanPlayer);
        
    }

    private void Start()
    {
        _overlay = GameObject.Find("Overlay").GetComponent<Overlay>();
        // Set building visibility

        foreach (var pair in Field.SeenBy)
        {
            if (pair.Value > 0)
                Discover(pair.Key);
        }

        // Building defined in scene
        if (BuildingState == BuildingState.Fine)
        {
            ShowFields();

            Construct();
            PreviousBuildingState = BuildingState.UnderConstruction;
        }
    }

    /// <summary>
    /// Methods setting unit stats from BaseBuildingStats object
    /// </summary>
    private void SetStats()
    {
        MaxHealth = BaseBuildingStats.BaseHealth;
        CurrentHealth = BaseBuildingStats.BaseHealth;
        SightRange = BaseBuildingStats.BaseSightRange;
    }

    public bool IsEnemy(Player player)
    {
        return Player != player;
    }

    public string GetBuildingStateName()
    {
        return BuildingState switch
        {
            BuildingState.UnderConstruction => "Construction",
            BuildingState.Fine => "Fine",
            BuildingState.Plundered => "Plundered",
            BuildingState.UnderRepair => "Under repair",
            _ => ""
        };
    }

    public bool IsPassable(Player player)
    {
        return !IsEnemy(player) 
               || BaseBuildingStats.IsPassable 
               || BuildingState is BuildingState.Plundered or BuildingState.UnderConstruction 
               || PreviousBuildingState == BuildingState.Plundered; 
    }

    public bool CanBeTargeted(Player player)
    {
        return IsEnemy(player) 
               && BuildingState != BuildingState.Plundered 
               && PreviousBuildingState != BuildingState.Plundered
               && !IsPassable(player);
    }

    public bool CanAffordConstruction(Player player)
    {
        return player.ResourceManager.ResourcesAmount >= BaseBuildingStats.BaseCost;
    }

    public bool CanAffordRepair()
    {
        return Player.ResourceManager.ResourcesAmount >= BaseBuildingStats.BaseCost / 2;
    }

    private void SetVisibleFields()
    {
        VisibleFields = GetVisibleFields();
        VisibleFields.Add(Field);
    }

    private List<Field> GetVisibleFields()
    {
        return GraphSearch.BreadthFirstSearchList(Field, SightRange,
            (currentField, startingField) => currentField.IsVisibleFor(this, startingField), _ => 1);
    }

    public void Discover(Player player)
    {
        DiscoveredBy.Add(player);
        if (player == Player.HumanPlayer)
            _spriteRenderer.enabled = true;
    }

    public void Hide(Player player)
    {
        if (player == Player.HumanPlayer)
            _spriteRenderer.enabled = false;
    }

    public void ShowFields()
    {
        SetVisibleFields();
        foreach (var field in VisibleFields)
        {
            field.Discover(Player);
        }
    }

    public void HideFields()
    {
        SetVisibleFields();
        foreach (var field in VisibleFields)
        {
            field.Hide(Player);
        }
    }

    public UnitModifiers GetUnitModifiers()
    {
        if (BuildingState != BuildingState.Fine)
            return new UnitModifiers();
        return GetComponent<Barricade>()?.BuildingUnitModifier ?? new UnitModifiers();
    }

    public void Construct()
    {
        if (this == null) return;

        CurrentHealth = MaxHealth;
        Field.Unit?.AddUnitModifiers(Field.Building.GetUnitModifiers());

        ShowFields();

        // Methods in "derived" scripts
        GetComponent<IncomeBuilding>()?.Construct();
        GetComponent<Housing>()?.Construct();

        PreviousBuildingState = BuildingState;
        BuildingState = BuildingState.Fine;

        Logger.Log($"{Player.name} has finished construction of {BaseBuildingStats.BuildingName} at {Field.ThreeAxisCoordinates}");
    }


    public void TakeDamage(int damage)
    {
        if (BuildingState == BuildingState.UnderConstruction)
        {
            Player.PlayerEventHandler.RemoveStartTurnEvent(BuildingGameEvent);
            Destroy();
            return;
        }
        else if (BuildingState == BuildingState.UnderRepair)
        {
            Player.PlayerEventHandler.RemoveStartTurnEvent(BuildingGameEvent);
            BuildingState = PreviousBuildingState;
            
            BuildingGameEvent = null;
            Logger.Log($"Repair of {Player.name}'s {BaseBuildingStats.BuildingName} at {Field.ThreeAxisCoordinates} has been stopped ");
            return;
        }

        CurrentHealth -= damage;

        if (CurrentHealth <= 0) 
            Plunder();
    }

    public void Plunder()
    {
        Logger.Log($"{Player.name}'s {BaseBuildingStats.BuildingName} at {Field.ThreeAxisCoordinates} has been plundered");

        Field.Unit?.RemoveUnitModifiers(Field.Building.GetUnitModifiers());
        CurrentHealth = 0;

        HideFields();

        // Methods in "derived" scripts
        GetComponent<IncomeBuilding>()?.Plunder();
        GetComponent<Housing>()?.Plunder();

        PreviousBuildingState = BuildingState;
        BuildingState = BuildingState.Plundered;
    }

    public bool CanRepair()
    {
        return CurrentHealth < MaxHealth && CanAffordRepair() && BuildingState != BuildingState.UnderRepair;
    }

    public void BeginRepair()
    {
        Player.ResourceManager.RemoveAmount(BaseBuildingStats.BaseCost/2);
        BuildingGameEvent = new GameEvent(BaseRepairCooldown, Repair);

        PreviousBuildingState = BuildingState;
        BuildingState = BuildingState.UnderRepair;

        Player.PlayerEventHandler.AddStartTurnEvent(BuildingGameEvent);
    }

    public void Repair()
    {
        Logger.Log($"Repair of {Player.name}'s {BaseBuildingStats.BuildingName} at {Field.ThreeAxisCoordinates} has been finished ");

        CurrentHealth = MaxHealth;

        if (PreviousBuildingState == BuildingState.Plundered)
        {
            ShowFields();

            Field.Unit?.AddUnitModifiers(Field.Building.GetUnitModifiers());

            // Methods in "derived" scripts
            GetComponent<IncomeBuilding>()?.Repair();
            GetComponent<Housing>()?.Repair();
        }

        PreviousBuildingState = BuildingState;
        BuildingState = BuildingState.Fine;
    }

    public void Destroy()
    {
        Logger.Log($"{Player.name}'s {BaseBuildingStats.BuildingName} at {Field.ThreeAxisCoordinates} has been destroyed");
        
        HideFields();

        // Methods in "derived" scripts
        GetComponent<IncomeBuilding>()?.Destroy();
        GetComponent<Housing>()?.Destroy();

        Player.RemoveBuilding(this);
        Field.Unit?.RemoveUnitModifiers(Field.Building.GetUnitModifiers());
        Field.Building = null;     
        Destroy(gameObject);
    }

    public void HandleOnClick()
    {
        _overlay.ClearPicked();
        _overlay.PickedBuilding = this;
        var defensiveBuilding = GetComponent<DefensiveBuilding>();
        var showButtons = Player.IsPlayersTurn();
        
        if (defensiveBuilding is not null)
        {
            defensiveBuilding.SetAttackableFields();

            var outpost = GetComponent<Outpost>();

            if(outpost is not null)
            {
                _overlay.OutpostInfoBox(defensiveBuilding, outpost, showButtons);
                return;
            }

            _overlay.DefensiveBuildingInfoBox(defensiveBuilding, showButtons);
            return;
        }

        var incomeBuilding = GetComponent<IncomeBuilding>();

        if (incomeBuilding is not null)
        {
            _overlay.IncomeBuildingInfoBox(incomeBuilding, showButtons);
            return;
        }

        var barricade = GetComponent<Barricade>();

        if (barricade is not null)
        {
            _overlay.BarricadeInfoBox(barricade, showButtons);
            return;
        }

        var housing = GetComponent<Housing>();

        if (housing is not null)
        {
            _overlay.HousingInfoBox(housing, showButtons);
            return;
        }

        _overlay.WatchtowerInfobox(showButtons);
    }
}
