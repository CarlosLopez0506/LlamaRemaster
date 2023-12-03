using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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

    public IEnumerator InitializeBoardScene(List<string> orderedPlayers)
    {
        yield return null;
        InitializeCameras();
        yield return null;
        InitializePlayers();
        yield return null;
        InitializePlayerCards();
        yield return null;
        InitializePlayerPieces();
        yield return null;
        PositionPlayers(orderedPlayers);
        yield return StartCoroutine(LaunchStore());
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

    private void PositionPlayers(List<string> orderedPlayers)
    {
        foreach (PlayerData player in PlayerDataManager.Instance.allPlayers)
        {
            for (int i = 0; i < orderedPlayers.Count; i++)
            {
                if (player.playerName == orderedPlayers[i])
                {
                    PositionPlayerBasedOnOrder(player, i + 1);
                }
            }
        }
    }

    private void PositionPlayerBasedOnOrder(PlayerData player, int currentPosition)
    {
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
                    ResetButtonColor(storeButton, buttonColor);
                }

                ChangeButtonColor(selectedButton);

                yield return new WaitForSeconds(inputCooldown); // Introduce a delay before checking for input

                PlayerData.JoystickDirection joystickDirection = player.GetJoystickDirection();

                switch (joystickDirection)
                {
                    case PlayerData.JoystickDirection.Left:
                        currentStoreButton = (currentStoreButton + 1) % storeButtons.Length;
                        break;
                    case PlayerData.JoystickDirection.Right:
                        break;
                }

                if (player.ButtonPressed(PlayerData.Button.A))
                {
                    Debug.Log(currentStoreButton);
                    break;
                }

                yield return null;
            }

            ResetButtonColor(selectedButton, buttonColor);

            player.SetPlayerPosition(playerCurrentPosition);
        }
    }
    else
    {
        Debug.LogError("No se encontró ningún objeto con la etiqueta: MasterControl");
    }

    yield return null;
}
    private void ChangeButtonColor(GameObject button)
    {
        Renderer renderer = button.GetComponent<Renderer>();
        if (renderer != null)
        {
            float attenuationFactor = 0.8f;
            attenuationFactor = Mathf.Clamp(attenuationFactor, 0f, 1f);

            var material = renderer.material;
            Color currentColor = material.color;

            Color darkenedColor = new Color(currentColor.r * attenuationFactor,
                currentColor.g * attenuationFactor,
                currentColor.b * attenuationFactor,
                currentColor.a);

            material.color = darkenedColor;
        }
    }

    private void ResetButtonColor(GameObject button, Color color)
    {
        Renderer renderer = button.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Reset to the original color or your desired color
            renderer.material.color = color;
        }
    }
}