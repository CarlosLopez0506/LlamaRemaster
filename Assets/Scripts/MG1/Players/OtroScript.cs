using UnityEngine;

public class OtroScript : MonoBehaviour
{
    public EscalaZScript scriptEscalaZ;

    public void Crecer()
    {
        float escalaZObjetoActual = transform.localScale.z;
        float escalaZ = scriptEscalaZ.escalaZ;


        escalaZObjetoActual += escalaZ;

        Vector3 nuevaEscala = transform.localScale;
        nuevaEscala.z = escalaZObjetoActual;
        transform.localScale = nuevaEscala;
    }
}