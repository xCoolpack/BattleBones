using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// Methods returning list of neighbours' coordinates of field at given coordinates
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public List<Vector2Int> GetNeighboursCoordOf(Vector2Int coordinates)
    {
        return GetNeighboursOf(coordinates).Select(x => x.coordinates).ToList();
    }

    /// <summary>
    /// Methods returning list of neighbours' coordinates of given field
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public List<Vector2Int> GetNeighboursCoordOf(Field field)
    {
        return GetNeighboursCoordOf(field.coordinates);
    }
}
