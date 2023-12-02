using UnityEngine;

public class EscalaZScript : MonoBehaviour
{
    // Variable para almacenar la escala en el eje Z
    public float escalaZ;

    


    void Update()
    {

        
        // Guardar la escala en el eje Z del objeto
        escalaZ = transform.localScale.y;

        // Puedes imprimir la escala en la consola para verificar
        
    }
}
