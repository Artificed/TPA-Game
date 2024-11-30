using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/player.pl";
    
    public static void SaveGameData(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData(player);
        
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveData LoadGameData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData saveData = (SaveData) formatter.Deserialize(stream);
            stream.Close();

            Player.Instance.FileData = saveData;
            
            return saveData;
        }
        else
        {
            Debug.LogError("File not found!");
            return null;
        }
    }
}
