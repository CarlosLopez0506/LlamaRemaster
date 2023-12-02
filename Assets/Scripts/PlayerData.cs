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
    public List<Debuff> debuffs;
    public Gamepad assignedGamepad;
    public GameObject llamaPiece;
    public Transform[] targetCubes;

    public PlayerData(string name, int initialPosition, Gamepad gamepad, int controllerNumber)
    {
        playerName = name;
        position = initialPosition;
        cizanaPoints = 0;
        debuffs = new List<Debuff>();
        assignedGamepad = gamepad;
        playerControllerNumber = controllerNumber;
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

    public enum Button
    {
        A,
        B,
        X,
        Y,
    }
    
    
}