using System.Collections;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField]
    private GridGenerator gridGenerator;
    public bool hasData;

    public void SetObstacle(bool isActive, int length, int width)
    {
        string boxName = $"Block {length}:{width}";

       
        GameObject searchedBlock = gridGenerator.blocks.Find(gameObject => gameObject.name == boxName);
        if (searchedBlock)
        {
            searchedBlock.GetComponent<BlockScript>().SetActiveObstacle(isActive);
        }
        else
        {
            Debug.Log("no block found");
        }

    }
}
