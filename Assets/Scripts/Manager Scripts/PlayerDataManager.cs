using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public PieceManager pieceManager4PlayerData;


    public List<PlayerData> allPlayers;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(
                gameObject); // Optional: Prevents the object from being destroyed when loading a new scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializePlayers()
    {
        // Initialize players with names, initial positions, and assigned gamepads
        allPlayers = new List<PlayerData>();

        // Desired names for players
        string[] playerNames = { "Manny", "Moe", "Maroon", "Mei" };

        for (int i = 0; i < Gamepad.all.Count && i < playerNames.Length; i++)
        {
            // Create a new player with the desired name, initial position, and assigned gamepad
            PlayerData player = new PlayerData(playerNames[i], 1, Gamepad.all[i], i+1);

            // Add the player to the list of all players
            allPlayers.Add(player);

            // Find the cubes in the scene by name and group them in an array
            Transform[] targetCubes = pieceManager4PlayerData.GetTargetCubes(12, i);

            // Set the target cubes for the player
            player.SetTargetCubes(targetCubes);
        }
    }

    public void AddCubes()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            
            Transform[] targetCubes = pieceManager4PlayerData.GetTargetCubes(12, i);
            allPlayers[i].SetTargetCubes(targetCubes);
        }
    }


    void AdjustPositionsBasedOnOrder(string[] playerNames)
    {
        foreach (string playerName in playerNames)
        {
            PlayerData player = allPlayers.Find(p => p.playerName == playerName);

            if (player != null)
            {
                int playerPosition = allPlayers.IndexOf(player) + 1;
                Debug.Log($"{player.playerName} is in position {playerPosition}");
            }
            else
            {
                Debug.LogWarning($"Player with name {playerName} not found in allPlayers list.");
            }
        }
    }
    
    public bool IsButtonPressed(PlayerData.Button button)
    {
        foreach (var gamepad in Gamepad.all)
        {
            switch (button)
            {
                case PlayerData.Button.A:
                    if (gamepad.aButton.wasPressedThisFrame)
                        return true;
                    break;
                case PlayerData.Button.X:
                    if (gamepad.xButton.wasPressedThisFrame)
                        return true;
                    break;
                case PlayerData.Button.Y:
                    if (gamepad.yButton.wasPressedThisFrame)
                        return true;
                    break;
                case PlayerData.Button.B:
                    if (gamepad.bButton.wasPressedThisFrame)
                        return true;
                    break;
                default:
                    return false;
            }
        }
        return false;
    }
    
    


}
