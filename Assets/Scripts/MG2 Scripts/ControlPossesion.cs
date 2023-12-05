using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPossesion : MonoBehaviour
{
    [SerializeField] private RectTransform controlTransform;

    public Transform followTransform;
    void FixedUpdate()
    {
        controlTransform.position = followTransform.position;
    }
}
