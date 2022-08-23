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
        foreach (var field in FindObjectsOfType<Field>())
        {
            FieldGrid.Add(field.Coordinates, field);
        }
    }

    /// <summary>
    /// Methods returning field at given coordinates
    /// </summary>
    /// <param name="fieldCoordinates"></param>
    /// <returns></returns>
    public Field GetFieldAt(Vector2Int fieldCoordinates)
    {
        FieldGrid.TryGetValue(fieldCoordinates, out Field result);
        return result;
    }

    /// <summary>
    /// Methods returning list of neighbours of field at given coordinates
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public List<Field> GetNeighboursOf(Vector2Int coordinates)
    {
        if (!FieldGrid.ContainsKey(coordinates))
            return new List<Field>();

        return FieldGrid[coordinates].GetNeighbours();
    }

    /// <summary>
    /// Methods returning list of neighbours of given field
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public List<Field> GetNeighboursOf(Field field)
    {
        return GetNeighboursOf(field.Coordinates);
    }

    /// <summary>
    /// Methods returning list of neighbours' coordinates of field at given coordinates
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public List<Vector2Int> GetNeighboursCoordOf(Vector2Int coordinates)
    {
        return GetNeighboursOf(coordinates).Select(x => x.Coordinates).ToList();
    }

    /// <summary>
    /// Methods returning list of neighbours' coordinates of given field
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public List<Vector2Int> GetNeighboursCoordOf(Field field)
    {
        return GetNeighboursCoordOf(field.Coordinates);
    }
}
