using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class DataUtils 
{

    public static void SaveData(string dataName, string json)
    {
        Master.SaveToDisk(json, GetDataPath(dataName));
    }

    public static T LoadData<T>(string dataName) where T : class, new()
    {
        if (!File.Exists(GetDataPath(dataName)))
        {
            var saveData = JsonConvert.SerializeObject(new T());
            SaveData(dataName, saveData);
        }
        string data = Master.LoadFromDisk(GetDataPath(dataName));
        var returnData = Master.DeserializeObject<T>(data);

        return returnData;
    }

    public static void ClearData(string dataName, string json) 
    {
        SaveData(dataName, json);
    }

    static string GetDataPath(string dataName)
    {
        string path = Application.persistentDataPath + Path.DirectorySeparatorChar + dataName;
        return path;
    }
}