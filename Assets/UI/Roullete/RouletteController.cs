using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RouletteController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rouletteCanvas;
    public Animator rouletteItems;
    public Animator animator;

    public GameObject RI;

    public string[] Scenes; 

    void Start()
    {
        
    }

    public void StartRoulette(int idx, string scene)
    {
        if(rouletteCanvas.active) return;
        rouletteCanvas.SetActive(true);
        StartCoroutine(StopRoulette(idx, scene));
        Debug.Log("Start Stoped");

        return;
    }

    public IEnumerator StopRoulette(int index, string scene){
        Debug.Log("Start Stoped");
        if(!rouletteCanvas.active) yield return null; 
        yield return new WaitForSeconds(3);

        Debug.Log("stoped");

        animator.speed = 0;
        RI.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (1080 *index));
        yield return new WaitForSeconds(3);
        StartCoroutine(LoadMinigameScene(scene));

    }

    public IEnumerator LoadMinigameScene(string sceneName)
    {
        yield return null;
        SceneManager.LoadScene(sceneName);
    }

}