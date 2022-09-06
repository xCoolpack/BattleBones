using System.Collections.Generic;
using UnityEngine;

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
    public GameObject Player;
    public Field Field;
    public Movement Movement;
    public GameMap GameMap;

    public  List<Field> MoveableFields;

    // Heuristic for A*
    public delegate int Heuristic(Field startingField, Field targetField);

    private void Awake()
    {
        SetStats();

        // Temp
        Movement = GetComponent<Movement>();
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
        SetMoveableFields();
        Debug.Log("hey");
        GetVisibleFields().ForEach(Debug.Log);
        DisplayVisibleFields();
        //DisplayMoveableFields();
    }

    private void OnMouseUp()
    {
        HideVisibleFields();
        //HideMoveableFields();
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

    private List<Field> GetVisibleFields() 
    {
        return GraphSearch.BreadthFirstSearch(Field, SightRange, 
            (currentField, startingField) => currentField.IsVisibleFor(startingField), _ => 1);
    }
    private void DisplayVisibleFields()
    {
        foreach (var field in GetVisibleFields())
        {
            field.transform.Find("Mark").gameObject.SetActive(true);
        }
    }

    private void HideVisibleFields()
    {
        foreach (var field in GetVisibleFields())
        {
            field.transform.Find("Mark").gameObject.SetActive(false);
        }
    }

    private void SetMoveableFields()
    {
        MoveableFields = GetMoveableFields();
    }

    private List<Field> GetMoveableFields() 
    {
       return GraphSearch.BreadthFirstSearch(Field, CurrentMovementPoints,
            (currentField, startingField) => Movement.CanMove(currentField), Movement.GetMovementPointsCostForUnit);
    }

    private void DisplayMoveableFields() 
    {
        foreach (var field in MoveableFields)
        {
            field.transform.Find("Mark").gameObject.SetActive(true);
        }
    }

    private void HideMoveableFields() 
    {
        foreach (var field in MoveableFields)
        {
            field.transform.Find("Mark").gameObject.SetActive(false);
        }
    }

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
                if (Movement.CanMove(field))
                {
                    sumCost = costOfFields[currentField] + Movement.GetMovementPointsCostForUnit(field);

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
        Dictionary<Field, Field> graph = AStarSearch(Field, targetField, GetDistance);
        List<Field> movementPath = GeneratePathTo(graph, Field, targetField);
        List<Field> accessibleMovementPath = new List<Field>();
        int movementPointCost = 0;
        int nextMovementPointCost = 0;

        foreach (var field in movementPath)
        {
            nextMovementPointCost += Movement.GetMovementPointsCostForUnit(field);
            Debug.Log(nextMovementPointCost);
            if (CurrentMovementPoints < nextMovementPointCost)
                break;

            accessibleMovementPath.Add(field);
            Debug.Log(field);
            movementPointCost = nextMovementPointCost;
        }

        foreach (var field in accessibleMovementPath)
        {
            Debug.Log(field.Coordinates);
        }

        Field.Unit = null;
        Field = targetField;
        targetField.Unit = this;
        CurrentMovementPoints -= movementPointCost;
        MoveGraphicModel(accessibleMovementPath);

        return true;

        //if (MoveableFields.Contains(targetField))
        //{
        //    Debug.Log("Moving to" + targetField.Coordinates);
        //    (List<Field> movementPath, int movementPointsCost) = GeneratePathWithCost(this.Field, targetField);
        //    this.Field.Unit = null;
        //    this.Field = targetField;
        //    targetField.Unit = this;
        //    CurrentMovementPoints -= movementPointsCost;
        //    MoveGraphicModel(movementPath);

        //    if (CurrentMovementPoints > 0)
        //        SetMoveableFields();

        //    return true;
        //}
        //else 
        //    return false;
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

