using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarData : MonoBehaviour
{
    public GameObject button;
    [HideInInspector]
    public string carName;
    public bool unlocked = false;

    public int value;

    //[HideInInspector]
    public _CarData carData;

    /*private void Awake()
    {
        carData = SaveSystem.LoadCarData(gameObject, unlocked);

        UpdateCarItem();
    }*/

    private void Start()
    {
        
    }

    public void UpdateCarItem()
    {
        carName = carData.carName;
        unlocked = carData.unlocked;
        if (unlocked)
        {
            button.transform.Find("cost").gameObject.SetActive(false);
        }
    }


}

[System.Serializable]
public class _CarData
{
    public string carName;
    public bool unlocked;

    public _CarData(GameObject carGo,bool unlocked)
    {
        carName = carGo.name;
        this.unlocked = unlocked;
    }
}
