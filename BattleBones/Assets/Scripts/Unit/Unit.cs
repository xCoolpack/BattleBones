using System.Collections;
using System.Collections.Generic;
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
    public GameObject Player;
    public Field field;
    public Movement movement;

    public Dictionary<Field, Field> moveableFields;

    private void Awake()
    {
        SetStats();

        // Temp
        movement = GetComponent<Movement>();
    }

    private void Update()
    {


    }

    private void OnMouseDown()
    {
        moveableFields = GetMoveableFields(field, GameObject.Find("GameMap").GetComponent<GameMap>());
        DisplayMoveableFields();
        // debug
        int counter = 0;
        foreach(var x in moveableFields.Keys) 
        {
            Debug.Log(counter + " - " + x.coordinates);
            counter++;
        }
        
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

    public bool CanMove(Field field) 
    {
        return !field.IsObstacle() && !field.HasUnit();
    }

    private Dictionary<Field, Field> GetMoveableFields(Field startingField, GameMap gameMap) 
    {
        Dictionary<Field, Field> visitedFields = new();
        Queue<Field> fieldsToVisit = new();
        Dictionary<Field, int> costOfFields = new();
        int sumCost = 0;

        fieldsToVisit.Enqueue(startingField);
        costOfFields.Add(startingField, sumCost);
        visitedFields.Add(startingField, null);

        while(fieldsToVisit.Count > 0) 
        {
            Field currentField = fieldsToVisit.Dequeue();

            foreach(Field field in gameMap.GetNeighboursOf(currentField))
            {
                if(CanMove(field))
                {
                    sumCost = costOfFields[currentField] + movement.GetMovementPointsCostForUnit(field);;

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

        return visitedFields;

    }

    private void DisplayMoveableFields() 
    {
        foreach(Field field in moveableFields.Keys)
        {
            field.transform.Find("Mark").gameObject.SetActive(true);
        }
    }

    private void HideMoveableFields() 
    {
        foreach(Field field in moveableFields.Keys)
        {
            field.transform.Find("Mark").gameObject.SetActive(false);
        }
    }

}

