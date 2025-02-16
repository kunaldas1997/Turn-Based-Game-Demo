using UnityEngine;

[CreateAssetMenu(fileName ="Block Info", menuName =  "Scriptable Objects/Block Object")]
public class BlockInfo : ScriptableObject
{
    public string blockName;
    public string blockType;
    public int xCoord;
    public int yCoord;
}
