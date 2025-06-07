using System.IO;
using UnityEngine;

[System.Serializable]
public class Data
{
    public float highscore;
    public float BGMvolume;
    public float SFXvolume;
}

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/saves/";
    public static readonly string FILE_EXT = ".json";

    public static void Save(string fileName, string dataToSave)
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        File.WriteAllText(SAVE_FOLDER + fileName + FILE_EXT, dataToSave);
        Debug.Log("saved data! filename:" + fileName + "data:" + dataToSave);
    }

    public static string Load(string fileName)
    {
        string file = SAVE_FOLDER + fileName + FILE_EXT;
        if (File.Exists(file))
        {
            string loadedData = File.ReadAllText(file);
            Debug.Log("loaded data! data: " + loadedData);
            return loadedData;
        }
        else
        {
            Debug.Log("null :(");
            return null;
        }
    }
}