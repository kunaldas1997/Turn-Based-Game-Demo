using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private GridGenerator gridGenerator;

    [SerializeField]
    private Button spawnButton;

    [SerializeField]
    private TurnManager turnManager;

    [SerializeField]
    private GameObject panelUI;

    public List<GameObject> possibleEmptyBlocks = new();

    void Start()
    {

        // A button on UI has been set to run this function once clicked. This is attached at run time.
        spawnButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            InstantiateEnemy();
        });

    }

    private void InstantiateEnemy()
    {
        // Function to Find Possible Empty Block Types.
        PossibleEmptyBlocks();

        // Generate Random Block index.
        System.Random randomBlock = new System.Random();

        int index = randomBlock.Next(0, possibleEmptyBlocks.Count);


        // Instantiate Enemy Object and assign its position to selected game block above
        GameObject enemyGO = Instantiate(enemy);

        GameObject parentBlock = gridGenerator.blocks.Find(gameObject => gameObject.name == possibleEmptyBlocks[index].name);

        enemyGO.transform.localPosition = new Vector3(parentBlock.GetComponent<BlockScript>().blockInfo.xCoord, 0.5f, parentBlock.GetComponent<BlockScript>().blockInfo.yCoord);

        enemyGO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        enemyGO.transform.GetComponent<EnemyScript>().SetCoordinates(parentBlock.GetComponent<BlockScript>().blockInfo.xCoord, parentBlock.GetComponent<BlockScript>().blockInfo.yCoord);

        parentBlock.GetComponent<BlockScript>().SetTypeOfBlock("enemy");

        // disable spawn button and then the entire spawning ui.
        spawnButton.enabled = false;
        panelUI.SetActive(false);

        // turn on Turn Manager Script once everything is done.
        turnManager.GetComponent<TurnManager>().enabled = true;
    }

    // Find All blocks that are marked empty, and place it inside a list.
    private void PossibleEmptyBlocks()
    {
        if (possibleEmptyBlocks.Count > 0)
        {
            possibleEmptyBlocks.Clear();
        }

        foreach (GameObject block in gridGenerator.blocks)
        {
            if (block.GetComponent<BlockScript>().blockInfo.blockType == "empty")
            {
                possibleEmptyBlocks.Add(block);
            }
        }
    }
}
