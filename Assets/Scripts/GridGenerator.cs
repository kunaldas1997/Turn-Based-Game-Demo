using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject gameBlock;

    [SerializeField]
    private float spacing = 1f;
    public int gridSize = 10;

    [SerializeField]
    private GameObject envBlock;

    [SerializeField]
    List<Material> materials = new();

    [SerializeField]
    private int[] randomNumberArray;

    public List<GameObject> blocks = new();
    private GameObject block;

    [SerializeField]
    private ObstacleManager obstacleManager;


    /// <summary>
    /// Start Function assigns the grid size, and generate a random number to select the material from the material list. 
    /// </summary>
    void Awake()
    {
        randomNumberArray = new int[gridSize * gridSize];
        System.Random randomNumber = new();

        for (int i = 0; i < randomNumberArray.Length; ++i)
        {
            int randomVal = randomNumber.Next(0, materials.Capacity);
            randomNumberArray[i] = randomVal;
        }
    }


    // enable Obstacle Manager for detection.
    private void Start()
    {
        GridMaker();

        obstacleManager.enabled = true;
        obstacleManager.hasData = true;

     
    }

    /// <summary>
    /// Function to generate Grid. 
    /// A 2D grid, which needs spacing for placement of the block, name, material, block initial info, and then child it to Environment Block.
    /// Later add it to universal block list.
    /// </summary>
    public void GridMaker()
    {
        System.Random randomIndexer = new();
        for (int length = 0; length < gridSize; ++length)
        {
            for (int width = 0; width < gridSize; ++width)
            {
                Vector3 blockPosition = new(length * spacing, 0, width * spacing);
                block = Instantiate(gameBlock, blockPosition, Quaternion.identity);
                block.name = $"Block {length}:{width}";
                block.transform.GetChild(0).GetComponent<MeshRenderer>().material = materials[randomNumberArray[randomIndexer.Next(0, gridSize * gridSize)]];
                block.GetComponent<BlockScript>().SetInfo(block.name, length, width);
                block.transform.parent = envBlock.transform;
                blocks.Add(block);
            }
        }
    }
}
