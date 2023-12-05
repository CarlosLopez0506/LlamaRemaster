using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text time;
    public List<TMP_Text> scoreTexts; 
    public AudioSource audioSource;

    private float _left;
    private bool _flag;

    void Start()
    {
        _left = 2;
        _flag = true;
    }

    void Update()
    {
        if (_flag)
        {
            _left -= Time.deltaTime;
            if (_left <= 0)
            {
                _left = 0;
                _flag = false;
                audioSource.Stop();
                MinigameManager.Instance.GameOver(GetScoresFromTexts(scoreTexts)); // Pass scores using ObtenerNumero
            }

            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        time.text = Mathf.CeilToInt(_left).ToString();
    }

    List<float> GetScoresFromTexts(List<TMP_Text> texts)
    {
        List<float> scores = new List<float>();
        Debug.Log(scores.Count);

        foreach (TMP_Text text in texts)
        {
            float score = ObtenerNumero(text);
            scores.Add(score);
        }

        return scores;
    }

    float ObtenerNumero(TMP_Text texto)
    {
        float numero;
        if (float.TryParse(texto.text, out numero))
        {
            return numero;
        }
        else
        {
            string textoLimpio = new string(texto.text.Where(char.IsDigit).ToArray());
            if (float.TryParse(textoLimpio, out numero))
            {
                return numero;
            }
            else
            {
                Debug.LogError("El contenido del texto no es un número válido.");
                return float.MinValue;
            }
        }
    }
}