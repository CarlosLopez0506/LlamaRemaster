using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Instance;

    public GameObject meiPrefab; // Prefab para la pieza
    public GameObject moePrefab; // Prefab para la pieza
    public GameObject maroonPrefab; // Prefab para la pieza
    public GameObject mannyPrefab; // Prefab para la pieza

    public List<GameObject> pieces = new List<GameObject>();
    public List<Transform> playerPieces = new List<Transform>();

    private List<PlayerData> allPlayers; // Add this line


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetAllPlayers(List<PlayerData> players)
    {
        allPlayers = players;
    }

    public void CreatePlayerPieces(List<PlayerData> players)
    {
        playerPieces.Clear();
        pieces.Clear();

        foreach (var player in players)
        {
            GameObject pieceObject;

            switch (player.playerName)
            {
                case "Mei":
                    pieceObject = Instantiate(meiPrefab);
                    break;
                case "Maroon":
                    pieceObject = Instantiate(maroonPrefab); 
                    break;
                case "Manny":
                    pieceObject = Instantiate(mannyPrefab); 
                    break;
                case "Moe":
                    pieceObject = Instantiate(moePrefab); 
                    break;
                default:
                    Debug.LogError($"Piece for player '{player.playerName}' does not exist.");
                    continue;
            }

            playerPieces.Add(pieceObject.transform);
            pieces.Add(pieceObject);
        }
    }

    public void SetPiecesLastPositions()
    {
        for (int playerIndex = 0; playerIndex < PlayerDataManager.Instance.allPlayers.Count; playerIndex++)
        {
            Transform[] targetCubes = PlayerDataManager.Instance.allPlayers[playerIndex].targetCubes;

            if (targetCubes.Length > 0)
            {
                int lastPosition = PlayerDataManager.Instance.allPlayers[playerIndex].position - 1;

                if (lastPosition >= 0 && lastPosition < targetCubes.Length)
                {
                    Vector3 lastPiecePosition = targetCubes[lastPosition].position;
                    PieceManager.Instance.MovePieceToPosition(playerIndex, lastPiecePosition);
                }
                else
                {
                    Debug.LogError(
                        $"Invalid last position {lastPosition + 1} for player {playerIndex + 1}. Check player positions and target cubes.");
                }
            }
            else
            {
                Debug.LogWarning($"No target cubes found for player {playerIndex + 1}");
            }
        }
    }


    public void MovePieceToPosition(int playerIndex, Vector3 targetPosition)
    {
        if (playerIndex >= 0 && playerIndex < playerPieces.Count)
        {
            Transform pieceTransform = playerPieces[playerIndex];
            pieceTransform.position = targetPosition;
        }
        else
        {
            Debug.LogWarning($"Invalid player index: {playerIndex}");
        }
    }


    public IEnumerator MovePieceByCubes(PlayerData player, int numCubesToMove)
    {
        Transform pieceTransform = playerPieces[player.playerControllerNumber - 1];
        var startPlace = player.position;
        var finalPlace = player.position + numCubesToMove;

        for (int i = startPlace; i < finalPlace; i++)
        {
            // Get the new position and set the Y-coordinate to the ground level
            Vector3 newPosition = player.targetCubes[i].position;
            newPosition.y = pieceTransform.position.y; // Set Y to the current Y (ground level)
            yield return MovePieceParabolic(pieceTransform, newPosition, 0.5f);


            player.MoveForward();
            PlayerCardManager.Instance.UpdateCardData(PlayerDataManager.Instance.allPlayers);
        }
    }

    private IEnumerator MovePieceParabolic(Transform pieceTransform, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = pieceTransform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // Use a parabolic (quadratic) interpolation for the Y-coordinate
            pieceTransform.position = new Vector3(
                Mathf.Lerp(initialPosition.x, targetPosition.x, t),
                initialPosition.y + Mathf.Sin(t * Mathf.PI) * 2f, // Adjust the jump height
                Mathf.Lerp(initialPosition.z, targetPosition.z, t)
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pieceTransform.position = targetPosition; // Ensure the final position is exact
    }


    public Transform[] GetTargetCubes(int spawnCount, int playerIndex)
    {
        List<Transform> targetCubes = new List<Transform>();

        for (int i = 0; i < spawnCount; i++)
        {
            // Find cubes in the scene under the specified spawn parent
            GameObject spawnObject = GameObject.Find($"Spawn ({i})");

            if (spawnObject != null)
            {
                // Find cubes based on player index
                Transform cube = playerIndex == 0
                    ? spawnObject.transform.Find("Cube")
                    : spawnObject.transform.Find($"Cube ({playerIndex})");

                if (cube != null)
                {
                    targetCubes.Add(cube);
                    //Debug.Log($"Found {cube.name} under 'Spawn ({i})' for player {playerIndex}.");
                }
                else
                {
                    Debug.LogWarning($"Cube not found under 'Spawn ({i})' for player {playerIndex}.");
                }
            }
            else
            {
                Debug.LogWarning($"Spawn with name 'Spawn ({i})' not found in the scene.");
            }
        }

        return targetCubes.ToArray();
    }
}