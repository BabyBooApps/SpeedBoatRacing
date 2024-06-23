using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static void SavePlayerData(Data playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerData.saved";
        FileStream stream = new FileStream(path, FileMode.Create);


        formatter.Serialize(stream, playerData);
        stream.Close();
    }
    public static Data LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/PlayerData.saved";
        try
        {
            if (!File.Exists(path))
            {
                Debug.Log("doesn't exist PlayerData");
                //saving
                SaveNewPlayerData();
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();
            return data;
        }
        catch (System.Exception)
        {
            //File.Delete(path);
            return new Data();
        }





    }
    static void SaveNewPlayerData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerData.saved";
        FileStream stream = new FileStream(path, FileMode.Create);

        Data data = new Data();

        formatter.Serialize(stream, data);
        stream.Close();


    }


    /// <summary>
    /// CAR
    /// </summary>
    /// <param name="Cardata"></param>

    public static void SaveCarData(_CarData carData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/"+carData.carName+".saved";
        FileStream stream = new FileStream(path, FileMode.Create);


        formatter.Serialize(stream, carData);
        stream.Close();
    }
    public static _CarData LoadCarData(GameObject car, bool unlocked)
    {
        string path = Application.persistentDataPath + "/" + car.name + ".saved";
        try
        {
            if (!File.Exists(path))
            {
                //Debug.Log("doesn't exist CarData");
                //saving
                SaveNewCarData( car,  unlocked);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            _CarData data = formatter.Deserialize(stream) as _CarData;
            stream.Close();
            return data;
        }
        catch (System.Exception)
        {
            //File.Delete(path);
            return new _CarData(car,unlocked);
        }
    }
    static void SaveNewCarData(GameObject car,bool unlocked)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + car.name + ".saved";
        FileStream stream = new FileStream(path, FileMode.Create);

        _CarData data = new _CarData(car,unlocked);

        formatter.Serialize(stream, data);
        stream.Close();


    }
}
