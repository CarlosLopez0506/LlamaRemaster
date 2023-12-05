using UnityEngine;

namespace Manager_Scripts
{
    public class ButtonEventsHandler: MonoBehaviour
    {
        public void ChangeButtonColor(GameObject button)
        {
            Renderer renderer = button.GetComponent<Renderer>();
            if (renderer != null)
            {
                float attenuationFactor = 0.8f;
                attenuationFactor = Mathf.Clamp(attenuationFactor, 0f, 1f);

                var material = renderer.material;
                Color currentColor = material.color;

                Color darkenedColor = new Color(currentColor.r * attenuationFactor,
                    currentColor.g * attenuationFactor,
                    currentColor.b * attenuationFactor,
                    currentColor.a);

                material.color = darkenedColor;
            }
        }

        public void ResetButtonColor(GameObject button, Color color)
        {
            Renderer renderer = button.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Reset to the original color or your desired color
                renderer.material.color = color;
            }
        }
    }
}