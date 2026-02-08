using System;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string baseSavePath = Path.Combine(Application.persistentDataPath, "Saves");
    
    public static void SavePlayer(MoneyUI money)
    {
        string path = Path.Combine(baseSavePath, "player.smoop");

        try
        {
            SaveMoney newMoney = new SaveMoney(money);

            string jsonSaveData = JsonUtility.ToJson(newMoney, true);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonSaveData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not save player data!");
        }
    }

    public static SaveMoney LoadPlayer()
    {
        string path = Path.Combine(baseSavePath, "player.smoop");

        if (File.Exists(path))
        {
            try
            {
                string loadedJsonData = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        loadedJsonData = reader.ReadToEnd();
                    }
                }

                SaveMoney loadedMoney = JsonUtility.FromJson<SaveMoney>(loadedJsonData);
                return loadedMoney;
            }
            catch (Exception e)
            {
                Debug.LogError("Could not load player data!");
                return null;
            }
        }
        else
        {
            return null;
        }  
    }

    public static void SaveBests(string name, Tracker tracker)
    {
        string path = Path.Combine(baseSavePath, "LevelBests", $"{name}.smoop");

        try
        {
            SaveBests bests = new SaveBests(tracker);

            string jsonSaveData = JsonUtility.ToJson(bests, true);
            
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonSaveData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save level best!");
        }
    }

    public static SaveBests LoadBests(string name)
    {
        string path = Path.Combine(baseSavePath, "LevelBests", $"{name}.smoop");

        if (File.Exists(path))
        {
            try
            {
                string loadedJsonData = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        loadedJsonData = reader.ReadToEnd();
                    }
                }

                SaveBests loadedBest = JsonUtility.FromJson<SaveBests>(loadedJsonData);
                return loadedBest;
            }
            catch (Exception e)
            {
                Debug.LogError("Could not load level best!");
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    #region Separate Saves
    /*public static void SaveTime (string name, Tracker tracker)
    {
        string folderPath = Application.persistentDataPath + $"/LevelBests/{name}";

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/LevelBests/{name}/time.smoop";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveTime newBests = new SaveTime(tracker);

        formatter.Serialize(stream, newBests);
        stream.Close();
    }

    public static SaveTime LoadTime(string name)
    {
        string path = Application.persistentDataPath + $"/LevelBests/{name}/time.smoop";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            SaveTime time = formatter.Deserialize(stream) as SaveTime;
            stream.Close();

            return time;
        }
        else
        {
            return null;
        }


    }

    public static void SaveKills(string name, Tracker tracker)
    {
        string folderPath = Application.persistentDataPath + $"/LevelBests/{name}";

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/LevelBests/{name}/kills.smoop";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveKills newBests = new SaveKills(tracker);

        formatter.Serialize(stream, newBests);
        stream.Close();
    }

    public static SaveKills LoadKills(string name)
    {
        string path = Application.persistentDataPath + $"/LevelBests/{name}/kills.smoop";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            SaveKills kills = formatter.Deserialize(stream) as SaveKills;
            stream.Close();

            return kills;
        }
        else
        {
            return null;
        }
    }*/
    #endregion
}
