using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    public Canvas minigameCanvas;
    public RectTransform roulettePanel;
    public GameObject minigameTextPrefab;
    public List<string> minigameNames = new List<string>();
    public bool _chooseRandomMinigame = true;
    public bool _duel = false;
    public string channelButtonSceneToPlay;


    private int _selectedMinigameIndex;
    private List<PlayerInfo> _playerScores = new List<PlayerInfo>();

    private void Awake()
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

        InitializeMinigameNames();
    }

    private void InitializeMinigameNames()
    {
        minigameNames.Add("MG1");
        minigameNames.Add("MG2");
    }

    private void CreateMinigameText()
    {
        var pixelRect = minigameCanvas.pixelRect;
        float textHeight = pixelRect.height / minigameNames.Count;
        int initialPosition = (int)(pixelRect.height / 2 - textHeight / 2);

        for (int i = 0; i < minigameNames.Count; i++)
        {
            GameObject textObj = Instantiate(minigameTextPrefab, minigameCanvas.transform);
            RectTransform textRect = textObj.GetComponent<RectTransform>();

            textRect.sizeDelta = new Vector2(minigameCanvas.pixelRect.width, textHeight);
            textRect.anchoredPosition = new Vector2(0, initialPosition - i * textHeight);
            textRect.localPosition = new Vector3(textRect.localPosition.x, textRect.localPosition.y, 0);

            textObj.GetComponentInChildren<TextMeshProUGUI>().text = minigameNames[i];
        }
    }

    public void ChannelButton(string sceneName)
    {
        if (sceneName != "Duel")
        {
            _chooseRandomMinigame = false;
            channelButtonSceneToPlay = sceneName;
        }
        else
        {
            Debug.Log("Duel Activated");
            _duel = true;
        }
        
    }

    public void StartMinigame()
    {
        Debug.Log(_chooseRandomMinigame);
        Debug.Log(channelButtonSceneToPlay);
        if (_chooseRandomMinigame == false && channelButtonSceneToPlay != null)
        {
            StartCoroutine(LoadMinigameScene(channelButtonSceneToPlay));
            _chooseRandomMinigame = true;
            channelButtonSceneToPlay = null;
        }
        else if (_chooseRandomMinigame && !_duel)
        {
            ChooseRandomMinigame();
        }
        else if (_duel)
        {
            StartCoroutine(LoadMinigameScene("Duel"));
            _duel = false;
        }
    }

    public void ChooseRandomMinigame()
    {
        minigameCanvas = GameObject.Find("TV")?.GetComponent<Canvas>();
        roulettePanel = minigameCanvas?.GetComponent<RectTransform>();

        if (minigameCanvas != null)
        {
            StartCoroutine(SpinRoulette());
        }
        else
        {
            Debug.LogError("Canvas named 'TV' not found.");
        }
    }

    private IEnumerator SpinRoulette()
    {
        float spinDuration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            float t = elapsedTime / spinDuration;
            float lerpedY = Mathf.Lerp(0, -_selectedMinigameIndex * minigameCanvas.pixelRect.height, t);

            if (minigameCanvas != null && roulettePanel != null)
            {
                roulettePanel.anchoredPosition = new Vector2(0, lerpedY);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        int randomIndex = Random.Range(0, minigameNames.Count);
        _selectedMinigameIndex = randomIndex;

        string minigameScene = minigameNames[_selectedMinigameIndex];
        StartCoroutine(LoadMinigameScene(minigameScene));
    }

    public IEnumerator LoadMinigameScene(string sceneName)
    {
        yield return null;
        SceneManager.LoadScene(sceneName);
    }

    private struct PlayerInfo
    {
        public readonly PlayerData playerData;
        public readonly float score;

        public PlayerInfo(PlayerData data, float steps)
        {
            playerData = data;
            score = steps;
        }
    }

    public void GameOver(List<float> scoreTexts)
    {
        StopGameAudio();
        PlayerDataManager playerDataManager = PlayerDataManager.Instance;

        if (playerDataManager == null || playerDataManager.allPlayers == null || scoreTexts == null ||
            scoreTexts.Count != playerDataManager.allPlayers.Count)
        {
            Debug.LogError("Invalid data or mismatched counts.");
            return;
        }
        _playerScores.Clear(); 

        for (int i = 0; i < scoreTexts.Count; i++)
        {
            if (i >= playerDataManager.allPlayers.Count)
            {
                Debug.LogError($"Index {i} exceeds the bounds of the player list.");
                break;
            }

            float playerScore = scoreTexts[i];
            PlayerData playerData = playerDataManager.allPlayers[i];

            PlayerInfo playerInfo = new PlayerInfo(playerData, playerScore);
            _playerScores.Add(playerInfo);
        }

        List<PlayerInfo> orderedPlayers = _playerScores.OrderByDescending(player => player.score).ToList();

        Debug.Log("Ordered Player Names and Scores (Descending):");

        foreach (PlayerInfo playerInfo in orderedPlayers)
        {
            Debug.Log($"{playerInfo.playerData.playerName}: {playerInfo.score} points");
        }

        List<string> orderedPlayerNames = orderedPlayers.Select(player => player.playerData.playerName).ToList();
        Debug.Log(orderedPlayerNames.Count);
        ResetGameState();
        SendResultsToNextMiniGame(orderedPlayerNames);
    }

    private void StopGameAudio()
    {
        // Add logic to stop any game-related audio
    }

    private void SendResultsToNextMiniGame(List<string> orderedPlayerNames)
    {
        BoardManager.Instance.ReloadBoard(orderedPlayerNames);
    }

    private void ResetGameState()
    {
        SceneManager.LoadScene("Board");
    }
}