using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerTextUpdater : MonoBehaviour
{
    public TextMeshProUGUI[] textComponents;
    public float delayBeforeSceneChange = 2f;

    private int controllersPressedA = 0;

    void Update()
    {
        for (int i = 0; i < Mathf.Min(Gamepad.all.Count, textComponents.Length); i++)
        {
            Gamepad gamepad = Gamepad.all[i];

            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                textComponents[i].text = $"Controller {i + 1} joined";
                controllersPressedA++;

                // Check if all controllers have pressed the "A" button
                if (controllersPressedA == Mathf.Min(Gamepad.all.Count, textComponents.Length))
                {
                    // Schedule scene change after a delay
                    Invoke("ChangeScene", delayBeforeSceneChange);
                }
            }
        }
    }

    void ChangeScene()
    {
        // Change scene here
        SceneManager.LoadScene("Board");
    }
}
