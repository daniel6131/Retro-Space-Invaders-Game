using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad
{
    private static string filename = "SaveData.txt";
    private static string directoryName = "SaveData";

    // Method to save a saveobject onto the system
    public static void SaveState(SaveObject so)
    {
        // If the save directory does not exist create a new one
        if (!DirectoryExists())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + directoryName);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetSavePath());
        bf.Serialize(file, so);
        file.Close();
    }

    // Method to load saveobject from storage
    public static SaveObject LoadState()
    {
        SaveObject so = new SaveObject();

        if(SaveExists())
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(GetSavePath(), FileMode.Open);
                so = (SaveObject)bf.Deserialize(file);
                file.Close();
            }
            catch (SerializationException)
            {
                Debug.LogWarning("Failed to load save");
            }

        }

        return so;
    }

    // Check to see if a save object exists under the expected save path
    private static bool SaveExists()
    {
        return File.Exists(GetSavePath());
    }

    // Check to see if a save directory exists under the expected directory path
    private static bool DirectoryExists()
    {
        return Directory.Exists(Application.persistentDataPath + "/" + directoryName);
    }

    // Return the save path for a save object
    private static string GetSavePath()
    {
        return Application.persistentDataPath + "/" + directoryName + "/" + filename;
    }
}
