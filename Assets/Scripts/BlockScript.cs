using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public BlockInfo blockInfo;

    [SerializeField]
    private GameObject capsuleEnemy;


    // sets initial info about the current block
    public void SetInfo(string name,int xCoord, int yCoord)
    {
        blockInfo = Instantiate(blockInfo);
        blockInfo.blockType = "empty";
        blockInfo.blockName = name;
        blockInfo.xCoord = xCoord;
        blockInfo.yCoord = yCoord;
    }

    // Lets outside element access data of the block
    public BlockDataInfo GetInfo()
    {
        BlockDataInfo block = new()
        {
            blockName = blockInfo.blockName,
            blockType = blockInfo.blockType
        };
        return block;
    }


    // Set Status - Active or non Active for Obstacle block.
    public void SetActiveObstacle(bool activateState)
    {

        if (activateState == true)
        {
            blockInfo.blockType = "obstacle";
            capsuleEnemy.SetActive(true);
        }
        else if (activateState == false)
        {
            blockInfo.blockType = "empty";
            capsuleEnemy.SetActive(false);
        }
    }

    // Sets type of block 
    public void SetTypeOfBlock(string type)
    {
        blockInfo.blockType = type;
    }
}
