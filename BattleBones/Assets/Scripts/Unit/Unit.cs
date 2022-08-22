using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Max Stats
    public int maxHealth;
    public int maxDamage;
    public int maxDefense;
    public int maxMovementPoints;

    // Current Stats
    public int currentHealth;
    public int currentDamage;
    public int currentDefense;
    public int currentMovementPoints;
    public int attackRange;
    public int sightRange;

    // References
    public BaseUnitStats baseUnitStats;
    public GameObject player;
    public Field field;
    public Movement movement;

    public Dictionary<Field, Field> moveableFields;
    public Dictionary<Field, int> moveableFieldsCost;

    private void Awake()
    {
        SetStats();

        // Temp
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        // Test necessary
        if (Input.GetKeyDown("r")) 
        {
            Debug.Log("clicked r");
            currentMovementPoints = maxMovementPoints;
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
        maxHealth = baseUnitStats.baseHealth;
        currentHealth = baseUnitStats.baseHealth;
        maxDamage = baseUnitStats.baseDamage;
        currentDamage = baseUnitStats.baseDamage;
        maxDefense = baseUnitStats.baseDefense;
        currentDefense = baseUnitStats.baseDefense;
        maxMovementPoints = baseUnitStats.baseMovementPoints;
        currentMovementPoints = baseUnitStats.baseMovementPoints;
        attackRange = baseUnitStats.baseAttackRange;
        sightRange = baseUnitStats.baseSightRange;
    }

    private void SetMoveableFields()
    {
        (moveableFields, moveableFieldsCost) = GetMoveableFields(field, GameObject.Find("GameMap").GetComponent<GameMap>());
    }

    private (Dictionary<Field, Field>, Dictionary<Field, int>) GetMoveableFields(Field startingField, GameMap gameMap) 
    {
        Dictionary<Field, Field> visitedFields = new();
        Queue<Field> fieldsToVisit = new();
        Dictionary<Field, int> costOfFields = new();
        int sumCost = 0;

        fieldsToVisit.Enqueue(startingField);
        costOfFields.Add(startingField, sumCost);
        visitedFields.Add(startingField, null);

        while (fieldsToVisit.Count > 0) 
        {
            Field currentField = fieldsToVisit.Dequeue();

            foreach (Field field in gameMap.GetNeighboursOf(currentField))
            {
                if (movement.CanMove(field))
                {
                    sumCost = costOfFields[currentField] + movement.GetMovementPointsCostForUnit(field);

                    if (sumCost <= currentMovementPoints)
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
        costOfFields.Remove(startingField);

        return (visitedFields, costOfFields);
    }

    private void DisplayMoveableFields() 
    {
        foreach (Field field in moveableFields.Keys)
        {
            field.transform.Find("Mark").gameObject.SetActive(true);
        }
    }

    private void HideMoveableFields() 
    {
        foreach (Field field in moveableFields.Keys)
        {
            field.transform.Find("Mark").gameObject.SetActive(false);
        }
    }

    public bool Move(Field targetField) 
    {
        if (moveableFields.ContainsKey(targetField))
        {
            Debug.Log("Moving to" + targetField.coordinates);
            (List<Field> movementPath, int movementPointsCost) = GeneratePathWithCost(this.field, targetField);
            this.field.unit = null;
            this.field = targetField;
            targetField.unit = this;
            currentMovementPoints -= movementPointsCost;
            MoveGraphicModel(movementPath);

            if (currentMovementPoints > 0)
                SetMoveableFields();

            return true;
        }
        else 
            return false;
    }

    private (List<Field>, int) GeneratePathWithCost(Field startingfield, Field targetField)
    {
        List<Field> movementPath = new ();    
        Field currentField = targetField;
        movementPath.Add(currentField);
        while (moveableFields[currentField] != startingfield)
        {
            currentField = moveableFields[currentField];
            movementPath.Add(currentField);
        }

        //Debug.Log("after while loop");
        movementPath.Reverse();
        return (movementPath, moveableFieldsCost[targetField]);
    }

    // Not working as intended, only the last field is set as parent
    // comeback later
    private void MoveGraphicModel(List<Field> movementPath)
    {
        foreach (Field field in movementPath) 
        {
            Debug.Log(field.coordinates);
            //temporary solution, find better later (clipping colliders - selecting field instead of unit)
            this.transform.SetParent(field.transform, false); 
            //System.Threading.Thread.Sleep(1000);  
        }
    }

}

