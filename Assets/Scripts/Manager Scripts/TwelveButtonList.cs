using System.Collections.Generic;
using UnityEngine;


public class TwelveButtonList : MonoBehaviour
{
    public static TwelveButtonList Instance;

    public List<TwelveButton> twelveButtonsArray;
    [SerializeField] private TwelveButtonManager twelveButtonManager;

    private void Awake()
    {
        InitializeTwelveButtons(twelveButtonManager.BuscarYOrdenarBotonesPorNombre());

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

    void Start()
    {
        InitializeTwelveButtons(twelveButtonManager.BuscarYOrdenarBotonesPorNombre());
        Debug.Log("papu");
    }

    private void InitializeTwelveButtons(GameObject[] buttons)
    {
        twelveButtonsArray = new List<TwelveButton>();

        for (int i = 0; i < buttons.Length; i++)
        {
            TwelveButton twelveButton = new TwelveButton(i + 1, buttons[i], "none",
                buttons[i].GetComponent<Renderer>().material.color);

            twelveButtonsArray.Add(twelveButton);
        }
    }

    public void FindButtons()
    {
        GameObject[] orderedButtons = twelveButtonManager.BuscarYOrdenarBotonesPorNombre();

        for (int i = 0; i < twelveButtonsArray.Count; i++)
        {
            twelveButtonsArray[i].ButtonPrefab = orderedButtons[i];
        }
    }


    public TwelveButton GetTwelveButton(int buttonNumber)
    {
        if (buttonNumber >= 0 && buttonNumber <= twelveButtonsArray.Count)
        {
            return twelveButtonsArray[buttonNumber];
        }
        else
        {
            Debug.LogWarning("Invalid button number: " + buttonNumber);
            return null;
        }
    }
}