using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordinatesConverter 
{
    /// <summary>
    /// Method converting 2 axis coordinates to 3 axis coordinates 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector3Int To3Axis(Vector2Int position)
    {
        var z = position.x - (position.y + (position.y % 2 + 2) % 2) / 2;
        var y = position.y;
        return new Vector3Int(-z - y, y, z);
    }

    /// <summary>
    /// Method converting 3 axis coordinates to 2 axis coordinates 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector2Int To2Axis(Vector3Int position)
    {
        var x = position.z + (position.y + (position.y % 2 + 2) % 2) / 2;
        var y = position.y;
        return new Vector2Int(x, y);
    }
}
