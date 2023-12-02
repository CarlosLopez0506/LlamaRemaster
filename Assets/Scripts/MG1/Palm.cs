using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palm : MonoBehaviour
{
    public float velocidadEstiramiento = 2.0f;

    void Update()
    {
        // Verificar si una tecla específica está siendo presionada (por ejemplo, la tecla 'E').
        if (Input.GetKey(KeyCode.E))
        {
            // Invocar la función de estiramiento con un retraso de 3 segundos.
            Invoke("Estirar", 5f);
        }
    }

    void Estirar()
    {
        // Obtener la escala actual del cubo.
        Vector3 escalaActual = transform.localScale;

        // Incrementar la escala en el eje Y.
        escalaActual.y += velocidadEstiramiento * Time.deltaTime;

        // Aplicar la nueva escala al cubo.
        transform.localScale = escalaActual;
    }
}

