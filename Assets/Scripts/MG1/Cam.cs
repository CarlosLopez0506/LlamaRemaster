using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float velocidad = 0.5f; // Nueva variable para la velocidad de bajada

    void Update()
    {
        // Bajar la cámara junto con el cubo.
        transform.position -= new Vector3(0f, velocidad * Time.deltaTime, 0f);
    }
}

