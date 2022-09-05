using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.ParticleSystem;

public static class GraphSearch
{
    public delegate int Cost(Field field);
    public delegate bool CanDoAction(Field field);

    /// <summary>
    /// Breadth first search for field grid
    /// </summary>
    /// <param name="startingField"></param>
    /// <param name="range"></param>
    /// <param name="canDoAction"></param>
    /// <param name="cost"></param>
    /// <returns></returns>
    public static List<Field> BreadthFirstSearch(Field startingField, int range, CanDoAction canDoAction, Cost cost)
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
                if (canDoAction(field))
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

        return visitedFields.Keys.ToList();
    } 
}
