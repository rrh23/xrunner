using System.IO;
using UnityEngine;

[System.Serializable]
public class Data
{
    public float highscore;
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
    }

    public static string Load(string fileName)
    {
        string file = SAVE_FOLDER + fileName + FILE_EXT;
        if (File.Exists(file))
        {
            string loadedData = File.ReadAllText(file);

            return loadedData;
        }
        else
        {
            return null;
        }
    }
}