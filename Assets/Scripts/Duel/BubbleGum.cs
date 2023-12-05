using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGum : MonoBehaviour
{
    [SerializeField] private LlamaPoints llamaPoints;
    private float _scale;

    public void UpdateScale()
    {
        _scale = Mathf.Max(0.001f, llamaPoints.GetPoints() / 1000);
        transform.localScale = _scale * Vector3.one;
    }
}
