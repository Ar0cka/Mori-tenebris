using System;
using System.IO;
using DefaultNamespace.PlayerStatsOperation.SaveSystem;
using UnityEngine;


public class SaveAndLoadSystem : ISaveAndLoad
{
    public void SaveData(object saveObject, string filePath)
    {
        if (saveObject == null) return;
        try
        {
            string data = JsonUtility.ToJson(saveObject);
            if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(filePath)
                                            && filePath.EndsWith(".json"))
            {
                File.WriteAllText(filePath, data);
            }
            else
            {
                Debug.LogError("SaveAndLoadSystem SaveData Error");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public T LoadData<T>(string filePath)
    {
        try
        {
            string data = File.ReadAllText(filePath);
            T result = JsonUtility.FromJson<T>(data);

            if (result != null)
            {
                return result;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return default;
    }
}