using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    private int _currentPlayerIndex;
    private bool _isGameStarted;
    private bool _waitingForButtonPress = true;
    private const bool Counting = true;
    private int _points;


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
        PieceManager.Instance.CreatePlayerPieces(Gamepad.all.Count);
        PieceManager.Instance.SetPiecesLastPositions();
        PieceManager.Instance.SetAllPlayers(PlayerDataManager.Instance.allPlayers);
        PlayerCardManager.Instance.UpdateCardData(PlayerDataManager.Instance.allPlayers);
    }

    private IEnumerator ManageDialogIntroduction()
    {
        string[] paragraphs =
        {
            "Welcome to Llama Masters – the ultimate party filled with laughter, strategy, and llama magic. Assemble your friends, unleash your inner llama wrangler, and let the llama drama unfold!",
            "How it works: Players get unique numbers to determine the order. The one with the lowest or highest number kicks off as the first Llama Master. The llama baton is passed, ensuring everyone gets their chance to shine!",
            "Grab your llama hats, dust off your llama dance moves, and get ready for an epic showdown of wit and whimsy. Llama Masters is not just a game; it's a journey into the fantastical world of llama lore!",
            "May the llama gods be in your favor – let the llama-filled festivities begin!"
        };

        GameObject dialog = GameObject.Find("Dialog");
        RawImage rawImageEncontrada = GameObject.Find("Dialog")?.GetComponent<RawImage>();

        if (rawImageEncontrada != null)
        {
            rawImageEncontrada.enabled = true;
            GameObject dialogChild = dialog.transform.GetChild(0).gameObject;
            TextMeshProUGUI textDialog = dialogChild.GetComponent<TextMeshProUGUI>();
            textDialog.enabled = true;


            textDialog.color = Color.black;

            var textMeshProComponent = textDialog.GetComponentsInChildren<TextMeshProUGUI>(true)[0];
            textMeshProComponent.enableAutoSizing = true;

            foreach (var paragraph in paragraphs)
            {
                textDialog.text = paragraph;
                yield return null;
                Debug.Log(PlayerDataManager.Instance.IsButtonPressed(PlayerData.Button.A));

                yield return new WaitUntil(() => PlayerDataManager.Instance.IsButtonPressed(PlayerData.Button.A));
            }

            textDialog.enabled = false;
            rawImageEncontrada.enabled = false;
            CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.Instance.cameras[0],
                CameraSwitcher.Instance.cameras[2]);
        }
        else
        {
            Debug.Log(
                "RawImage no encontrada. Asegúrate de que el nombre sea correcto y de que el objeto tiene un componente RawImage.");
        }
    }

    public void ReloadBoard(List<string> orderedPlayers)
    {
        StartCoroutine(GameEvents.Instance.WaitForSceneLoadAndCreatePlayerCards(orderedPlayers));
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
        MinigameManager.Instance.ChooseRandomMinigame();
    }


    IEnumerator StartGame()
    {
        if (_isGameStarted)
        {
            Debug.LogWarning("Game already started.");
        }

        _isGameStarted = true;
        InitializeGameComponents();
        yield return (ManageDialogIntroduction());
        yield return StartCoroutine(AssignTurnOrderCoroutine());
        // Debug.Log("Game started successfully.");
    }

    IEnumerator AssignTurnOrderCoroutine()
    {
        GameObject masterControl = GameObject.FindGameObjectWithTag("MasterControl");

        if (masterControl != null)
        {
            Vector3 masterControlPosition = masterControl.transform.position;
            Debug.Log($"La posición del objeto es: {masterControlPosition}");


            yield return MovePlayersToMasterControl(masterControlPosition);
            CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.Instance.cameras[2],CameraSwitcher.Instance.cameras[1]);
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con la etiqueta: MasterControl");
        }
    }

    IEnumerator MovePlayersToMasterControl(Vector3 masterControlPosition)
    {
        foreach (var player in PlayerDataManager.Instance.allPlayers)
        {
            _points = 0;

            GameObject playerPiece = PieceManager.Instance.pieces[player.playerControllerNumber - 1];

            GameObject textMeshContainer = new GameObject("TextMeshContainer");
            textMeshContainer.transform.parent = playerPiece.transform;

            TextMeshPro textMesh = textMeshContainer.AddComponent<TextMeshPro>();

            textMesh.fontSize = 8;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMeshContainer.transform.localPosition = new Vector3(0f, 3.5f, 0f);
            textMeshContainer.transform.localRotation = Quaternion.Euler(new Vector3(30f, 180f, 0f));

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
            player.SetPlayerPosition(playerCurrentPosition);
            textMeshContainer.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

            _waitingForButtonPress = true;
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