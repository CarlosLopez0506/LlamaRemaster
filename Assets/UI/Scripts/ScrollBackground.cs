using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBackground : MonoBehaviour
{

    [SerializeField] private RawImage bgImg;
    [SerializeField] private float vy, vx;

    // Update is called once per frame
    void Update()
    {
        bgImg.uvRect = new Rect(bgImg.uvRect.position + new Vector2(vx, vy) * Time.deltaTime, bgImg.uvRect.size);
    }
}
