using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void Jugar(string opt) {
        SceneManager.LoadScene("CharacterSelection");
    }
}
