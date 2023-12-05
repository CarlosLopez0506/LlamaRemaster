using System;
using System.Collections;
using System.Collections.Generic;
using Manager_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;
    [SerializeField] private TwelveButtonManager _twelveButtonManager;
    [SerializeField] private ButtonEventsHandler _buttonEventsHandler;

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

    public IEnumerator InitializeBoardScene(List<string> orderedPlayers)
    {
        yield return null;
        MinigameManager.Instance._chooseRandomMinigame = true;
        MinigameManager.Instance.channelButtonSceneToPlay = null;
        InitializeCameras();
        yield return null;
        InitializePlayers();
        yield return null;
        InitializePlayerCards();
        yield return null;
        InitializePlayerPieces();
        yield return null;
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(PositionPlayers(orderedPlayers));
        TwelveButtonList.Instance.FindButtons();
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(LaunchStore());
        MinigameManager.Instance.StartMinigame();
    }

    private void InitializeCameras()
    {
        CameraSwitcher.Instance.FindCameras();
        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.Instance.cameras[0], CameraSwitcher.Instance.cameras[1]);
    }

    private void InitializePlayers()
    {
        PlayerDataManager.Instance.AddCubes();
    }

    private void InitializePlayerCards()
    {
        PlayerCardManager.Instance.CreatePlayerCards();
        PlayerCardManager.Instance.UpdateCardData(PlayerDataManager.Instance.allPlayers);
    }

    private void InitializePlayerPieces()
    {
        PieceManager.Instance.CreatePlayerPieces(PlayerDataManager.Instance.allPlayers);
        PieceManager.Instance.SetPiecesLastPositions();
        PieceManager.Instance.SetAllPlayers(PlayerDataManager.Instance.allPlayers);
    }

    private IEnumerator PositionPlayers(List<string> orderedPlayers)
    {
        foreach (var player in PlayerDataManager.Instance.allPlayers)
        {
            foreach (var playerName in orderedPlayers)
            {
                if (player.playerName == playerName)
                {
                    int placement = orderedPlayers.IndexOf(playerName) + 1;
                    switch (placement)
                    {
                        case 1:
                            yield return StartCoroutine(
                                PieceManager.Instance.MovePieceByCubes(player, MoveByCubesValue));
                            continue;
                        case 2:
                            player.AddCizanaPoints(PointsForSecondPlace);
                            continue;
                        case 3:
                            player.AddCizanaPoints(PointsForThirdPlace);
                            continue;
                        case 4:
                            player.AddCizanaPoints(PointsForFourthPlace);
                            continue;
                    }
                }
            }
        }
    }

    private const int MoveByCubesValue = 2;
    private const int PointsForSecondPlace = 25;
    private const int PointsForThirdPlace = 50;
    private const int PointsForFourthPlace = 75;


    public IEnumerator ManageDialogIntroduction()
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

    public IEnumerator LaunchStore()
    {
        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.Instance.cameras[1], CameraSwitcher.Instance.cameras[3]);
        GameObject masterControl = GameObject.FindGameObjectWithTag("MasterControl");
        Vector3 masterControlPosition = masterControl.transform.position;

        GameObject[] storeButtons = GameObject.FindGameObjectsWithTag("ButtonStore");
        GameObject selectedButton = null; // Track the currently selected button
        Color buttonColor = storeButtons[0].GetComponent<Renderer>().material.color;

        if (masterControl != null)
        {
            foreach (var player in PlayerDataManager.Instance.allPlayers)
            {
                var playerCurrentPosition = player.GetPlayerCurrentPosition();
                yield return new WaitForSeconds(1f);

                player.SetPlayerPosition(masterControlPosition);
                var currentStoreButton = 0;
                float inputCooldown = 0.25f;

                while (true)
                {
                    selectedButton = storeButtons[currentStoreButton];
                    foreach (GameObject storeButton in storeButtons)
                    {
                        _buttonEventsHandler.ResetButtonColor(storeButton, buttonColor);
                    }

                    _buttonEventsHandler.ChangeButtonColor(selectedButton);

                    yield return new WaitForSeconds(inputCooldown);

                    PlayerData.JoystickDirection joystickDirection = PlayerData.JoystickDirection.None;

                    yield return new WaitUntil(() =>
                    {
                        joystickDirection = player.GetJoystickDirection();
                        return joystickDirection != PlayerData.JoystickDirection.None ||
                               player.ButtonPressed(PlayerData.Button.A) || player.ButtonPressed(PlayerData.Button.B) ||
                               player.ButtonPressed(PlayerData.Button.Y);
                    });

                    switch (joystickDirection)
                    {
                        case PlayerData.JoystickDirection.Left:
                            currentStoreButton = (currentStoreButton + 1) % storeButtons.Length;
                            break;
                        case PlayerData.JoystickDirection.Right:
                            // Handle Right direction if needed
                            break;
                        case PlayerData.JoystickDirection.None:
                            // Handle the case when no joystick direction is pressed
                            break;
                    }

                    if (MinigameManager.Instance._duel && selectedButton.name == "ChannelButton")
                    {
                    }
                    else
                    {
                        if (player.ButtonPressed(PlayerData.Button.A))
                        {
                            Debug.Log(selectedButton.name);
                            yield return ProcessPurchase(player, selectedButton);

                            break;
                        }

                        if (player.ButtonPressed(PlayerData.Button.Y))
                        {
                            Debug.Log("You pressed Y");
                            MinigameManager.Instance.ChannelButton("Duel");
                            break;
                        }
                    }

                    if (player.ButtonPressed(PlayerData.Button.B))
                    {
                        Debug.Log("B button pressed");
                        Debug.Log("next player");
                        break;
                    }
                }

                _buttonEventsHandler.ResetButtonColor(selectedButton, buttonColor);

                player.SetPlayerPosition(playerCurrentPosition);
            }
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con la etiqueta: MasterControl");
        }
    }

    private IEnumerator ProcessPurchase(PlayerData player, GameObject selectedButton)
    {
        int playerWealth = player.cizanaPoints;
        Debug.Log(selectedButton.name);
        string whichButton = selectedButton.name;
        switch (whichButton)
        {
            case "ChannelButton":

                MinigameManager.Instance.ChannelButton("MG1");


                break;

            case "VolumeButton":
                yield return StartCoroutine(VolumeUI(player));

                break;

            case "MuteButton":
                yield return StartCoroutine(_twelveButtonManager.PlaceTrap(player, "Mute"));

                break;

            case "FastForward":

                break;

            case "Rewind":

                break;


            default:
                Debug.LogError("No button");
                break;
        }
    }

    public IEnumerator VolumeUI(PlayerData player)
    {
        string volume = null;
        PlayerData.JoystickDirection joystickDirection = PlayerData.JoystickDirection.None;

        yield return new WaitUntil(() =>
        {
            joystickDirection = player.GetJoystickDirection();
            return joystickDirection != PlayerData.JoystickDirection.None;
        });

        switch (joystickDirection)
        {
            case PlayerData.JoystickDirection.Up:
                yield return new WaitUntil(() =>
                {
                    joystickDirection = player.GetJoystickDirection();
                    return joystickDirection == PlayerData.JoystickDirection.None ||
                           joystickDirection == PlayerData.JoystickDirection.Down;
                });

                if (joystickDirection == PlayerData.JoystickDirection.None)
                {
                    volume = "VolumeUp";
                }

                break;
            case PlayerData.JoystickDirection.Down:
                yield return new WaitUntil(() =>
                {
                    joystickDirection = player.GetJoystickDirection();
                    return joystickDirection == PlayerData.JoystickDirection.None ||
                           joystickDirection == PlayerData.JoystickDirection.Up;
                });

                if (joystickDirection == PlayerData.JoystickDirection.None)
                {
                    volume = "VolumeDown";
                }

                break;
            case PlayerData.JoystickDirection.None:
                break;
        }

        yield return new WaitUntil(() => player.ButtonPressed(PlayerData.Button.A));

        yield return StartCoroutine(_twelveButtonManager.PlaceTrap(player, volume));
    }
}