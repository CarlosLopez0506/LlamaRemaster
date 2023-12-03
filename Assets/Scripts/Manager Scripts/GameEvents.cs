using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator WaitForSceneLoadAndCreatePlayerCards(List<string> orderedPlayers)
    {
        yield return null;

        PlayerDataManager.Instance.AddCubes();
        PlayerCardManager.Instance.CreatePlayerCards();
        PlayerCardManager.Instance.UpdateCardData(PlayerDataManager.Instance.allPlayers);
        PieceManager.Instance.CreatePlayerPieces(PlayerDataManager.Instance.allPlayers);

        PieceManager.Instance.SetPiecesLastPositions();

        PieceManager.Instance.SetAllPlayers(PlayerDataManager.Instance.allPlayers);

        foreach (PlayerData player in PlayerDataManager.Instance.allPlayers)
        {
            for (int i = 0; i < orderedPlayers.Count; i++)
            {
                if (player.playerName == orderedPlayers[i])
                {
                    int currentPosition = i + 1;
                    switch (currentPosition)
                    {
                        case 1:
                            PieceManager.Instance.MovePieceByCubes(player, 2);
                            break;
                        case 2:
                            player.AddCizanaPoints(25);
                            break;
                        case 3:
                            player.AddCizanaPoints(50);
                            break;
                        case 4:
                            player.AddCizanaPoints(75);
                            break;
                    }
                }
            }
        }
    }
}
