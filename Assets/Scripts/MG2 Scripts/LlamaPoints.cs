using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LlamaPoints : MonoBehaviour
{
    private int _points;
    [SerializeField] private TMP_Text pointsText;

    public void AddPoints(int points)
    {
        _points += points;
        pointsText.text = _points.ToString();
    }

    public float GetPoints()
    {
        return _points;
    }
}
