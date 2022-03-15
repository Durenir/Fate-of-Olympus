using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void Save<T>(T objectToSave, string key)
    {
        string path;
        if(GameManager.instance.useMainSave)
        {
            path = Application.persistentDataPath + "/saves/";             //Player is loading their save game
        } else {
            path = Application.persistentDataPath + "/temp/";              //Load the data from the previous scene
        }
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path + key + ".lul", FileMode.Create))
        {
            formatter.Serialize(stream, objectToSave);
        }
    }

    public static T Load<T>(string key)
    {
        string path;
        if(GameManager.instance.useMainSave)
        {
            Debug.Log("Loading " + key + " from saves.");
            path = Application.persistentDataPath + "/saves/";             //Player is loading their save game
            GameManager.instance.useMainSave = false;
        } else {
            Debug.Log("Loading " + key + " from temp.");
            path = Application.persistentDataPath + "/temp/";              //Load the data from the previous scene
        }
        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default(T);
        using (FileStream stream = new FileStream(path + key + ".lul", FileMode.Open))
        {
            returnValue = (T)formatter.Deserialize(stream);
        }
        return returnValue;
    }

    public static bool SaveExists(string key)
    {
        string path = Application.persistentDataPath + "/saves/" + key + ".lul";
        return File.Exists(path);
    }

    public static bool TempExists(string key)
    {
        string path = Application.persistentDataPath + "/temp/" + key + ".lul";
        return File.Exists(path);
    }

    public static void SeriouslyDeleteAllSaveFiles()
    {
        string path = Application.persistentDataPath + "/saves/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
        path = Application.persistentDataPath + "/temp/";
        directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
    }

    public static void ClearTempFiles()
    {        
        string path = Application.persistentDataPath + "/temp/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
    }
}
