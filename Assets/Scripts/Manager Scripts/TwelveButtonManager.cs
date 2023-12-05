using System;
using System.Collections;
using System.Collections.Generic;
using Manager_Scripts;
using UnityEngine;

public class TwelveButtonManager : MonoBehaviour
{
    [SerializeField] private ButtonEventsHandler _buttonEventsHandler;

    void Start()
    {
    }


    public GameObject[] BuscarYOrdenarBotonesPorNombre()
    {
        GameObject[] tempTwelveButton = GameObject.FindGameObjectsWithTag("TwelveButton");

        List<GameObject> listaDeBotones = new List<GameObject>(tempTwelveButton);

        listaDeBotones.Sort((a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));

        GameObject[] twelveButtons = listaDeBotones.ToArray();

        for (int i = 0; i < twelveButtons.Length; i++)
        {
            Debug.Log("Botón #" + i + ": " + twelveButtons[i].name);
        }

        return twelveButtons;
    }

    public IEnumerator PlaceTrap(PlayerData player, string trapName)
    {
        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.Instance.cameras[0], CameraSwitcher.Instance.cameras[1]);

        TwelveButton selectedButton = null;


        var currentStoreButton = 0;
        float inputCooldown = 0.25f;

        while (true)
        {
            selectedButton = TwelveButtonList.Instance.GetTwelveButton(currentStoreButton);
            for (int i = 0; i < TwelveButtonList.Instance.twelveButtonsArray.Count; i++)
            {
                _buttonEventsHandler.ResetButtonColor(TwelveButtonList.Instance.GetTwelveButton(i).ButtonPrefab,
                    TwelveButtonList.Instance.GetTwelveButton(i).OriginalColor);
            }

            _buttonEventsHandler.ChangeButtonColor(selectedButton.ButtonPrefab);

            yield return new WaitForSeconds(inputCooldown);

            PlayerData.JoystickDirection joystickDirection = PlayerData.JoystickDirection.None;

            yield return new WaitUntil(() =>
            {
                joystickDirection = player.GetJoystickDirection();
                return joystickDirection != PlayerData.JoystickDirection.None ||
                       player.ButtonPressed(PlayerData.Button.A);
            });

            switch (joystickDirection)
            {
                case PlayerData.JoystickDirection.Left:
                    currentStoreButton = (currentStoreButton + 1) % TwelveButtonList.Instance.twelveButtonsArray.Count;
                    break;
                case PlayerData.JoystickDirection.Right:
                    break;
                case PlayerData.JoystickDirection.None:
                    break;
            }

            if (player.ButtonPressed(PlayerData.Button.A))
            {
                Debug.Log(selectedButton.ButtonNumber);
                yield return StartCoroutine(ActivateTrap(selectedButton, trapName));
                CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.Instance.cameras[1],
                    CameraSwitcher.Instance.cameras[3]);
                break;
            }
        }

        _buttonEventsHandler.ResetButtonColor(selectedButton.ButtonPrefab,
            TwelveButtonList.Instance.GetTwelveButton(0).OriginalColor);
        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.Instance.cameras[3], CameraSwitcher.Instance.cameras[1]);
    }

    private IEnumerator ActivateTrap(TwelveButton button, string debuffName)
    {
        switch (debuffName)
        {
            case "Mute":
                button.TrapType = debuffName;
                yield return StartCoroutine(ProcessTraps());
                Debug.Log("Mute debuff selected");

                break;

            case "Pause":
                Debug.Log("Pause debuff selected");
                break;

            case "VolumeUp":
                Debug.Log("VolumeUp debuff selected");
                button.TrapType = debuffName;
                yield return StartCoroutine(ProcessTraps());
                break;

            case "VolumeDown":
                Debug.Log("VolumeDown debuff selected");
                button.TrapType = debuffName;
                yield return StartCoroutine(ProcessTraps());
                break;

            case "Rewind":
                Debug.Log("Rewind debuff selected");
                break;

            case "FastForward":
                Debug.Log("FastForward debuff selected");
                break;

            default:
                Debug.LogWarning("Debuff not recognized: " + debuffName);
                break;
        }

        yield return null;
    }

    public IEnumerator ProcessTraps()
    {
        foreach (var player in PlayerDataManager.Instance.allPlayers)
        {
            TwelveButton twelveButton = TwelveButtonList.Instance.GetTwelveButton(player.position-1);
            switch (twelveButton.TrapType)
            {
                case "Mute":
                {
                    player.ModifyDebuffState(twelveButton.TrapType);
                    Debug.Log(player.debuffList[0].debuffData["Mute"]);
                    break;
                }
                case "Pause":
                {
                    player.ModifyDebuffState(twelveButton.TrapType);
                    
                    break;
                }
                case "VolumeUp":
                {
                    player.ModifyDebuffState(twelveButton.TrapType);
                    Debug.Log(player.IsDebuffActive(twelveButton.TrapType));
                    break;
                }

                case "VolumeDown":
                {
                    player.ModifyDebuffState(twelveButton.TrapType);
                    Debug.Log(player.IsDebuffActive(twelveButton.TrapType));                    break;
                }

                case "Rewind":
                {
                    break;
                }
                case "FastForward":
                {
                    yield return StartCoroutine(PieceManager.Instance.MovePieceByCubes(player, 1));
                    break;
                }
            }
        }

        {
        }
    }
}