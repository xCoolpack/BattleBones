using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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
        
    }

    private void Start()
    {
        PopulateGrid();
        foreach (var player in FindObjectsOfType<Player>())
        {
            player.Units.ForEach(u => u.AtStart());
            player.Buildings.ForEach(b => b.AtStart());
        }
    }

    /// <summary>
    /// Methods adding fields to gird
    /// </summary>
    private void PopulateGrid()
    {
        FieldGrid.Clear();
        foreach (var field in FindObjectsOfType<Field>())
        {
            Logger.Log($"{field.Type.FieldName} {field.ThreeAxisCoordinates}");
            if (!FieldGrid.ContainsKey(field.Coordinates))
                FieldGrid.Add(field.Coordinates, field);
        }
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
