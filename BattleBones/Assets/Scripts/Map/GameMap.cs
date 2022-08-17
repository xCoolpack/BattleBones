using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    // Grid size
    public int width, height;
    // Grid
    public Dictionary<Vector2Int, Field> fieldGrid;

    private void Awake()
    {
        fieldGrid = new();
    }

    private void Start()
    {
        PopulateGrid();

        // tests
        //Debug.Log("Neighbours for 0, 0");

        //GetNeighboursOf(new Vector2Int(0, 0)).ForEach(x => Debug.Log(x.coordinates));

        //Debug.Log("Neighbours for 0, -1");

        //GetNeighboursOf(new Vector2Int(0, -1)).ForEach(x => Debug.Log(x.coordinates));
    }

    /// <summary>
    /// Methods adding fields to gird
    /// </summary>
    private void PopulateGrid()
    {
        foreach (Field field in FindObjectsOfType<Field>())
        {
            fieldGrid[field.coordinates] = field;
        }
    }

    /// <summary>
    /// Methods returning field at given coordinates
    /// </summary>
    /// <param name="fieldCoordinates"></param>
    /// <returns></returns>
    public Field GetFieldAt(Vector2Int fieldCoordinates)
    {
        fieldGrid.TryGetValue(fieldCoordinates, out Field result);
        return result;
    }

    /// <summary>
    /// Methods returning list of neighbours of field at given coordinates
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public List<Field> GetNeighboursOf(Vector2Int coordinates)
    {
        if (!fieldGrid.ContainsKey(coordinates))
            return new();
        else 
            return fieldGrid[coordinates].GetNeighbours();
    }

    /// <summary>
    /// Methods returning list of neighbours of given field
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public List<Field> GetNeighboursOf(Field field)
    {
        return GetNeighboursOf(field.coordinates);
    }
}
