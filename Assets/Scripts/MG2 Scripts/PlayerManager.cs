using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private ControlPossesion controlTransform;

    public GameObject[] playerPrefabs;
    private List<LlamaPoints> _points = new();

    private MinigameManager _minigameManager;

    private float _maxPoints;
    // Start is called before the first frame update
    void Start()
    {
        _minigameManager = GameObject.FindObjectOfType<MinigameManager>();
        for (int i = 0; i < PlayerDataManager.Instance.allPlayers.Count; i++)
        {
            playerInputManager.playerPrefab = playerPrefabs[i];
            var input = playerInputManager.JoinPlayer();
            _points.Add(input.gameObject.GetComponent<LlamaPoints>());
        }
    }

    public void GameOverEvent()
    {
        List<float> fPoints = new();
        foreach (LlamaPoints points in _points)
        {
            fPoints.Add(points.GetPoints());
        }
        _minigameManager.GameOver(fPoints);
    }

    public void GetMaxScore()
    {
        foreach (LlamaPoints points in _points)
        {
            if (points.GetPoints() > _maxPoints)
            {
                _maxPoints = points.GetPoints();
                controlTransform.followTransform = points.gameObject.transform;
            }
        }
    }
}
