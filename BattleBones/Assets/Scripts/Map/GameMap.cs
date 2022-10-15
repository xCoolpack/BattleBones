using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    // Grid size
    public int Width, Height;
    // Grid
    public Dictionary<Vector2Int, Field> FieldGrid;

    private void Awake()
    {
        FieldGrid = new Dictionary<Vector2Int, Field>();
    }

    private void OnEnable()
    {
        PopulateGrid();
    }

    private void Start()
    {
        //PopulateGrid();
    }

    /// <summary>
    /// Methods adding fields to gird
    /// </summary>
    private void PopulateGrid()
    {
        Debug.Log(FieldGrid.Count);
        FieldGrid.Clear();
        foreach (var field in FindObjectsOfType<Field>())
        {
            if (!FieldGrid.ContainsKey(field.Coordinates))
                FieldGrid.Add(field.Coordinates, field);
        }
        Debug.Log(FieldGrid.Count);
    }

    /// <summary>
    /// Methods returning field at given 2 axis coordinates
    /// </summary>
    /// <param name="fieldCoordinates"></param>
    /// <returns></returns>
    public Field GetFieldAt(Vector2Int fieldCoordinates)
    {
        FieldGrid.TryGetValue(fieldCoordinates, out Field result);
        return result;
    }

    /// <summary>
    /// Methods returning field at given 3 axis coordinates
    /// </summary>
    /// <param name="fieldCoordinates"></param>
    /// <returns></returns>
    public Field GetFieldAt(Vector3Int fieldCoordinates)
    {
        FieldGrid.TryGetValue(CoordinatesConverter.To2Axis(fieldCoordinates), out Field result);
        return result;
    }

    /// <summary>
    /// Methods returning list of neighbors of field at given coordinates
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public List<Field> GetNeighborsOf(Vector2Int coordinates)
    {
        if (!FieldGrid.ContainsKey(coordinates))
            return new List<Field>();

        return FieldGrid[coordinates].GetNeighbors();
    }

    /// <summary>
    /// Methods returning list of neighbors of given field
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public List<Field> GetNeighborsOf(Field field)
    {
        return GetNeighborsOf(field.Coordinates);
    }

    /// <summary>
    /// Methods returning list of neighbors' coordinates of field at given coordinates
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public List<Vector2Int> GetNeighborsCoordOf(Vector2Int coordinates)
    {
        return GetNeighborsOf(coordinates).Select(x => x.Coordinates).ToList();
    }

    /// <summary>
    /// Methods returning list of neighbors' coordinates of given field
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public List<Vector2Int> GetNeighborsCoordOf(Field field)
    {
        return GetNeighborsCoordOf(field.Coordinates);
    }
}
