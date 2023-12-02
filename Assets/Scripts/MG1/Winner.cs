using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Winner : MonoBehaviour
{
    public TMP_Text textoGanador;

    void Start()
    {
        // Recupera el ganador de PlayerPrefs
        string ganador = PlayerPrefs.GetString("Ganador", "No hay ganador");

        // Muestra el ganador en el TMP_Text
        textoGanador.text = "El ganador es: " + ganador;
    }
}
