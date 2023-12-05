using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ONLY FOR TESTING THIS SCENE STAND-ALONE
//Otherwise, call Initialize() from BDManager
public class BDCallInitialize : MonoBehaviour
{
    public string llama1;
    public string llama2;
    private BDManager _bdManager;
    // Start is called before the first frame update
    void Start()
    {
        _bdManager = GetComponent<BDManager>();
        _bdManager.Initialize(llama1, llama2);
    }
}
