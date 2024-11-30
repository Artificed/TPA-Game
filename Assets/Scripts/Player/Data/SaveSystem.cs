using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/data.dat";
    
    public static void SaveGameData(PlayerDataSO player, PlayerUpgradesSO playerUpgrades)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData(player, playerUpgrades);
        
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
            
            return saveData;
        }
        else
        {
            Debug.LogError("File not found!");
            return null;
        }
    }
}
