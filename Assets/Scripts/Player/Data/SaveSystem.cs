using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/player.pl";
    
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerSaveData playerSaveData = new PlayerSaveData(player);
        
        formatter.Serialize(stream, playerSaveData);
        stream.Close();
    }

    public static PlayerSaveData LoadPlayer()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSaveData playerSaveData = (PlayerSaveData) formatter.Deserialize(stream);
            stream.Close();
            
            return playerSaveData;
        }
        else
        {
            Debug.LogError("File not found!");
            return null;
        }
    }
}
