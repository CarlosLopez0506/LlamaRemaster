using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int position;
    public int cizanaPoints;
    public int playerControllerNumber;
    public Debuff[] debuffList;
    public Gamepad assignedGamepad;
    public GameObject llamaPiece;
    public GameObject card;
    public Transform[] targetCubes;

    public PlayerData(string name, int initialPosition, Gamepad gamepad, int controllerNumber)
    {
        playerName = name;
        position = initialPosition;
        cizanaPoints = 0;
        assignedGamepad = gamepad;
        playerControllerNumber = controllerNumber;
        InitializeDebuffs();
    }

    public void InitializeDebuffs()
    {
        string[] debuffNames = new string[] { "Mute", "Pause", "VolumeUp", "VolumeDown", "Rewind", "FastForward" };
        int[] cost = new int[] { 100, 200, 100, 50, 300, 250 };

        debuffList = new Debuff[debuffNames.Length];

        for (int i = 0; i < debuffNames.Length; i++)
        {
            debuffList[i] = new Debuff(debuffNames[i], cost[i]);
        }
    }

    public void SetCard(GameObject card)
    {
        this.card = card;
    }

    public void SetTargetCubes(Transform[] cubes)
    {
        ClearTargetCubes(); // Clear existing target cubes before setting new ones
        targetCubes = cubes;
    }

    public void ClearTargetCubes()
    {
        // Clear the content of the array without destroying the associated objects
        targetCubes = Array.Empty<Transform>();
    }

    public void AddCizanaPoints(int pointsToAdd)
    {
        cizanaPoints += pointsToAdd;
        PlayerCardManager.Instance.UpdateCardData(PlayerDataManager.Instance.allPlayers);
    }

    public void SubtractCizanaPoints(int pointsToSubtract)
    {
        cizanaPoints = Mathf.Max(0, cizanaPoints - pointsToSubtract);
    }

    public void AdjustPosition(int newPosition)
    {
        position = newPosition;
    }

    public void MoveForward()
    {
        // Move the player forward by incrementing the position
        position++;
        PlayerCardManager.Instance.UpdateCardData(PlayerDataManager.Instance.allPlayers);
    }

    public void MoveBackward()
    {
        // Move the player backward by decrementing the position, but not below 1
        position = Mathf.Max(1, position - 1);
    }

    public Vector3 GetPlayerCurrentPosition()
    {
        int pieceIndex = playerControllerNumber - 1;
        return PieceManager.Instance.playerPieces[pieceIndex].position;
    }

    public void SetPlayerPosition(Vector3 positionToSet)
    {
        int pieceIndex = playerControllerNumber - 1;
        PieceManager.Instance.MovePieceToPosition(pieceIndex, positionToSet);
    }

    public bool ButtonPressed(Button button)
    {
        switch (button)
        {
            case Button.A:
                if (assignedGamepad.aButton.wasPressedThisFrame)
                    Debug.Log($"{playerName} pressed the {button} button!");
                return assignedGamepad.aButton.wasPressedThisFrame;
            case Button.B:
                if (assignedGamepad.bButton.wasPressedThisFrame)
                    Debug.Log($"{playerName} pressed the {button} button!");
                return assignedGamepad.bButton.wasPressedThisFrame;
            case Button.X:
                if (assignedGamepad.xButton.wasPressedThisFrame)
                    Debug.Log($"{playerName} pressed the {button} button!");
                return assignedGamepad.xButton.wasPressedThisFrame;
            case Button.Y:
                if (assignedGamepad.yButton.wasPressedThisFrame)
                    Debug.Log($"{playerName} pressed the {button} button!");
                return assignedGamepad.yButton.wasPressedThisFrame;
            default:
                return false;
        }
    }

    public enum JoystickDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }


    public JoystickDirection GetJoystickDirection()
    {
        Vector2 leftStickInput = assignedGamepad.leftStick.ReadValue();

        float sensitivityThreshold = 0.12f;

        if (leftStickInput.magnitude < sensitivityThreshold)
        {
            return JoystickDirection.None;
        }

        float angle = Mathf.Atan2(leftStickInput.y, leftStickInput.x) * Mathf.Rad2Deg;

        // Ensure the angle is between 0 and 360 degrees
        angle = (angle + 360) % 360;

        const float UpRange = 90;
        const float DownRange = 270;
        const float LeftRange = 180;

        if (angle <= UpRange + 45 && angle >= UpRange - 45)
        {
            // Debug.Log("Joystick Direction: Up");
            return JoystickDirection.Up;
        }
        else if (angle >= DownRange - 45 && angle < DownRange + 45)
        {
            // Debug.Log("Joystick Direction: Down");
            return JoystickDirection.Down;
        }
        else if (angle <= LeftRange + 45 && angle > LeftRange - 45)
        {
            // Debug.Log("Joystick Direction: Left");
            return JoystickDirection.Left;
        }
        else
        {
            // Debug.Log("Joystick Direction: Right");
            return JoystickDirection.Right;
        }
    }


    public enum Button
    {
        A,
        B,
        X,
        Y,
    }
}