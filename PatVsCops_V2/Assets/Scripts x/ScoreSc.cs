using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSc : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreTxt;
    public bool running = true;


    private void Start()
    {
        score = 0;
        scoreTxt.text = score.ToString();
        running = true;
    }

    public void startCounting()
    {
        StartCoroutine(GetPointsOverSeconds());
    }
    public void StopCounting()
    {
        StopCoroutine(GetPointsOverSeconds());
    }

    IEnumerator GetPointsOverSeconds()
    {
        while (running)
        {
            score++;
            scoreTxt.text = score.ToString();
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    public void AddPoint(int point,int combo)
    {
        if (combo!=0)
        {
            StartCoroutine(IncreaseScore(point * combo));
        }
        else
        {
            StartCoroutine(IncreaseScore(point));
        }
        
    }

    IEnumerator IncreaseScore(int addition)
    {
        for (int i = 1; i <= addition; i++)
        {
            score++;
            scoreTxt.text = score.ToString();
            yield return new WaitForEndOfFrame();
        }
    }
}
