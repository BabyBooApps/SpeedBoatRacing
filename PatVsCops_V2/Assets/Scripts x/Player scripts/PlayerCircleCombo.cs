using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerCircleCombo : MonoBehaviour
{
    public GameObject circleDetection;
    List<GameObject> colls = new List<GameObject>();
    public Image comboFill;

    float timeToFill = 1.5f;

    public int combo=0;

    PlayerBehavior playerBehavior;

    public bool OneTwo=true;
    public Animator scorePopup;

    public GameObject diamond;

    private void Awake()
    {
        playerBehavior = GetComponentInParent<PlayerBehavior>();
    }



    private void OnTriggerEnter(Collider other)
    {
       /* if (other.CompareTag("CopBroken"))
        {
            colls.Add(other.gameObject);
            if (colls.Count == 1)
            {
                //circleDetection.SetActive(true);
                DisplayCircle();
                StartCoroutine(ComboTime());
            }
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (other.CompareTag("CopBroken"))
        {
            colls.Remove(other.gameObject);
            if (colls.Count<=0)
            {

                HideCircle();
                //circleDetection.SetActive(false);
                StartCoroutine(ComboTimeInversed());
            }
        }*/
    }


    IEnumerator ComboTime()
    {
        yield return new WaitForSeconds(0.2f);

        //comboFill.fillAmount = 0.0f;
        
        while (colls.Count>0)
        {
            if (comboFill.fillAmount<1)
            {
                comboFill.fillAmount += Time.deltaTime / timeToFill;
                yield return new WaitForEndOfFrame();
                continue;
            }
            if (combo == 0)
            {
                UpdateCombo(1);
                playerBehavior.playComboSound();
            }
            yield return new WaitForEndOfFrame();
        }
        //comboFill.fillAmount = 0.0f;
    }

    IEnumerator ComboTimeInversed()
    {
        //yield return new WaitForSeconds(0.2f);
        //float startTime = Time.time;

        //comboFill.fillAmount = 0.0f;
        while (colls.Count <= 0)
        {
            if (comboFill.fillAmount > 0)
            {
                comboFill.fillAmount -= Time.deltaTime/ (timeToFill*2.0f);
                yield return new WaitForEndOfFrame();
                continue;
            }
            if (combo > 0)
            {
                UpdateCombo(0);
            }
            yield return new WaitForEndOfFrame();
        }
        //comboFill.fillAmount = 0.0f;
    }


    void DisplayCircle()
    {
        circleDetection.GetComponent<Image>().DOFade(.4f, 0.35f);
    }
    void HideCircle()
    {
        circleDetection.GetComponent<Image>().DOFade(0, 0.35f);
    }

    public void UpdateCombo(int combo)
    {
        this.combo = combo;
        playerBehavior.canvas.GetComponent<UIOperations>().SetCombo(combo);
    }

    //called when cop destroyed
    public void IncreaseCombo(Vector3 hitPoint)
    {
        
        if (combo>0)
        {
            //OneTwo = !OneTwo;
            if (OneTwo)
            {
                combo++;
                playerBehavior.canvas.GetComponent<UIOperations>().SetCombo(combo);
                playerBehavior.playComboSound();
            }
        }
        if (OneTwo)
        {
            playerBehavior.canvas.GetComponent<ScoreSc>().AddPoint(6, combo);
            if (combo != 0)
            {
                scorePopup.transform.Find("txt").GetComponent<TextMeshProUGUI>().text = (6 * combo).ToString();
            }
            else
            {
                scorePopup.transform.Find("txt").GetComponent<TextMeshProUGUI>().text = (6).ToString();
            }
            scorePopup.SetTrigger("Popup");
            SpawnDiamond(hitPoint);
        }
        OneTwo = !OneTwo;
    }

    public void SpawnDiamond(Vector3 spawnPos)
    {
        Instantiate(diamond, new Vector3(spawnPos.x,0,spawnPos.z), Quaternion.identity);
    }
}
