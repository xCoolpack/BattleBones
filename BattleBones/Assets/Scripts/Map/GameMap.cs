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
            fieldGrid[field.GetCoordinates()] = field;
        }
    }

    public Field GetFieldAt(Vector2Int fieldCoordinates)
    {
        fieldGrid.TryGetValue(fieldCoordinates, out Field result);
        return result;
    }
}
