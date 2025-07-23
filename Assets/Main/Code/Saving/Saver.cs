using System;
using System.IO;
using UnityEngine;

public class Saver : MonoBehaviour
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    private static readonly string SAVE_FILE = "save.json";

    public static void Initialize()
    {
        if (Directory.Exists(SAVE_FOLDER) == false)
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(SaveFile saveFile)
    {
        try
        {
            string json = JsonUtility.ToJson(saveFile);
            File.WriteAllText(SAVE_FOLDER + SAVE_FILE, json);
            Debug.Log("Game is saved in: " + SAVE_FOLDER + SAVE_FILE);
        }
        catch (Exception e)
        {
            Debug.LogError("Saving error: " + e.Message);
        }
    }

    public static SaveFile Load()
    {
        if (File.Exists(SAVE_FOLDER + SAVE_FILE))
        {
            try
            {
                string json = File.ReadAllText(SAVE_FOLDER + SAVE_FILE);
                SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);

                return saveFile;
            }
            catch (Exception e)
            {
                Debug.LogError("Download error: " + e.Message);

                return null;
            }
        }
        else
        {
            Debug.Log("Save file was not found. Creating a new.");

            return new SaveFile();
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(SAVE_FOLDER + SAVE_FILE))
        {
            File.Delete(SAVE_FOLDER + SAVE_FILE);
            Debug.Log("Save was deleted");
        }
        else
        {
            Debug.Log("No save found");
        }
    }
}
