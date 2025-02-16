using System;


[Serializable]
public class BlockDataInfo
{
    public string blockName { get; set; }
    public string blockType { get; set; }

}

public enum EventHandler
{
    PLAYER,
    ENEMY
}