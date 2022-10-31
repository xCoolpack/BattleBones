using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Const 
    public const double HealRatio = 0.2;
    public const double DefenseRatio = 0.15;

    // Max Stats
    public int MaxHealth;
    public int MaxDamage;
    public int MaxDefense;
    public int MaxMovementPoints;

    // Current Stats
    public int CurrentHealth;
    public int CurrentDamage;
    public int CurrentDefense;
    public int CurrentMovementPoints;
    public int AttackRange;
    public int SightRange;

    // References
    public BaseUnitStats BaseUnitStats;
    public Player Player;
    public Field Field;
    public Movement MovementScript;
    public Attack AttackScript;
    public GameMap GameMap;

    public UnitModifiers CurrentModifiers;
    private UnitModifiers _currentUnitModifiersFormHealth;

    public List<Field> MoveableFields;
    private Dictionary<Field, int> _moveableFieldWithCost;
    public List<Field> VisibleFields;
    public List<Field> FieldsWithinAttackRange;
    public List<Field> AttackableFields;
    private Overlay _overlay;

    private UnitsCounter _unitsCounter;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        SetCurrentStats();
        SetStartingStats();
        // Temp
        _spriteRenderer = GetComponent<SpriteRenderer>();
        MovementScript = GetComponent<Movement>(); // if null then it's hero
        AttackScript = GetComponent<Attack>(); // if null then it's hero
        GameMap = GameObject.Find("GameMap").GetComponent<GameMap>();
        _unitsCounter = GameObject.Find("UnitsCounter").GetComponent<UnitsCounter>();
        Hide(Player.HumanPlayer);
    } 

    private void Update()
    {
        // Test necessary
        if (Input.GetKeyDown("r")) 
        {
            ShowFields();
            CurrentMovementPoints = MaxMovementPoints;
        }
    }

    private void Start()
    {
        _overlay = GameObject.Find("Overlay").GetComponent<Overlay>();
        // Set unit visibility
        ShowFields();

        foreach (var pair in Field.SeenBy)
        {
            if (pair.Value > 0)
                Show(pair.Key);
        }   
    }

    public void UpdateFieldSets()
    {
        SetVisibleFields();
        SetMoveableFields();
        SetAttackableFields();
    }

    /// <summary>
    /// Methods setting unit stats from BaseUnitStats object
    /// </summary>
    public void SetStartingStats()
    {
        CurrentHealth = MaxHealth;
        CurrentDamage = MaxDamage;
        CurrentDefense = MaxDefense;
        MaxMovementPoints = BaseUnitStats.BaseMovementPoints;
        CurrentMovementPoints = MaxMovementPoints;
        AttackRange = BaseUnitStats.BaseAttackRange;
        SightRange = BaseUnitStats.BaseSightRange;

        _currentUnitModifiersFormHealth = new UnitModifiers();
    }

    public void SetCurrentStats()
    {
        (MaxHealth, MaxDamage, MaxDefense) = CurrentModifiers.CalculateModifiers(this);
        CurrentDamage = MaxDamage;
        CurrentDefense = MaxDefense;
    }


    public void AddUnitModifiers(UnitModifiers unitModifiers)
    {
        CurrentModifiers += unitModifiers;
        SetCurrentStats();
    }

    public void ChangeModifiersFromHealth()
    {
        CurrentModifiers -= _currentUnitModifiersFormHealth;
        _currentUnitModifiersFormHealth = new UnitModifiers(damage: UnitCalculation.CalculateDamageModifier(CurrentHealth, MaxHealth));
        CurrentModifiers += _currentUnitModifiersFormHealth;
        SetCurrentStats();
    }

    public void RemoveUnitModifiers(UnitModifiers unitModifiers)
    {
        CurrentModifiers -= unitModifiers;
        SetCurrentStats();
    }

    public bool IsEnemy(Player player)
    {
        return Player != player;
    }

    public bool CanAffordRecruitment(Player player)
    {
        return player.ResourceManager.ResourcesAmount >= BaseUnitStats.BaseCost;
    }

    #region VisibleFields

    public void Show(Player player)
    {
        if (player == Player.HumanPlayer)
            _spriteRenderer.enabled = true;
    }

    public void Hide(Player player)
    {
        if (player == Player.HumanPlayer)
            _spriteRenderer.enabled = false;
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

    private void ToggleVisibleFields()
    {
        foreach (var field in VisibleFields)
        {
            var mark = field.transform.Find("AttackMark").gameObject;
            mark.SetActive(!mark.activeSelf);
        }
    }

    public void ShowFields()
    {
        SetVisibleFields();
        foreach (var field in VisibleFields)
            field.Discover(Player);
    }

    public void HideFields()
    {
        SetVisibleFields();
        foreach (var field in VisibleFields)
            field.Hide(Player);
    }

    public void ChangeFieldsVisibility(List<Field> list)
    {
        SetVisibleFields();
        foreach (var field in VisibleFields)
            field.Discover(Player);
        foreach (var field in list)
            field.Hide(Player);
    }

    #endregion 

    #region MoveableFields
    private void SetMoveableFields()
    {
        _moveableFieldWithCost = GetMoveableFields();
        MoveableFields = _moveableFieldWithCost.Keys.ToList();
    }

    private Dictionary<Field, int> GetMoveableFields() 
    {
       return GraphSearch.BreadthFirstSearchDict(Field, CurrentMovementPoints,
            (currentField, startingField) => MovementScript.CanMoveAll(this, currentField),
            (field) => MovementScript.GetMovementPointsCostAll(this, field));
    }

    public void ToggleOnMoveableFields() 
    {
        foreach (var field in MoveableFields)
        {
            field.Mark_ = Field.Mark.Movable;
            var mark = field.transform.Find("MoveMark").gameObject;
            mark.SetActive(true);
        }
    }

    public void ToggleOffMoveableFields()
    {
        foreach (var field in MoveableFields)
        {
            field.Mark_ = Field.Mark.Unmarked;
            var mark = field.transform.Find("MoveMark").gameObject;
            mark.SetActive(false);
        }
    }
    #endregion

    #region AttackableFields
    private void SetAttackableFields()
    {
        (FieldsWithinAttackRange, AttackableFields) = GetAttackableFields();
    }

    public (List<Field> FieldsWithinAttackRange, List<Field> AttackableFields) GetAttackableFields()
    {
        HashSet<Field> set = new();
        List<Field> list = new();
        List<Field> temp = new();
        //Attack range is always smaller or equal to sight range
        
        temp = GraphSearch.BreadthFirstSearchList(Field, AttackRange, 
            (currentField, startingField) => AttackScript.CanAttack(this, Field, currentField), _ => 1);
        foreach (var field in temp)
        {
            if (AttackScript.HaveEnoughMovementPoints(CurrentMovementPoints, this, field))
                set.Add(field);
        }
        foreach (var keyPair in _moveableFieldWithCost)
        {
            temp = GraphSearch.BreadthFirstSearchList(keyPair.Key, AttackRange, 
                (currentField, startingField) => AttackScript.CanAttack(this, keyPair.Key, currentField), _ => 1);
            foreach (var field in temp)
            {
                if (AttackScript.HaveEnoughMovementPoints(CurrentMovementPoints - keyPair.Value, this, field))
                    set.Add(field);
            }
        }
        foreach (Field field in set)
        {
            if (AttackScript.CanTarget(this, field)) 
                list.Add(field);
        }
        set.Remove(Field);

        return (set.ToList(), list);
    }

    private void ToggleFieldsWithinAttackRange()
    {
        foreach (var field in FieldsWithinAttackRange)
        {
            var mark = field.transform.Find("AttackMark").gameObject;
            mark.SetActive(!mark.activeSelf);
        }
    }

    public void ToggleOnAttackableFields()
    {
        foreach (var field in AttackableFields)
        {
            field.Mark_ = Field.Mark.Attackable;
            var mark = field.transform.Find("AttackMark").gameObject;
            mark.SetActive(true);
        }
    }

    public void ToggleOffAttackableFields()
    {
        foreach (var field in AttackableFields)
        {
            field.Mark_ = Field.Mark.Unmarked;
            var mark = field.transform.Find("AttackMark").gameObject;
            mark.SetActive(false);
        }
    }
    #endregion

    #region Move
    /// <summary>
    /// Generating path to target field
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="startingField"></param>
    /// <param name="targetField"></param>
    /// <returns></returns>
    private static List<Field> GeneratePathTo(Dictionary<Field, Field> graph, Field startingField, Field targetField)
    {
        List<Field> movementPath = new();
        Field currentField = targetField;
        movementPath.Add(currentField);
        while (graph[currentField] != startingField)
        {
            currentField = graph[currentField];
            movementPath.Add(currentField);
        }

        //Debug.Log("after while loop");
        movementPath.Reverse();
        return movementPath;
    }


    /// <summary>
    /// Move method for unit - responsible for defining A* search
    /// </summary>
    /// <param name="targetField"></param>
    /// <returns></returns>
    public bool Move(Field targetField)
    {
        if (targetField == Field)
            return false;
        (Dictionary<Field, Field> graph, _) = GraphSearch.AStarSearch(Field, targetField, 
            (currentField, startingField) => MovementScript.CanMoveAll(this, currentField), 
            (field) => MovementScript.GetMovementPointsCostAll(this, field), GraphSearch.GetDistance, targetField => false);
        
        MoveUnit(graph, targetField);

        return true;
    }

    /// <summary>
    /// Helper move method for unit - responsible for counting accessible path
    /// </summary>
    /// <param name="targetField"></param>
    private void MoveUnit(Dictionary<Field, Field> graph, Field targetField)
    {
        List<Field> movementPath = GeneratePathTo(graph, Field, targetField);
        var accessibleMovementPath = new List<Field>();
        int movementPointCost = 0;
        int nextMovementPointCost = 0;
        Field previousField = null;

        foreach (var field in movementPath)
        {
            nextMovementPointCost += MovementScript.GetMovementPointsCost(this, field);
            if (CurrentMovementPoints < nextMovementPointCost || !MovementScript.CanMoveVisible(this, field))
                break;

            accessibleMovementPath.Add(field);
            previousField = field;
            movementPointCost = nextMovementPointCost;
        }

        MoveReferences(previousField, movementPointCost, accessibleMovementPath);
    }

    /// <summary>
    /// Method responsible for moving references and graphical model of unit
    /// </summary>
    /// <param name="targetField"></param>
    /// <param name="movementPointCost"></param>
    /// <param name="accessibleMovementPath"></param>
    private void MoveReferences(Field targetField, int movementPointCost, List<Field> accessibleMovementPath)
    {
        // Rethink - apply show and hide methods for every position in movement path
        // Hide fields that unit no longer see
        SetVisibleFields();
        List<Field> temp = VisibleFields;

        // Remove modifiers from starting field
        RemoveUnitModifiers(Field.Type.FieldUnitModifiers);
        RemoveUnitModifiers(Field.GetUnitModifiersFromBuilding());

        // Add modifiers from target field
        AddUnitModifiers(targetField.Type.FieldUnitModifiers);
        AddUnitModifiers(targetField.GetUnitModifiersFromBuilding());


        // Move references between fields
        Field.Unit = null;
        Field = targetField;
        targetField.Unit = this;
        CurrentMovementPoints -= movementPointCost;

        // Show fields that unit now see
        ChangeFieldsVisibility(temp);

        // Show unit to every player that see this field
        foreach (var pair in Field.SeenBy)
            if (pair.Value > 0)
                Show(pair.Key);
            else if (pair.Value <= 0)
                Hide(pair.Key);

        MoveGraphicModel(accessibleMovementPath);
    }

    // Not working as intended, only the last field is set as parent
    // comeback later
    private void MoveGraphicModel(List<Field> movementPath)
    {
        foreach (Field field in movementPath)
        {
            this.transform.SetParent(field.transform, false);
        }
    }
    #endregion

    #region Attack
    /// <summary>
    /// Return all fields from which this unit can attack field
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public List<Field> GetFieldsFromUnitCanAttack(Field field)
    {
        List<Field> list = new();
        list.AddRange(GraphSearch.BreadthFirstSearchList(field, AttackRange,
            (currentField, startingField) => AttackScript.CanAttack(this, field, currentField), _ => 1));

        var i = 0;
        while (i < list.Count)
        {
            if (!MovementScript.CanMove(this, list[i]) && list[i] != Field)
                list.RemoveAt(i);
            else
                i++;
        }

        return list;
    }

    public bool CanMoveOrAttack(Field currentField, List<Field> possibleFieldsForAttack)
    {
        return MovementScript.CanMove(this, currentField) || possibleFieldsForAttack.Contains(currentField);
    }

    public void Attack(Field targetField)
    {
        if (targetField == Field)
            return ;

        //Find path to field from unit can attack
        List<Field> possibleFieldsForAttack = GetFieldsFromUnitCanAttack(targetField);
        
        // If unit cannot access attacked unit return false
        if (possibleFieldsForAttack.Count == 0)
            return ;

        //If unit cannot attack from current field, move it
        if (!possibleFieldsForAttack.Contains(Field))
        {
            (Dictionary<Field, Field> graph, Field attackingField) = GraphSearch.AStarSearch(Field, targetField,
                (currentField, startingField) => MovementScript.CanMoveAll(this, currentField),
                (field) => MovementScript.GetMovementPointsCostAll(this, field), GraphSearch.GetDistance,
                (currentField) => possibleFieldsForAttack.Contains(currentField));
            //Move unit to that field
            MoveUnit(graph, attackingField);
        }

        //Check if unit can attack and have enough movement points to attack
        if (possibleFieldsForAttack.Contains(Field) 
            && AttackScript.HaveEnoughMovementPoints(CurrentMovementPoints, this, targetField))
        {
            //Deal damage to unit
            if (AttackScript.CanTargetBuilding(this, targetField))
                DealDamage(targetField.Building);
            else
                DealDamage(targetField.Unit);
            //If unit is melee and destroy enemy unit, move unit to new position 
            if (AttackRange == 1 && MovementScript.CanMove(this, targetField))
                MoveReferences(targetField, MovementScript.GetMovementPointsCost(this, targetField),
                    new List<Field>() {targetField});

            CurrentMovementPoints = 0;
        }
    }

    public void DealDamage(Building building)
    {
        // If building has a unit, it does counterattack
        var damage = 0;
        var thisDamage = CurrentDamage;
        if (building.Field.HasUnit())
        {
            UnitModifiers unitModifiers = building.Field.Unit.CurrentModifiers 
                                          + new UnitModifiers(damage: building.Field.Unit.AttackScript.GetCounterAttackModifier());
            (_, damage, _) = unitModifiers.CalculateModifiers(0, building.Field.Unit.CurrentDamage, 0);
            
        }

        if (BaseUnitStats.UnitName is "Battering dog" or "Dog-a-pult")
            (_, thisDamage, _) = (CurrentModifiers + new UnitModifiers(damage: 150)).CalculateModifiers(0, CurrentDamage, 0);
        building.TakeDamage(thisDamage);
        if (AttackScript.IsProvokingCounterAttack() && building.Field.HasUnit())
            TakeDamage(damage);
    }

    public void DealDamage(Unit unit)
    {
        UnitModifiers unitModifiers = unit.CurrentModifiers 
                                      + new UnitModifiers(damage: unit.AttackScript.GetCounterAttackModifier());
        var (_, damage, _) = unitModifiers.CalculateModifiers(0, unit.CurrentDamage, 0);
        var (_, thisDamage, _) =
            GetCounterModifier(unit.BaseUnitStats.UnitName).CalculateModifiers(0, CurrentDamage, 0);


        unit.TakeDamage(thisDamage);
        if (AttackScript.IsProvokingCounterAttack())
            if (TakeDamage(damage))
                Player.UnitsKilled++;
    }

    /// <summary>
    /// Method applying dealt damage 
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool TakeDamage(int damage)
    {
        CurrentHealth -= UnitCalculation.CalculateDealtDamage(damage, CurrentDefense);

        if (CurrentHealth <= 0)
        {
            Delete();
            return true;
        }

        // Recalculate DamageModifiers from missing health
        ChangeModifiersFromHealth();
        return false;
    }

    public UnitModifiers GetCounterModifier(string defender)
    {
        return _unitsCounter.GetCounterModifier(defender, BaseUnitStats.UnitName, CurrentModifiers);
    }

    public int PredictDamage(Unit targetUnit)
    {
        var (_, damage, _) =
            GetCounterModifier(targetUnit.BaseUnitStats.UnitName).CalculateModifiers(0, CurrentDamage, 0);
        return UnitCalculation.CalculateDealtDamage(damage, targetUnit.CurrentDefense);
    }

    public bool CanPlunder()
    {
        return CurrentMovementPoints >= 1 && Field.HasBuilding() && Field.Building.IsEnemy(Player);
    }

    public void Plunder()
    {
        Field.Building.TakeDamage(CurrentDamage);
        CurrentMovementPoints -= 1;
    }

    #endregion

    public bool CanHeal()
    {
        return CurrentMovementPoints > 0 && CurrentHealth <= MaxHealth;
    }

    public void BeginHealing()
    {
        CurrentMovementPoints = 0;
        Player.PlayerEventHandler.AddEndTurnEvent(new GameEvent(1, Heal));
    }

    public void Heal()
    {
        CurrentHealth = Math.Min(CurrentHealth + (int)Math.Ceiling(HealRatio*MaxHealth), MaxHealth);
        ChangeModifiersFromHealth();
    }

    public bool CanDefend()
    {
        return CurrentMovementPoints > 0;
    }

    public void BeginDefending()
    {
        CurrentMovementPoints = 0;
        AddUnitModifiers(new UnitModifiers(defense: DefenseRatio));
        Player.PlayerEventHandler.AddStartTurnEvent(new GameEvent(1, Defend));
    }

    public void Defend()
    {
        RemoveUnitModifiers(new UnitModifiers(defense: DefenseRatio));
    }

    public void RestoreMovementPoints()
    {
        CurrentMovementPoints = MaxMovementPoints;
    }

    public void Delete()
    {
        HideFields();
        Field.Unit = null;
        Player.RemoveUnit(this);
        Destroy(gameObject);
    }

    public void HandleOnClick()
    {
        _overlay.ClearPicked();
        _overlay.PickedUnit = this;

        if (Player.IsPlayersTurn())
        {
            _overlay.UnitInfoBox(true);
            UpdateAndDisplayMarks();
        }
        else
        {
            _overlay.UnitInfoBox(false);
        }
    }

    /// <summary>
    /// Updates an displays all marks on map
    /// </summary>
    public void UpdateAndDisplayMarks()
    {
        //ShowFields();

        SetMoveableFields();
        SetAttackableFields();
        SetVisibleFields();
        //ToggleVisibleFields();
        ToggleOnMoveableFields();
        ToggleOnAttackableFields();
    }

    /// <summary>
    /// Clears map from all unit marks
    /// </summary>
    public void ToggleOffAllMarks()
    {
        ToggleOffMoveableFields();
        ToggleOffAttackableFields();
    }

    public void TurnOnChosenMark()
    {
        Field.TurnOnChosenMark();
    }

    public void TurnOffChosenMark()
    {
        Field.TurnOffChosenMark();
    }
}