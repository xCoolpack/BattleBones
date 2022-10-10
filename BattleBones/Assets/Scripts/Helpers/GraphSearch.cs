using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unit;

public static class GraphSearch
{
    public delegate int Cost(Field field);
    public delegate bool CanDoAction(Field currentField, Field startingField);
    public delegate int Heuristic(Field startingField, Field targetField);

    /// <summary>
    /// Breadth first search for field grid
    /// </summary>
    /// <param name="startingField"></param>
    /// <param name="range"></param>
    /// <param name="canDoAction"></param>
    /// <param name="cost"></param>
    /// <returns></returns>
    public static Dictionary<Field, int> BreadthFirstSearchDict(Field startingField, int range, CanDoAction canDoAction, Cost cost)
    {
        Dictionary<Field, Field> visitedFields = new();
        Queue<Field> fieldsToVisit = new();
        Dictionary<Field, int> costOfFields = new();
        int sumCost = 0;

        fieldsToVisit.Enqueue(startingField);
        costOfFields[startingField] = sumCost;
        visitedFields[startingField] = null;

        while (fieldsToVisit.Count > 0)
        {
            Field currentField = fieldsToVisit.Dequeue();

            foreach (var field in currentField.GetNeighbors())
            {
                
                if (canDoAction(field, startingField))
                {
                    
                    
                    sumCost = costOfFields[currentField] + cost(field);
                    if (field.Coordinates.x == 6 && field.Coordinates.y == -2)
                    {
                        Debug.Log($"Neighbor of {currentField}: {field}");
                        Debug.Log($"Cost: {sumCost} - {sumCost <= range}");
                    }
                    
                    if (sumCost <= range)
                    {
                        if (!visitedFields.ContainsKey(field) || costOfFields[field] > sumCost)
                        {
                            fieldsToVisit.Enqueue(field);
                            costOfFields[field] = sumCost;
                            visitedFields[field] = currentField;
                        }
                    }
                }
            }
        }

        visitedFields.Remove(startingField);
        costOfFields.Remove(startingField);

        return costOfFields;
    }

    public static List<Field> BreadthFirstSearchList(Field startingField, int range, CanDoAction canDoAction, Cost cost)
    {
        return BreadthFirstSearchDict(startingField, range, canDoAction, cost).Keys.ToList();
    }

    /// <summary>
    /// A* graph search for fields
    /// </summary>
    /// <param name="startingField"></param>
    /// <param name="targetField"></param>
    /// <param name="heuristic"></param>
    /// <returns></returns>
    public static (Dictionary<Field, Field>, Field) AStarSearch(Field startingField, Field targetField, CanDoAction canDoAction, Cost cost, 
        Heuristic heuristic, Func<Field, bool> IsFirstField)
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
                if (canDoAction(field, startingField))
                {
                    sumCost = costOfFields[currentField] + cost(field);

                    if (IsFirstField(currentField))
                        return (visitedFields, currentField);

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

        return (visitedFields, null);
    }
}
