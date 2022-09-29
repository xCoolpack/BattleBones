using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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

    // Heuristic for A*
    public delegate int Heuristic(Field startingField, Field targetField);

    private void Awake()
    {
        SetStartingStats();

        // Temp
        MovementScript = GetComponent<Movement>(); // if null then it's hero
        AttackScript = GetComponent<Attack>(); // if null then it's hero
        GameMap = GameObject.Find("GameMap").GetComponent<GameMap>();
    } 

    private void Update()
    {
        // Test necessary
        if (Input.GetKeyDown("r")) 
        {
            Debug.Log("clicked r");
            CurrentMovementPoints = MaxMovementPoints;
        }
    }

    private void Start()
    {
        _overlay = GameObject.Find("Overlay").GetComponent<Overlay>();
    }

    /// <summary>
    /// Methods setting unit stats from BaseUnitStats object
    /// </summary>
    private void SetStartingStats()
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

    private void SetVisibleFields()
    {
        VisibleFields = GetVisibleFields();
    }

    private List<Field> GetVisibleFields() 
    {
        return GraphSearch.BreadthFirstSearchList(Field, SightRange, 
            (currentField, startingField) => currentField.IsVisibleFor(this, startingField), _ => 1);
    }
    private void ToggleVisibleFields()
    {
        foreach (var field in GetVisibleFields())
        {
            var mark = field.transform.Find("Mark").gameObject;
            mark.SetActive(!mark.activeSelf);
        }
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
            (currentField, startingField) => MovementScript.CanMove(this, currentField), (field) => MovementScript.GetMovementPointsCostForUnit(this, field));
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
        //Attack range is always smaller or equal to sight range
        set.UnionWith(GraphSearch.BreadthFirstSearchList(Field, AttackRange,
                (currentField, startingField) => AttackScript.CanAttack(this, Field, currentField), _ => 1));
        foreach (var keyPair in _moveableFieldWithCost)
        {
            if (AttackScript.HaveEnoughMovementPoints(CurrentMovementPoints - keyPair.Value))
                set.UnionWith(GraphSearch.BreadthFirstSearchList(keyPair.Key, AttackRange, 
                    (currentField, startingField) => AttackScript.CanAttack(this, keyPair.Key, currentField), _ => 1));
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
            var mark = field.transform.Find("Mark").gameObject;
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

        public void MoveOrAttack(Field field)
    {
        // if unit can move to field
            Move(field);
        // if unit can attack building at field
            DealDamage(field.Building);
        // if unit can attack unit at field
            DealDamage(field.Unit);
    }

    #region Move
    /// <summary>
    /// Methods calculating distance beetwen Fields coordinates
    /// </summary>
    /// <param name="startingField"></param>
    /// <param name="targetField"></param>
    /// <returns></returns>
    public int GetDistance(Field startingField, Field targetField)
    {
        // TO DO
        return 0;
    } 

    /// <summary>
    /// A* graph search for fields
    /// </summary>
    /// <param name="startingField"></param>
    /// <param name="targetField"></param>
    /// <param name="heuristic"></param>
    /// <returns></returns>
    public Dictionary<Field, Field> AStarSearch(Field startingField, Field targetField, Heuristic heuristic)
    {
        Dictionary<Field, Field> visitedFields = new();
        IntPriorityQueue<Field> fieldsToVisit = new();
        Dictionary<Field, int> costOfFields = new();
        int sumCost = 0;

        fieldsToVisit.Enqueue(startingField, sumCost);
        costOfFields[startingField] = sumCost;
        visitedFields[startingField] = null;

        while (fieldsToVisit.Count > 0)
        {
            (Field currentField, _) = fieldsToVisit.Dequeue();

            if (targetField == currentField)
                break;

            foreach (var field in currentField.GetNeighbors())
            {
                if (MovementScript.CanMove(this, field))
                {
                    sumCost = costOfFields[currentField] + MovementScript.GetMovementPointsCostForUnit(this, field);

                    if (!visitedFields.ContainsKey(field) || costOfFields[field] > sumCost)
                    {
                        costOfFields[field] = sumCost;
                        int priority = sumCost + heuristic(field, targetField);
                        fieldsToVisit.Enqueue(field, priority);
                        visitedFields[field] = currentField;
                    }
                }
            }
        }

        return visitedFields;
    }

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
    /// Move method for unit 
    /// </summary>
    /// <param name="targetField"></param>
    /// <returns></returns>
    public bool Move(Field targetField)
    {
        if (targetField == Field)
            return false;
        Dictionary<Field, Field> graph = AStarSearch(Field, targetField, GetDistance);
        List<Field> movementPath = GeneratePathTo(graph, Field, targetField);
        var accessibleMovementPath = new List<Field>();
        int movementPointCost = 0;
        int nextMovementPointCost = 0;

        foreach (var field in movementPath)
        {
            nextMovementPointCost += MovementScript.GetMovementPointsCostForUnit(this, field);
            Debug.Log(nextMovementPointCost);
            if (CurrentMovementPoints < nextMovementPointCost)
                break;

            accessibleMovementPath.Add(field);
            Debug.Log(field);
            movementPointCost = nextMovementPointCost;
        }

        //foreach (var field in accessibleMovementPath)
        //{
        //    Debug.Log(field.Coordinates);
        //}

        // Remove modifiers from starting field
        RemoveUnitModifiers(Field.Type.FieldUnitModifiers);
        RemoveUnitModifiers(Field.Building.GetUnitModifiers());

        // Add modifiers from target field
        AddUnitModifiers(targetField.Type.FieldUnitModifiers);
        AddUnitModifiers(targetField.Building.GetUnitModifiers());


        // Move references between fields
        Field.Unit = null;
        Field = targetField;
        targetField.Unit = this;
        CurrentMovementPoints -= movementPointCost;
        MoveGraphicModel(accessibleMovementPath);

        return true;
    }

    // Not working as intended, only the last field is set as parent
    // comeback later
    private void MoveGraphicModel(List<Field> movementPath)
    {
        foreach (Field field in movementPath)
        {
            Debug.Log(field.Coordinates);
            //temporary solution, find better later (clipping colliders - selecting field instead of unit)
            this.transform.SetParent(field.transform, false);
            //System.Threading.Thread.Sleep(1000);  
        }
    }
    #endregion

    public void DealDamage(Building building)
    {
        UnitModifiers unitModifiers = building.Field.Unit.CurrentModifiers +
                                      new UnitModifiers(damage: building.Field.Unit.AttackScript.GetCounterAttackModifier());
        var (_, damage, _) = unitModifiers.CalculateModifiers(0, building.Field.Unit.CurrentDamage, 0);

        building.TakeDamage(CurrentDamage);
        if (AttackScript.IsProvokingCounterAttack())
            TakeDamage(damage);
    }

    public void DealDamage(Unit unit)
    {
            UnitModifiers unitModifiers = unit.CurrentModifiers +
                                          new UnitModifiers(damage: unit.AttackScript.GetCounterAttackModifier());
            var (_, damage, _) = unitModifiers.CalculateModifiers(0, unit.CurrentDamage, 0);
        
        unit.TakeDamage(CurrentDamage);
        if (AttackScript.IsProvokingCounterAttack())
            TakeDamage(damage);
    }

    /// <summary>
    /// Method applying dealt damage 
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public void TakeDamage(int damage)
    {
        CurrentHealth -= UnitCalculation.CalculateDealtDamage(damage, CurrentDefense);

        if (CurrentHealth <= 0)
            Delete();

        // Recalculate DamageModifiers from missing health
        ChangeModifiersFromHealth();
    }

    public void BeginHealing()
    {
        CurrentMovementPoints = 0;
        Player.PlayerEventHandler.AddStartTurnEvent(new GameEvent(1, Heal));
    }

    public void Heal()
    {
        CurrentHealth = Math.Min(CurrentHealth + (int)Math.Ceiling(HealRatio*MaxHealth), MaxHealth);
        ChangeModifiersFromHealth();
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

            SetMoveableFields();
            SetAttackableFields();
            SetVisibleFields();
            ToggleOnMoveableFields();
            ToggleOnAttackableFields();
        }
        else
        {
            _overlay.UnitInfoBox(false);
        }


    }
}

