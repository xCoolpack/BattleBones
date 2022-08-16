using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public int width, height;
    public Dictionary<Vector2Int, Field> fieldGrid;

    private void Awake()
    {
        fieldGrid = new();
    }

    private void Start()
    {
        foreach (Field field in FindObjectsOfType<Field>())
        {
            fieldGrid[field.coordinates] = field;
        }

        // tests
        Debug.Log("Neighbours for 0, 0");

        GetNeighboursOf(new Vector2Int(0, 0)).ForEach(x => Debug.Log(x.coordinates));

        Debug.Log("Neighbours for 0, -1");

        GetNeighboursOf(new Vector2Int(0, -1)).ForEach(x => Debug.Log(x.coordinates));
    }

    public Field GetFieldAt(Vector2Int fieldCoordinates)
    {
        fieldGrid.TryGetValue(fieldCoordinates, out Field result);
        return result;
    }

    public List<Field> GetNeighboursOf(Vector2Int coordinates)
    {
        if (!fieldGrid.ContainsKey(coordinates))
            return new();
        else 
            return fieldGrid[coordinates].GetNeighbours();
    }

    public List<Field> GetNeighboursOf(Field field)
    {
        return GetNeighboursOf(field.coordinates);
    }
}
