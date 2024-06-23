using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject[] hearts;
    public Animator loseHeart;

    public void UpdateHearts(int heartLeft)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i+1 <= heartLeft)
            {
                //hearts[i].transform.Find("alive").gameObject.SetActive(true);
                hearts[i].SetActive(true);
            }
            else
            {
                //hearts[i].transform.Find("alive").gameObject.SetActive(false);
                if (hearts[i].activeSelf)
                {
                    hearts[i].SetActive(false);
                    loseHeart.SetTrigger("Broke");
                } 
            }
        }
    }
}
