using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGameObject;

    [SerializeField]
    private GridGenerator gridGenerator;

    [SerializeField]
    private Button spawnButton;

    private List<GameObject> possiblePlayerBlocks = new();
    public int playerCount = 0;

    private void Start()
    {
        // A button on UI has been set to run this function once clicked. This is attached at run time.
        spawnButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            InstantiatePlayer();
        });
    }

    public void InstantiatePlayer()
    {
        // Function to Find Possible Empty Block Types.
        PossiblePlayerBlocks();

        // Check if the Obstacle Grid Count is greater  than or equal to 5
        if ((gridGenerator.blocks.Count - possiblePlayerBlocks.Count) >= 5)
        {
            // Check if player is present of not. if player count is 0, then this loop
            if (playerCount == 0)
            {
                // Generate Random Block index.
                System.Random randomBlock = new();
                int index = randomBlock.Next(0, possiblePlayerBlocks.Count);

                // Instantiate Enemy Object and assign its position to selected game block above

                GameObject player = Instantiate(playerGameObject);

                GameObject parentBlock = gridGenerator.blocks.Find(gameObject => gameObject.name == possiblePlayerBlocks[index].name);

                player.transform.localPosition = new Vector3(parentBlock.GetComponent<BlockScript>().blockInfo.xCoord, 0.5f, parentBlock.GetComponent<BlockScript>().blockInfo.yCoord);
               
                player.transform.localScale = new Vector3(0.6818f, 0.6818f, 0.6818f);

                player.transform.GetComponent<PlayerScript>().SetCoordinates(parentBlock.GetComponent<BlockScript>().blockInfo.xCoord, parentBlock.GetComponent<BlockScript>().blockInfo.yCoord);

                parentBlock.GetComponent<BlockScript>().SetTypeOfBlock("player");

                // Increase player count to 1 and disable spawn button.
                playerCount++;
               
                spawnButton.enabled = false; 
            }
            else
            {
                Debug.Log("Player Already Present");
            }
        }
        else
        {
            Debug.Log("Enable 5 Enemies from Game Toolkit Window");
        }
    }


    // Find All blocks that are marked empty, and place it inside a list.
    private void PossiblePlayerBlocks()
    {
        if(possiblePlayerBlocks.Count > 0)
        {
            possiblePlayerBlocks.Clear();
        }


        foreach(GameObject block in gridGenerator.blocks)
        {
            if(block.GetComponent<BlockScript>().blockInfo.blockType == "empty")
            {
                possiblePlayerBlocks.Add(block);
            }
        }
    }

}
