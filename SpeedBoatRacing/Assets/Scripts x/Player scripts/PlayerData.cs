using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject body;
    
    [HideInInspector]
    public Data data;

    private void Start()
    {
        //PlayerPrefs.SetInt("Diamonds", 1000);
        data = SaveSystem.LoadPlayerData();

        SetUpCar();
    }

    public void SetUpCar()
    {
        foreach (CarData item in body.GetComponentsInChildren<CarData>(true))
        {
            item.carData = SaveSystem.LoadCarData(item.gameObject, item.unlocked);
            item.UpdateCarItem();

            if (item.gameObject.name == data.currentCarName)
            {
                item.gameObject.SetActive(true);
                GetComponent<PlayerBehavior>().selectedCar = item.gameObject;
                GetComponent<PlayerBehavior>().UpdateCarPieces();
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public void ChangeCar(string carName)
    {
        data.currentCarName = carName;
        SaveSystem.SavePlayerData(data);
        SetUpCar();
    }

    public bool IsLocked(string carName)
    {
        bool islocked = false;
        foreach (CarData item in body.GetComponentsInChildren<CarData>(true))
        {
            if (item.gameObject.name.Equals(carName))
            {
                if (item.carData.unlocked)
                {
                    islocked = false;
                }
                else
                {
                    islocked = true;
                }
                
            }
        }
        return islocked;
    }

    public CarData GetCarData(string carName)
    {
        foreach (CarData item in body.GetComponentsInChildren<CarData>(true))
        {
            if (item.gameObject.name == data.currentCarName)
            {
                return item;
            }
        }
        return null;
    }

}

[System.Serializable]
public class Data
{
    public string currentCarName= "Car01Model";
    
    public Data()
    {
        currentCarName = "Car01Model";
    }
}