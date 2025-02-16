using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private PlayerScript player;

    [SerializeField]
    private EnemyScript enemy;

    /// <summary>
    /// A simple Turn Manager, which on Enable gets in Dedicated Scripts from Player and Enemy, and turns on Player Movement by Default.
    /// </summary>
    void OnEnable()
    {
        player = FindAnyObjectByType<PlayerScript>();
        enemy = FindAnyObjectByType<EnemyScript>();

        player.myTurn = true;
        enemy.enemyTurn = false;

    }

    public void TurnHandler(bool turnState, EventHandler type, Vector2Int playerLocation)
    {

       if(type == EventHandler.PLAYER && turnState == false)
        {
            player.myTurn = turnState;
            enemy.enemyTurn = !turnState;
            enemy.GetPlayerLocation(playerLocation.x, playerLocation.y);
        }
       else if(type == EventHandler.ENEMY && turnState == false)
        {
            player.myTurn = !turnState;
            enemy.enemyTurn = turnState;
        }
    }
}
