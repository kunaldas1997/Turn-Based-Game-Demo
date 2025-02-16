using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// A Simple implementation of Breadth First Search to scan all blocks and detect shortest path.
/// </summary>
public class AI : MonoBehaviour, IAIinterface
{
    [SerializeField]
    private GridGenerator gridGenerator;
    private GameObject[,] grid;
    private Vector2Int gridSize;

    
    // Get th gridSize.
  
    private void Start()
    {
        gridSize = new(gridGenerator.gridSize, gridGenerator.gridSize);

    }

    // Finding the Shortest Path.
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {

        // Queue to handle all paths
        Queue<Vector2Int> queue = new();

        //Add the first block
        queue.Enqueue(start);


        // Dictionary to hold the Starting Block.
        Dictionary<Vector2Int, Vector2Int> sourcePoint = new()
        {
            [start] = start
        };

        // Search Directions - up, down, bottom, left (not in order, to prevent diagnol movement).
        Vector2Int[] directions =
        {
            new (0, 1),
            new (1, 0),
            new (-1, 0),
            new (0, -1)
        };

        while(queue.Count > 0)
        {
            // Get Current value from the queue
            Vector2Int current = queue.Dequeue();
            
            // check if the current value is the target then break out.
            if(current == target)
            {
                break;
            }

            // If Not, then traverse for every direction in the directions
            foreach(var direction in directions)
            {

                // For next block from current, add the direction value to increment.
                Vector2Int next = current + direction;

                // if its a valid tile and doesn't exist in the dictionary add it to the queue and dictionary.
                if(CheckForValidTile(next) && !sourcePoint.ContainsKey(next))
                {
                    queue.Enqueue(next);
                    sourcePoint[next] = current;
                    
                }
            }
        }

        // Construct the path.
        return ConstructPath(sourcePoint, start, target);
    }

    /// <summary>
    /// Pass the complete dictionary, the source coords and the target coords.
    /// </summary>
    /// <param name="sourcePoint">Complete Source Dictionary</param>
    /// <param name="source">Source Coordinates</param>
    /// <param name="target">Target Coordinates</param>
    /// <returns></returns>
    public List<Vector2Int> ConstructPath(Dictionary<Vector2Int, Vector2Int> sourcePoint, Vector2Int source, Vector2Int target)
    {
        List<Vector2Int> path = new();
        if(!sourcePoint.ContainsKey(target))
        {
           return path;
        }

        // Traverse from Destination from Source. Done so as the path needs to backtracked to find how we reach this tile. 
        for(Vector2Int current = target; current != source; current = sourcePoint[current])
        {
            // if certain condition is met, move ahead, to add the vector2int to path.
            if(Mathf.Abs(current.x - sourcePoint[current].x )>1  || Mathf.Abs(current.y - sourcePoint[current].y) > 1)
            {
                continue;
            }
            path.Add(current);
        }

        // reverse the path.
        path.Reverse();
        return path;
    }


    /// <summary>
    /// Check for Valid Tile
    /// </summary>
    /// <param name="tilePosition">Takes in Current tile Position</param>
    /// <returns></returns>
    public bool CheckForValidTile(Vector2Int tilePosition)
    {
        if(tilePosition.x < 0 || tilePosition.x >= gridSize.x || tilePosition.y < 0 || tilePosition.y >= gridSize.y)
        {
            return false;
        }

        string blockName = $"Block {tilePosition.x}:{tilePosition.y}";
        GameObject gameTile = gridGenerator.blocks.Find(gameObject => gameObject.name == blockName);

        if(gameTile == null)
        {
            Debug.Log("no Tile Found");
        }

        // return the gametile if is not equal to obstacle.
        return gameTile.GetComponent<BlockScript>().blockInfo.blockType != "obstacle";
    }
}
