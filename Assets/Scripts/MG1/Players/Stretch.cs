using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Stretch : MonoBehaviour
{
    public string llamaName;
    public OtroScript scriptOtro;
    public float velocidadEstiramiento = 5.0f;
    public TMP_Text dist;
    private int aumentoPorPulsacion = 1;
    private int contador = 0;

    void Start()
    {
        // Inicializa el texto del contador.
        ActualizarTextoContador();
    }
    private Gamepad gamepad;

    public void Initialize(Gamepad gamepad, string playerName)
    {
        this.gamepad = gamepad;
        this.llamaName = playerName;
    }
    void Update()
    {
        // Check for controller input using the Input System.
        if (gamepad.bButton.wasPressedThisFrame)
        {
            HandleStretch();
        }
        // Check for keyboard input (for testing purposes).
        else if (Input.GetKeyDown(KeyCode.E))
        {
            HandleStretch();
        }
    }

    void HandleStretch()
    {
        scriptOtro.Crecer();

        // Obtener la escala actual del cubo.
        Vector3 escalaActual = transform.localScale;

        // Incrementar la escala en el eje Y multiplicando por un factor.
        escalaActual.z += velocidadEstiramiento * Time.deltaTime;

        // Aplicar la nueva escala al cubo.
        transform.localScale = escalaActual;

        contador += aumentoPorPulsacion;

        ActualizarTextoContador();
    }

    void ActualizarTextoContador()
    {
        // Actualiza el texto del contador en el objeto TMP_Text.
        dist.text = llamaName +": " + contador.ToString() + "cm";
    }
}
