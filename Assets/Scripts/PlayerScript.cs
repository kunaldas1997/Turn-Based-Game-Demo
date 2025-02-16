using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private GridGenerator gridGenerator;

    [SerializeField]
    private AI aiScript;

    public PlayerData playerData;


    private GameObject player;
    private Vector2Int currentPlayerPosition;

    [SerializeField]
    private List<Vector2Int> path = new();

    private float speed = 5f;
    private float rotationSpeed = 5f;

    public bool myTurn;
    public TurnManager turnManager;
    void Start()
    {
        playerData = Instantiate(playerData);
        gridGenerator = FindAnyObjectByType<GridGenerator>();
        aiScript = FindAnyObjectByType<AI>();
        turnManager = FindAnyObjectByType<TurnManager>();
    }

 
    /// <summary>
    /// The Update Cycle is running every frame to check if the player's turn is true or not. 
    /// If True, then the Raycast begins, therefore letting the player select two points - the Player, and the destination path
    /// On Selecting the player, its current value is taken from the ScriptableObject used to hold its coordinates. 
    /// On Selecting the Block, just after the player to move, the Grid based Path Finding Algorithm returns the shortest path necessary. 
    /// After Path is full, then Coroutine is run to begin player movement
    /// </summary>
    void Update()
    {
        if (myTurn == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Checking for Raycast from Main camera. ScreenPointToRay converts current mouse's point on screen
                // and use it as a ray.
                Ray playerSelectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(playerSelectionRay, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        SelectPlayer(hit.collider.gameObject);
                    }
                    else if (player != null && hit.collider.CompareTag("Block"))
                    {
                        // When both player and destination block is selected, the path is calculated.
                        path = aiScript.FindPath(currentPlayerPosition, new Vector2Int(hit.collider.gameObject.GetComponent<BlockScript>().blockInfo.xCoord, hit.collider.gameObject.GetComponent<BlockScript>().blockInfo.yCoord));

                        // Once path is calculated completely, Player Movement Begins.
                        if (path.Count > 0)
                        {
                            StartCoroutine(PlayerMoveToLocation());

                        }
                    }
                }

            }
            
        }
    }


    /// <summary>
    /// Selects the Player Object and set corrent Position with its coordinates values
    /// </summary>
    /// <param name="playerOB">Gets the Player Object when selected</param>
    private void SelectPlayer(GameObject playerOB)
    {
        player = playerOB;
        currentPlayerPosition = new Vector2Int(playerData.X, playerData.Y);
    }


    /// <summary>
    /// Sets Coordinates of the player scriptable object for future use.
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

            parentBlock.GetComponent<BlockScript>().SetTypeOfBlock("Player");

        }
    }

    // Coroutine for player movement
    IEnumerator PlayerMoveToLocation()
    {

        foreach (var pos in path)
        {

            Vector3 targetPosition = new(pos.x, 0.5f, pos.y);
            Vector3 directionToFace = (targetPosition - player.transform.position).normalized;

            // Rotating the player to the direction of the movement.
            if (directionToFace != Vector3.zero)
            {
                Quaternion rotatePlayer = Quaternion.LookRotation(directionToFace);
                while (Quaternion.Angle(player.transform.rotation, rotatePlayer) > 1f)
                {
                    // some complex slerp definition, never understood, but works as finding the mid point of the two coordinates with given third as the speed of changing. something like that.
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotatePlayer, rotationSpeed * Time.deltaTime);
                    yield return null;
                }
            }

            // Actual Movement is done here
            while (Vector3.Distance(targetPosition, player.transform.position) > 0.01f)
            {

                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, speed * Time.deltaTime);

                yield return null;
            }

            // Set Player's position after above loop, And then set its coordinates to current position
            player.transform.position = targetPosition;
            SetCoordinates((int)targetPosition.x, (int)targetPosition.z);
        }

        // Deselect player once turn is over, and then call Turn manager to activate enemy movement.
        player = null;
        turnManager.TurnHandler(false, EventHandler.PLAYER, new Vector2Int(playerData.X, playerData.Y));
    }
}
