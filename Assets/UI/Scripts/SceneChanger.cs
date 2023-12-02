using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneChanger : MonoBehaviour
{
    // You can set the scene name in the Unity Editor or through code
    public string sceneToLoad = "CharacterSelection";
    public Button startButton; // Reference to your start button

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        {
            // Check if the start button is currently selected
            if (EventSystem.current.currentSelectedGameObject == startButton.gameObject)
            {
                // Both conditions are met, perform your scene change here
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
