using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIOperations : MonoBehaviour
{
    public GameObject startScreen, playingScreen,gameOverScreen;

    GameObject player;

    public GameObject comboScreen;
    public TextMeshProUGUI comboNumber;

    public Image squareVignette;

    public TextMeshProUGUI finishScore;

    public TextMeshProUGUI diamondTxt;

    int diamondThisParty = 0;

    public TextMeshProUGUI highscoreTxtStart;
    public TextMeshProUGUI highscoreTxtEnd;

    bool customScrollDisplaying = false;
    public RectTransform customScroll;

    Vector3 camPos = new Vector3(21, 35, -25);
    Vector3 camRot = new Vector3(47, -40, 0);

    Vector3 camShopPos = new Vector3(12.5f, 6, -15);
    Vector3 camShopRot = new Vector3(17, -40, 0);

    public string runSceneName;

    private void Awake()
    {
        //Debug.LogError(Application.persistentDataPath);
        //PlayerPrefs.DeleteAll();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        diamondTxt.text = PlayerPrefs.GetInt("Diamonds").ToString();
        highscoreTxtStart.text = PlayerPrefs.GetInt("HighScore").ToString();

        //StartButton();
    }
    public void StartButton()
    {
        startScreen.SetActive(false);
        playingScreen.SetActive(true);
        player.GetComponent<PlayerBehavior>().StartPlaying();
        GetComponent<ScoreSc>().startCounting();
    }

    public void ActiveGameOverSceen()
    {
        playingScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        finishScore.text = GetComponent<ScoreSc>().score.ToString();
        if (GetComponent<ScoreSc>().score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", GetComponent<ScoreSc>().score);
        }
        
        highscoreTxtEnd.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
    public void ReplayButton()
    {
        // ADManager.Display_InterstitialAd();
        StartCoroutine(On_Replay_Button_Click());
       
    }

    IEnumerator On_Replay_Button_Click()
    {
        AdsManager.Instance.interstitial.ShowAd();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(runSceneName);
    }

    public void Watch_Button()
    {
        AdsManager.Instance.RewardAd.ShowAd();
    }
    public void SetCombo(int combo)
    {
        if (combo == 1)
        {
            comboScreen.SetActive(true);
        }
        if (combo == 0)
        {
            comboScreen.SetActive(false);
        }
        
        comboNumber.text = "X" +combo.ToString();


        comboNumber.transform.DOComplete();
        comboNumber.transform.DOPunchScale(new Vector3(2, 2, 0), 0.2f);
        squareVignette.DOComplete();
        squareVignette.DOColor( Random.ColorHSV(), 0.5f);
    }

    public void CollectDiamond()
    {
        PlayerPrefs.SetInt("Diamonds", PlayerPrefs.GetInt("Diamonds") + 1);
        diamondThisParty++;
        diamondTxt.text = PlayerPrefs.GetInt("Diamonds").ToString();
        diamondTxt.transform.DOComplete();
        diamondTxt.transform.DOPunchScale(new Vector3(2, 2, 0), 0.2f);
    }

    public void Add25Diamond()
    {
        PlayerPrefs.SetInt("Diamonds", PlayerPrefs.GetInt("Diamonds") + 25);
        diamondTxt.text = PlayerPrefs.GetInt("Diamonds").ToString();
        diamondTxt.transform.DOComplete();
        diamondTxt.transform.DOPunchScale(new Vector3(2, 4, 0), 0.75f);
    }

    public void WatchAdButton()
    {
        //ADManager.Display_RewardVideoAd();
        Add25Diamond();
        Invoke("LoadScene", 0.5f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(runSceneName);
    }

    public void CustomButton()
    {
        customScrollDisplaying = !customScrollDisplaying;
        if (customScrollDisplaying)
        {
            customScroll.DOAnchorPosX(1.15f, 0.3f);
        }
        else
        {
            customScroll.DOAnchorPosX(-500, 0.3f);
        }
    }


    public void SetCustomColor(Image img)
    {
        player.GetComponent<PlayerBehavior>().SetMainColor(img.color);
    }

    public void SetCar(GameObject car)
    {
        if (player.GetComponent<PlayerData>().IsLocked(car.name))
        {
            if (PlayerPrefs.GetInt("Diamonds") >= car.GetComponent<CarData>().value)
            {
                player.GetComponent<PlayerData>().ChangeCar(car.name);
                UseDiamonds(car.GetComponent<CarData>().value);
                Unlock(player.GetComponent<PlayerData>().GetCarData(car.name));

            }
            else
            {
                diamondTxt.transform.DOComplete();
                diamondTxt.transform.DOPunchScale(new Vector3(-0.6f, -0.6f, 0), 0.5f);
            }
        }
        else
        {
            player.GetComponent<PlayerData>().ChangeCar(car.name);
        }

        
    }

    public void UseDiamonds(int val)
    {
        PlayerPrefs.SetInt("Diamonds", PlayerPrefs.GetInt("Diamonds") - val);
        diamondTxt.text = PlayerPrefs.GetInt("Diamonds").ToString();
        diamondTxt.transform.DOComplete();
        diamondTxt.transform.DOPunchScale(new Vector3(2, 2, 0), 0.5f);
    }

    public void Unlock(CarData btnCardData)
    {
        btnCardData.carData.unlocked = true;
        SaveSystem.SaveCarData(btnCardData.carData);
        btnCardData.UpdateCarItem();
    }
}
