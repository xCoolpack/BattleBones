using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static float xOffset = 2.56f, yOffset = 1.93f;

    // Coordinates
    [SerializeField] private Vector2Int coordinates;

    // References
    public FieldType type { get; set; }
    public List<Object> seeenBy; //temp - replace Object with Player
    public Object building; //temp - replace Object with Building
    public Object unit; //temp - replace Object with Unit

    private void Awake()
    {
        coordinates = ConvertPositionToCoordinates(transform.position);
    }

    private Vector2Int ConvertPositionToCoordinates(Vector2 position)
    {
        int x = Mathf.CeilToInt((float)System.Math.Round(position.x, 2) / xOffset);
        int y = Mathf.CeilToInt((float)System.Math.Round(position.y, 2) / yOffset);
        return new Vector2Int(x, y);
    }

    public Vector2Int GetCoordinates()
    {
        return coordinates;
    } 

    public List<Field> GetNeighbours()
    {
        return null;
    } 
}

