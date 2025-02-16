using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private GridGenerator gridGenerator;

    [SerializeField]
    private AI aiScript;

    public PlayerData playerData;

    public Vector2Int currentPlayerPosition;

    [SerializeField]
    private List<Vector2Int> path = new();

    [SerializeField]
    private TurnManager turnManager;

    private float speed = 5f;
    private float rotationSpeed = 5f;

    public bool enemyTurn = false;
    void Start()
    {
        playerData = Instantiate(playerData);
        gridGenerator = FindAnyObjectByType<GridGenerator>();
        aiScript = FindAnyObjectByType<AI>();
        turnManager = FindAnyObjectByType<TurnManager>();
    }


    /// <summary>
    /// The Update Cycle is running every frame to check if the player's turn is true or not. 
    /// If True, just after the player turn to move gets over, the Grid based Path Finding Algorithm returns the shortest path necessary. 
    /// After Path is full, then Coroutine is run to begin player movement
    /// </summary>
    private void Update()
    {
        if (enemyTurn == true)
        {
            Debug.Log(enemyTurn);


            // Check if the path is empty, only then call the path generation AI.
            if (path.Count == 0)
            {
                path = aiScript.FindPath(new Vector2Int(playerData.X, playerData.Y), currentPlayerPosition);
            }

            // Here to make sure the enemy doesnt land on player's position, but before that, the value at last index is removed. 
            if(path.Count > 1)
            {
                path.RemoveAt(path.Count - 1);
            }

            // Once path is calculated completely, Enemy Movement Begins. Turn is set to false to stop path updation on every frame.
            if (path.Count > 0)
            {
                StartCoroutine(EnemyMoveToLocation());
                enemyTurn = false;
            }
        }
    }

    /// <summary>
    /// Gets current player's coordinates
    /// </summary>
    /// <param name="x">X Coordinate</param>
    /// <param name="y">Y Coordinate</param>
    public void GetPlayerLocation(int x, int y)
    {
        currentPlayerPosition.x = x;
        currentPlayerPosition.y = y;

    }

    /// <summary>
    /// Sets Coordinates of the enemy scriptable object for future use.
    /// </summary>
    /// <param name="x">X Coordinate</param>
    /// <param name="y">Y Coordinate</param>
    public void SetCoordinates(int x, int y)
    {
        playerData.X = x;
        playerData.Y = y;

        // Once coordinates are fixed, the gridGenerator is called for changing the block type.
        if (gridGenerator != null)
        {
            string blockName = $"Block {x}:{y}";
            GameObject parentBlock = gridGenerator.blocks.Find(gameObject => gameObject.name == blockName);

            parentBlock.GetComponent<BlockScript>().SetTypeOfBlock("enemy");
            
        }
    }


    IEnumerator EnemyMoveToLocation()
    {

        foreach (var pos in path)
        {
            Vector3 targetPosition = new(pos.x, 0.5f, pos.y);
            Vector3 directionToFace = (targetPosition - gameObject.transform.position).normalized;


            // Rotating the player to the direction of the movement.
            if (directionToFace != Vector3.zero)
            {
                Quaternion rotatePlayer = Quaternion.LookRotation(directionToFace);
                while (Quaternion.Angle(gameObject.transform.rotation, rotatePlayer) > 1f)
                {
                    // some complex slerp definition, never understood, but works as finding the mid point of the two coordinates with given third as the speed of changing. something like that.
                    gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotatePlayer, rotationSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            // Actual Movement is done here
            while (Vector3.Distance(targetPosition, gameObject.transform.position) > 0.01f)
            {

                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);

                yield return null;
            }
            // Set Enemy's position after above loop, And then set its coordinates to current position
            gameObject.transform.position = targetPosition;
            SetCoordinates((int)targetPosition.x, (int)targetPosition.z);
        }

        // Clear path and reset current player's coordinates for future use. Call Turn Manager to handle the state.
        path.Clear();
        currentPlayerPosition = new Vector2Int(0, 0);
        turnManager.TurnHandler(false, EventHandler.ENEMY, new Vector2Int(0, 0));
    }
}
