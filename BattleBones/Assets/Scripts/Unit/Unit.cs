using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        DisplayMoveableFields();
        // debug
        // int counter = 0;
        // foreach(var x in moveableFields.Keys) 
        // {
        //     Debug.Log(counter + " - " + x.coordinates);
        //     counter++;
        // }
        
    }

    private void OnMouseUp() 
    {
        HideMoveableFields();
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

    private void SetMoveableFields()
    {
        MoveableFields = GetMoveableFields(Field, GameMap);
    }

    private List<Field> GetMoveableFields(Field startingField, GameMap gameMap) 
    {
        Dictionary<Field, Field> visitedFields = new();
        Queue<Field> fieldsToVisit = new();
        Dictionary<Field, int> costOfFields = new();
        var sumCost = 0;

        fieldsToVisit.Enqueue(startingField);
        costOfFields.Add(startingField, sumCost);
        visitedFields.Add(startingField, null);

        while (fieldsToVisit.Count > 0) 
        {
            Field currentField = fieldsToVisit.Dequeue();

            foreach (var field in gameMap.GetNeighboursOf(currentField))
            {
                if (Movement.CanMove(field))
                {
                    sumCost = costOfFields[currentField] + Movement.GetMovementPointsCostForUnit(field);

                    if (sumCost <= CurrentMovementPoints)
                    {
                        if(!visitedFields.ContainsKey(field))
                        {
                            fieldsToVisit.Enqueue(field);
                            costOfFields.Add(field, sumCost);
                            visitedFields.Add(field, currentField);
                        }
                        else if(visitedFields.ContainsKey(field) && costOfFields[field] > sumCost)
                        {
                            costOfFields[field] = sumCost;
                            visitedFields[field] = currentField;
                        }                       
                    }
                }
                
            }
            
        }

        visitedFields.Remove(startingField);

        return visitedFields.Keys.ToList();
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

    public bool Move(Field targetField) 
    {
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

    //private (List<Field>, int) GeneratePathWithCost(Field startingField, Field targetField)
    //{
    //    List<Field> movementPath = new ();    
    //    Field currentField = targetField;
    //    movementPath.Add(currentField);
    //    while (MoveableFields[currentField] != startingField)
    //    {
    //        currentField = MoveableFields[currentField];
    //        movementPath.Add(currentField);
    //    }

    //    //Debug.Log("after while loop");
    //    movementPath.Reverse();
    //    return (movementPath, MoveableFieldsCost[targetField]);
    //}

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

