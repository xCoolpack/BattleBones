using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            //Logger.Log($"{currentField.ThreeAxisCoordinates}");
            foreach (var field in currentField.GetNeighbors())
            {
                //Logger.Log($"{field.ThreeAxisCoordinates} - {canDoAction(field, startingField)}");
                if (canDoAction(field, startingField))
                {
                    sumCost = costOfFields[currentField] + cost(field);
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
    /// Methods calculating distance between Fields coordinates
    /// </summary>
    /// <param name="startingField"></param>
    /// <param name="targetField"></param>
    /// <returns></returns>
    public static int GetDistance(Field startingField, Field targetField)
    {
        return Math.Abs(startingField.ThreeAxisCoordinates.x - targetField.ThreeAxisCoordinates.x) +
               Math.Abs(startingField.ThreeAxisCoordinates.y - targetField.ThreeAxisCoordinates.y) +
               Math.Abs(startingField.ThreeAxisCoordinates.z - targetField.ThreeAxisCoordinates.z);
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
