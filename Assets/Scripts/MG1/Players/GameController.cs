using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public Stretch[] objects;

    void Start()
    {
        PlayerDataManager playerDataManager = PlayerDataManager.Instance;

        if (playerDataManager != null)
        {
            List<PlayerData> allPlayers = playerDataManager.allPlayers;

            if (objects.Length > 0 && allPlayers.Count > 0)
            {
                // Ensure that the number of objects and players match
                int numObjects = Mathf.Min(objects.Length, allPlayers.Count);

                for (int i = 0; i < numObjects; i++)
                {
                    objects[i].Initialize(allPlayers[i].assignedGamepad, allPlayers[i].playerName);
                }
            }
            else
            {
                Debug.LogError("Objects or players not set in GameController.");
            }
        }
        else
        {
            Debug.LogError("PlayerDataManager instance not found!");
        }
    }
}
