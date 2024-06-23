using UnityEngine;
using System;
using System.IO;

[Serializable]
public class NestedData
{
    public int[][] nestedArray;
}

public class SerializationExample : MonoBehaviour
{
    private void Start()
    {
        // Example data
        int[][] myNestedArray = new int[][]
        {
            new int[] { 1, 2, 3 },
            new int[] { 4, 5, 6 },
            new int[] { 7, 8, 9 }
        };

        // Create an instance of NestedData and assign the nested array
        NestedData nestedData = new NestedData
        {
            nestedArray = myNestedArray
        };

        // Convert the NestedData object to JSON format
        string json = JsonUtility.ToJson(nestedData);

        // Specify the file path
        string filePath = Application.persistentDataPath + "/nestedData.txt";

        // Write the JSON data to a text file
        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to: " + filePath);
    }
}