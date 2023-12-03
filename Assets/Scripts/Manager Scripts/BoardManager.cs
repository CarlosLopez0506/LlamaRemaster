using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    private int _currentPlayerIndex;
    private bool _isGameStarted;
    private bool _waitingForButtonPress = true;
    public GameObject masterControl;
    private Vector3 masterControlPosition;
    private const bool Counting = true;
    private int _points;
    private List<PlayerData> newPlayerTurns = new List<PlayerData>();


    void Awake()
    {
        ManageSingleton();
    }

    void Start()
    {
        InitializePlayers();
        StartCoroutine(StartGame());
    }

    void ManageSingleton()
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

    void InitializePlayers()
    {
        PlayerDataManager.Instance.InitializePlayers();
    }


    void InitializeGameComponents()
    {
        PlayerCardManager.Instance.CreatePlayerCards();
        PieceManager.Instance.CreatePlayerPieces(PlayerDataManager.Instance.allPlayers);
        PieceManager.Instance.SetPiecesLastPositions();
        PieceManager.Instance.SetAllPlayers(PlayerDataManager.Instance.allPlayers);
        PlayerCardManager.Instance.UpdateCardData(PlayerDataManager.Instance.allPlayers);
    }


    public void ReloadBoard(List<string> orderedPlayers)
    {
        StartCoroutine(GameEvents.Instance.InitializeBoardScene(orderedPlayers));
    }


    List<PlayerData> ShufflePlayers(List<PlayerData> players)
    {
        int count = players.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int randomIndex = Random.Range(i, count);
            (players[i], players[randomIndex]) = (players[randomIndex], players[i]);
        }

        return players;
    }

    void Update()
    {
        HandleTurnInput();
    }

    void HandleTurnInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartNextTurn();
        }

        HandlePlayerInput();
    }

    void HandlePlayerInput()
    {
        if (PlayerDataManager.Instance.allPlayers.Count > 0)
        {
            _currentPlayerIndex = Mathf.Clamp(_currentPlayerIndex, 0, PlayerDataManager.Instance.allPlayers.Count - 1);
            PlayerData currentPlayer = PlayerDataManager.Instance.allPlayers[_currentPlayerIndex];

            if (currentPlayer.assignedGamepad != null && currentPlayer.assignedGamepad.aButton.wasPressedThisFrame)
            {
                // Debug.Log($"{currentPlayer.playerName} pressed the 'A' button!");
            }
        }
        else
        {
            Debug.LogWarning("No players in the list.");
        }
    }

    void StartNextTurn()
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % PlayerDataManager.Instance.allPlayers.Count;
        PlayerCardManager.Instance.UpdateCardData(PlayerDataManager.Instance.allPlayers);
        PlayerData currentPlayer = PlayerDataManager.Instance.allPlayers[_currentPlayerIndex];
        Debug.Log($"Starting turn for {currentPlayer.playerName}");
    }


    IEnumerator StartGame()
    {
        if (_isGameStarted)
        {
            Debug.LogWarning("Game already started.");
        }

        _isGameStarted = true;
        InitializeGameComponents();
        yield return StartCoroutine(GameEvents.Instance.ManageDialogIntroduction());
        // yield return StartCoroutine(AssignTurnOrderCoroutine());
        // yield return new WaitForSeconds(2f);
        // MinigameManager.Instance.ChooseRandomMinigame();
        StartCoroutine(GameEvents.Instance.LaunchStore());
    }

    IEnumerator AssignTurnOrderCoroutine()
    {
        masterControl = GameObject.FindGameObjectWithTag("MasterControl");

        if (masterControl != null)
        {
            masterControlPosition = masterControl.transform.position;
            Debug.Log($"La posición del objeto es: {masterControlPosition}");


            yield return MovePlayersToMasterControl(PlayerDataManager.Instance.allPlayers);
            CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.Instance.cameras[2],
                CameraSwitcher.Instance.cameras[1]);
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con la etiqueta: MasterControl");
        }
    }

    IEnumerator MovePlayersToMasterControl(List<PlayerData> playerWithoutTurn)
    {
        Dictionary<PlayerData, int> miDiccionario = new Dictionary<PlayerData, int>();
        List<PlayerData> jugadoresRepetidos = new List<PlayerData>();

        foreach (var player in playerWithoutTurn)
        {
            yield return StartCoroutine(MovePlayerWithTextMesh(player, miDiccionario, jugadoresRepetidos));
        }

        // Process the results, print unique players, and handle repeated players
        yield return StartCoroutine(ProcessResults(miDiccionario, jugadoresRepetidos));
    }

    IEnumerator MovePlayerWithTextMesh(PlayerData player, Dictionary<PlayerData, int> miDiccionario,
        List<PlayerData> jugadoresRepetidos)
    {
        _points = 0;

        GameObject playerPiece = PieceManager.Instance.pieces[player.playerControllerNumber - 1];

        TextMeshPro existingTextMesh = playerPiece.GetComponentInChildren<TextMeshPro>();

        TextMeshPro textMesh;
        GameObject textMeshContainer;

        if (existingTextMesh == null)
        {
            textMeshContainer = new GameObject("TextMeshContainer");
            textMeshContainer.transform.parent = playerPiece.transform;

            textMesh = textMeshContainer.AddComponent<TextMeshPro>();

            textMesh.fontSize = 8;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMeshContainer.transform.localPosition = new Vector3(0f, 3.5f, 0f);
            textMeshContainer.transform.localRotation = Quaternion.Euler(new Vector3(30f, 180f, 0f));
        }
        else
        {
            // If TextMeshPro component exists, use it
            textMesh = existingTextMesh;
            textMeshContainer = existingTextMesh.gameObject;
        }

        Vector3 playerCurrentPosition = player.GetPlayerCurrentPosition();

        yield return new WaitForSeconds(1f);

        player.SetPlayerPosition(masterControlPosition);

        var coroutineCount = StartCoroutine(CountEverySecond());

        while (_waitingForButtonPress && !player.ButtonPressed(PlayerData.Button.A))
        {
            textMesh.text = _points.ToString();

            yield return null;
        }

        StopCoroutine(coroutineCount);
        yield return null;
        player.SetPlayerPosition(playerCurrentPosition);
        yield return null;
        Debug.Log(_points);
        miDiccionario.Add(player, Convert.ToInt32(textMesh.text));
        textMeshContainer.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

        _waitingForButtonPress = true;
    }

    IEnumerator ProcessResults(Dictionary<PlayerData, int> miDiccionario, List<PlayerData> jugadoresRepetidos)
    {
        yield return null;

        var diccionarioOrdenado = miDiccionario.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        List<PlayerData> listaOrdenadaDeJugadores = diccionarioOrdenado
            .GroupBy(x => x.Value)
            .Where(group => group.Count() == 1)
            .Select(group => group.First().Key)
            .ToList();

        foreach (var jugadorRepetido in diccionarioOrdenado
                     .Where(x => diccionarioOrdenado.Count(y => y.Value == x.Value) > 1).Select(x => x.Key))
        {
            jugadoresRepetidos.Add(jugadorRepetido);
        }

        foreach (var jugador in listaOrdenadaDeJugadores)
        {
            Debug.Log("Jugador Único: " + jugador.playerName);
        }

        foreach (var jugadorRepetido in jugadoresRepetidos)
        {
            Debug.Log("Jugador Repetido: " + jugadorRepetido.playerName);
        }

        Debug.Log(listaOrdenadaDeJugadores.Count);

        if (listaOrdenadaDeJugadores.Count > 0)
        {
            foreach (var playerWithTurn in listaOrdenadaDeJugadores)
            {
                newPlayerTurns.Add(playerWithTurn);
            }
        }

        Debug.Log(jugadoresRepetidos.Count);

        if (jugadoresRepetidos.Count > 0)
        {
            yield return StartCoroutine(MovePlayersToMasterControl(jugadoresRepetidos));
        }
    }

    private IEnumerator CountEverySecond()
    {
        while (Counting)
        {
            yield return new WaitForSeconds(0.05f);

            _points++;

            if (_points > 6)
            {
                _points = 1;
            }
        }
    }
}