using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameToolkit : EditorWindow
{
    private bool[,] toggleMatrix = new bool[10, 10];
    private bool isToggleActive = false;

    private ObstacleManager obstacleManager;
    private PlayerHandler playerHandler;

    /// <summary>
    /// Sets Game Window Summoning location on Toolbar under Game Title.
    /// </summary>
    [MenuItem("Game/Game Toolkit")]
    public static void ShowWindow()
    {
        GetWindow<GameToolkit>("Game Toolkit");
    }



    // GUI Render and updates
    private void OnGUI()
    {
        // Checks if the Editor in Playing Mode.if IN Playing mode, the rest of the UI is shown.
        if (!EditorApplication.isPlaying)
        {
            obstacleManager = null;
            playerHandler = null;
        } else
        {
            obstacleManager = FindAnyObjectByType<ObstacleManager>();
            playerHandler = FindAnyObjectByType<PlayerHandler>();
        }

        if (obstacleManager && playerHandler.playerCount == 0)
        {
            GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
            GUILayout.Label("This section is used to set the enemy tiles at runtime");


            // Begins UI Generation on Game Toolkit Window. It will be flexible Space so as to make the grid in center.
            GUILayout.Space(50);
            for (int i = 0; i < 10; ++i)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int j = 0; j < 10; ++j)
                {

                    // stores previous state of the toggle.
                    bool prevState = toggleMatrix[i, j];

                    // sets toggle
                    toggleMatrix[i, j] = EditorGUILayout.Toggle(toggleMatrix[i, j], GUILayout.Width(20));

                    // checks if the current toggle is equal to previous state -> Check if current toggle is true, and previous toggle is true or false.
                    if (prevState != toggleMatrix[i, j])
                    {
                        if (prevState == false)
                        {
                            isToggleActive = true;
                            HandleToggle(i, j, isToggleActive);
                        }
                        else
                        {
                            isToggleActive = false;
                            HandleToggle(i, j, isToggleActive);
                        }
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUI.enabled = true;
            }

            //GUILayout.Label("Instantiate Player", EditorStyles.boldLabel);
            //if(GUILayout.Button("Instantiate Player"))
            //{
            //    playerHandler.InstantiatePlayer();
            //}
        }
        else
        {
            GUILayout.Label("Enabled in PlayMode only", EditorStyles.boldLabel);
            GUI.enabled = false;
        }

    }

    /// <summary>
    /// Calls Function from Obstacle manager to Enable or Disable Tiles for Obstacle.
    /// </summary>
    /// <param name="i">X Coordinate</param>
    /// <param name="j">Y Coordinate</param>
    /// <param name="isActive">If the Obstacle is set to be active or not</param>
    private void HandleToggle(int i, int j, bool isActive)
    {
        if (obstacleManager != null)
        {
            
            obstacleManager.SetObstacle(isActive, i, j);
        }
        else
        {
            Debug.Log("No Obstacle Manager");
        }
    }
}
