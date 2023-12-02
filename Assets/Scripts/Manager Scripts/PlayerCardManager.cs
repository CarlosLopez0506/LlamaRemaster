using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerCardManager : MonoBehaviour
{
    public static PlayerCardManager Instance; // Singleton instance

    public GameObject cardPrefab; // Prefab para la carta del jugador
    public List<Transform> playerCards = new List<Transform>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(
                gameObject); // Optional: Prevents the object from being destroyed when loading a new scene
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
        playerCards.Clear();

        // Create player cards dynamically
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, cardsParent);
            playerCards.Add(cardObject.transform);

            // Manually adjust the position of each card
            cardObject.transform.localPosition = new Vector3(-650 + i * 400, 363, 0);
        }
    }



    public void UpdateCardData(List<PlayerData> allPlayers)
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            PlayerData player = allPlayers[i];
            Transform playerCard = playerCards[i];

            // Actualiza la interfaz de la carta del jugador
            UpdateCardUI(playerCard, player.position, player.playerName, player.cizanaPoints);
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
