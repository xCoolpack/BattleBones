using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
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
    public Movement Movement;
    public Attack Attack;
    public GameMap GameMap;

    public List<Field> MoveableFields;
    public List<Field> VisibleFields;
    public List<Field> FieldsWithinAttackRange;
    public List<Field> AttackableFields;

    // Heuristic for A*
    public delegate int Heuristic(Field startingField, Field targetField);

    private void Awake()
    {
        SetStats();

        // Temp
        Movement = GetComponent<Movement>(); // if null then it's hero
        Attack = GetComponent<Attack>(); // if null then it's hero
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

    private void OnMouseDown()
    {
        if (!IsPlayersTurn()) return;
        SetVisibleFields();
        SetMoveableFields();
        SetAttackableFields(); 
        Debug.Log("hey");
        //GetVisibleFields().ForEach(Debug.Log);
        //ToggleVisibleFields();
        //ToggleMoveableFields();
        //ToggleAttackableFields();
        ToggleFieldsWithinAttackRange();
    }

    private void OnMouseUp()
    {
        if (!IsPlayersTurn()) return;
        //ToggleVisibleFields();
        //ToggleMoveableFields();
        //ToggleAttackableFields();
        ToggleFieldsWithinAttackRange();
    }

    /// <summary>
    /// Checks if it's units owners turn 
    /// </summary>
    /// <returns>True if it is</returns>
    private bool IsPlayersTurn()
    {
        var turnHandler = GameObject.Find("TurnHandler").GetComponent<TurnHandler>();
        return turnHandler.CurrentPlayer == Player;
    }

    /// <summary>
    /// Methods setting unit stats from BaseUnitStats object
    /// </summary>
    private void SetStats()
    {
        MaxHealth = BaseUnitStats.BaseHealth;
        CurrentHealth = BaseUnitStats.BaseHealth;
        MaxDamage = BaseUnitStats.BaseDamage;
        CurrentDamage = BaseUnitStats.BaseDamage;
        MaxDefense = BaseUnitStats.BaseDefense;
        CurrentDefense = BaseUnitStats.BaseDefense;
        MaxMovementPoints = BaseUnitStats.BaseMovementPoints;
        CurrentMovementPoints = BaseUnitStats.BaseMovementPoints;
        AttackRange = BaseUnitStats.BaseAttackRange;
        SightRange = BaseUnitStats.BaseSightRange;
    }

    public bool IsEnemy(Player player)
    {
        return Player != player;
    }
    #region VisibleFields

    private void SetVisibleFields()
    {
        VisibleFields = GetVisibleFields();
    }

    private List<Field> GetVisibleFields() 
    {
        return GraphSearch.BreadthFirstSearch(Field, SightRange, 
            (currentField, startingField) => currentField.IsVisibleFor(startingField), _ => 1);
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
        MoveableFields = GetMoveableFields();
    }

    private List<Field> GetMoveableFields() 
    {
       return GraphSearch.BreadthFirstSearch(Field, CurrentMovementPoints,
            (currentField, startingField) => Movement.CanMove(this, currentField), (field) => Movement.GetMovementPointsCostForUnit(this, field));
    }

    private void ToggleMoveableFields() 
    {
        foreach (var field in MoveableFields)
        {
            var mark = field.transform.Find("Mark").gameObject;
            mark.SetActive(!mark.activeSelf);
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
        set.UnionWith(GraphSearch.BreadthFirstSearch(Field, AttackRange,
                (currentField, startingField) => Attack.CanAttack(this, Field, currentField), _ => 1));
        foreach (var field in MoveableFields)
        {
            set.UnionWith(GraphSearch.BreadthFirstSearch(field, AttackRange,
                (currentField, startingField) => Attack.CanAttack(this, field, currentField), _ => 1));
        }
        foreach (Field field in set)
        {
            if (Attack.CanTarget(this, field)) 
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

    private void ToggleAttackableFields()
    {
        foreach (var field in AttackableFields)
        {
            var mark = field.transform.Find("Mark").gameObject;
            mark.SetActive(!mark.activeSelf);
        }
    }
    #endregion

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
                if (Movement.CanMove(this, field))
                {
                    sumCost = costOfFields[currentField] + Movement.GetMovementPointsCostForUnit(this, field);

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
            nextMovementPointCost += Movement.GetMovementPointsCostForUnit(this, field);
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

}

