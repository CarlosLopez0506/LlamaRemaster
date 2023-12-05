using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BDManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    //Useful for adding functionality to player initialization
    //NOT NECESSARY if calling Initialize() elsewhere
    public UnityEvent initializePlayersEvent;
    public Transform spawnPos1;
    public Transform spawnPos2;
    public LlamasContainer llamasContainer;
    public float gameDuration;
    private float _timer;
    private List<LlamaPoints> _llamaPointsList = new();
    private MinigameManager _minigameManager;

    private void Start()
    {
        initializePlayersEvent.Invoke();
        _minigameManager = GameObject.FindObjectOfType<MinigameManager>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= gameDuration)
        {
            if (_minigameManager)
            {
                GameOver();
            }
            else
            {
                Debug.Log("Time's up!");
            }
        }
    }

    //Use to create players by passing their data
    public void Initialize(PlayerData pd1, PlayerData pd2)
    {
        string player1Name = pd1.playerName;
        string player2Name = pd2.playerName;
        
        SpawnPlayer(player1Name, spawnPos1.position);
        SpawnPlayer(player2Name, spawnPos2.position);
    }

    public void Initialize(string pd1, string pd2)
    {
        SpawnPlayer(pd1, spawnPos1.position);
        SpawnPlayer(pd2, spawnPos2.position);
    }

    private void SpawnPlayer(string playerName, Vector3 pos)
    {
        switch (playerName)
        {
            case "Many":
                playerInputManager.playerPrefab = llamasContainer.many;
                break;
            case "Moe":
                playerInputManager.playerPrefab = llamasContainer.moe;
                break;
            case "Maroon":
                playerInputManager.playerPrefab = llamasContainer.maroon;
                break;
            case "Mei":
                playerInputManager.playerPrefab = llamasContainer.mei;
                break;
        }
        var playerInstance = playerInputManager.JoinPlayer();
        _llamaPointsList.Add(playerInstance.gameObject.GetComponent<LlamaPoints>());
        playerInstance.gameObject.transform.position = pos;
    }

    private void GameOver()
    {
        List<float> points = new();
        foreach (LlamaPoints llamaPoints in _llamaPointsList)
        {
            points.Add(llamaPoints.GetPoints());
        }
        _minigameManager.GameOver(points);
    }

    [System.Serializable]
    public class LlamasContainer
    {
        public GameObject many;
        public GameObject moe;
        public GameObject maroon;
        public GameObject mei;
        
    }
}
