using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hill : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color meiColor;
    [SerializeField] private Color maroonColor;
    [SerializeField] private Color manyColor;
    [SerializeField] private Color moeColor;
    [SerializeField] private Material _material;

    private float _timer;
    [SerializeField] private float addPointsRate;
    private List<LlamaPoints> _llamas = new();

    private void Start()
    {
        ChangeColor();
    }

    private void Update()
    {
        if (_llamas.Count == 1)
        {
            _timer += Time.deltaTime;
            if (_timer >= addPointsRate)
            {
                _llamas[0].AddPoints(1);
                _timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _llamas.Add(other.gameObject.GetComponent<LlamaPoints>());
        }
        ChangeColor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _llamas.Remove(other.gameObject.GetComponent<LlamaPoints>());
        }
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (_llamas.Count == 1)
        {
            var llamaName = _llamas[0].gameObject.name;
            switch (llamaName)
            {
                case "Mei(Clone)":
                    _material.color = meiColor;
                    break;
                case "Maroon(Clone)":
                    _material.color = maroonColor;
                    break;
                case "Many(Clone)":
                    _material.color = manyColor;
                    break;
                case "Moe(Clone)":
                    _material.color = moeColor;
                    break;
            }
        }
        else
        {
            _material.color = defaultColor;
        }
    }
}
