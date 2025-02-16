using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Interface for the Grid Path finding algorithm
/// </summary>
public interface IAIinterface
{
    List<Vector2Int> FindPath(Vector2Int source, Vector2Int target);
    bool CheckForValidTile(Vector2Int tilePosition);
    public List<Vector2Int> ConstructPath(Dictionary<Vector2Int, Vector2Int> sourcePoint, Vector2Int source, Vector2Int target);

}
