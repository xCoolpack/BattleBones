using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public enum BuildingState
{
    UnderConstruction,
    Fine,
    Plundered,
    UnderRepair,
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
        Hide(Player.HumanPlayer);
    }

    private void Start()
    {
        _overlay = GameObject.Find("Overlay").GetComponent<Overlay>();
        // Set building visibility
        foreach (Player key in Field.SeenBy.Keys)
        {
            Discover(key);
        }

        ShowFields();
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
        BuildingState = BuildingState.Fine;
        Field.Unit?.AddUnitModifiers(Field.Building.GetUnitModifiers());

        // Methods in "derived" scripts
        GetComponent<IncomeBuilding>()?.Construct();
        GetComponent<Housing>()?.Construct();
    }


    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0) 
            Plunder();
    }

    public void Plunder()
    {
        BuildingState = BuildingState.Plundered;
        Field.Unit?.RemoveUnitModifiers(Field.Building.GetUnitModifiers());

        // Methods in "derived" scripts
        GetComponent<IncomeBuilding>()?.Plunder();
        GetComponent<Housing>()?.Plunder();
    }

    public bool CanRepair()
    {
        return CanAffordRepair();
    }

    public void BeginRepair()
    {
        BuildingState = BuildingState.UnderRepair;
        Player.ResourceManager.RemoveAmount(BaseBuildingStats.BaseCost/2);
        Player.PlayerEventHandler.AddStartTurnEvent(new GameEvent(BaseRepairCooldown, Repair));
    }

    public void Repair()
    {
        BuildingState = BuildingState.Fine;
        CurrentHealth = MaxHealth;
        Field.Unit?.AddUnitModifiers(Field.Building.GetUnitModifiers());

        // Methods in "derived" scripts
        GetComponent<IncomeBuilding>()?.Repair();
        GetComponent<Housing>()?.Repair();
    }

    public void Destroy()
    {
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

        throw new System.Exception("Unknown building was clicked"); 
    }
}
