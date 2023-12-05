using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnLoad : MonoBehaviour
{
    private void Awake()
    {
        for (int i = 0; i < GameObject.FindObjectsOfType<KeepOnLoad>().Length; i++)
        {
            if (GameObject.FindObjectsOfType<KeepOnLoad>()[i] != this)
            {
                if (GameObject.FindObjectsOfType<KeepOnLoad>()[i].name == name)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}