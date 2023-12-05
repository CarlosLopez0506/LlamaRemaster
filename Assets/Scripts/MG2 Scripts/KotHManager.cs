using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KotHManager : MonoBehaviour
{
    [SerializeField] private float gameDuration;
    public UnityEvent gameOverEvent;
    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= gameDuration)
        {
            gameOverEvent.Invoke();
        }
    }
}
