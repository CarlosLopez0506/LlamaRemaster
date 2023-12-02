using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private GameObject menu; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SetUpMenu");
    }

    IEnumerator SetUpMenu(){
        yield return new WaitForSeconds(1);
        startButton.Select();
    }

}
