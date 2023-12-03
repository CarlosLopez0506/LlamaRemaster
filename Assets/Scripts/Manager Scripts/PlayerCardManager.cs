using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerCardManager : MonoBehaviour
{
    public static PlayerCardManager Instance; // Singleton instance

    public GameObject cardMei;
    public GameObject cardMaroon;
    public GameObject cardMoe;
    public GameObject cardManny;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(
                gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePlayerCards()
    {
        // Find the parent dynamically by tag (adjust as needed)
        GameObject cardsParentObject = GameObject.FindWithTag("YourCardsParentTag");

        if (cardsParentObject == null)
        {
            Debug.LogError("Cards parent not found in the scene.");
            return;
        }

        Transform cardsParent = cardsParentObject.transform;
        var startingPosition = 0;
        foreach (var player in PlayerDataManager.Instance.allPlayers)
        {
            GameObject cardObject;

            switch (player.playerName)
            {
                case "Mei":
                    cardObject = Instantiate(cardMei, cardsParent);
                    player.SetCard(cardObject);
                    break;
                case "Maroon":
                    cardObject = Instantiate(cardMaroon, cardsParent);
                    player.SetCard(cardObject);
                    break;
                case "Manny":
                    cardObject = Instantiate(cardManny, cardsParent);
                    player.SetCard(cardObject);
                    break;
                case "Moe":
                    cardObject = Instantiate(cardMoe, cardsParent);
                    player.SetCard(cardObject);
                    break;
                default:
                    Debug.LogWarning($"Piece for player '{player.playerName}' does not exist.");
                    continue;
            }


            cardObject.transform.localPosition = new Vector3(-760 + startingPosition * 500, 440, 0);
            startingPosition++;
        }
    }


    public void UpdateCardData(List<PlayerData> allPlayers)
    {
        foreach (var player in allPlayers)
        {
            UpdateCardUI(player.card.transform, player.position, player.playerName, player.cizanaPoints);
        }
    }

    void UpdateCardUI(Transform cardTransform, int position, string playerName, int cizanaPoints)
    {
        // Check if the cardTransform is not null
        if (cardTransform == null)
        {
            Debug.LogError("Card transform is null.");
            return;
        }

        // Suponiendo que la estructura de la carta tiene elementos TextMeshProUGUI llamados "TileText", "NameText" y "CizanaText"
        Transform tileTextTransform = cardTransform.Find("TileText");
        Transform nameTextTransform = cardTransform.Find("NameText");
        Transform cizanaTextTransform = cardTransform.Find("CizanaText");

        // Check if any of the required components are not found
        if (tileTextTransform == null || nameTextTransform == null || cizanaTextTransform == null)
        {
            Debug.LogError("One or more TextMeshProUGUI components not found.");
            return;
        }

        // Get the TextMeshProUGUI components
        TextMeshProUGUI tileText = tileTextTransform.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = nameTextTransform.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cizanaText = cizanaTextTransform.GetComponent<TextMeshProUGUI>();

        // Check if any of the components are null before accessing their properties
        if (tileText != null && nameText != null && cizanaText != null)
        {
            tileText.text = "Posición: " + position.ToString();
            nameText.text = playerName;
            cizanaText.text = "Cizaña: " + cizanaPoints.ToString();
        }
        else
        {
            Debug.LogError("One or more TextMeshProUGUI components are null.");
        }
    }
}