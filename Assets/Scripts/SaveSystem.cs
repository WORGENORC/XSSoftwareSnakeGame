using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveScoreboard(GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/scoreboard.xssoftware";
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreboardData data = new ScoreboardData(gameManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ScoreboardData LoadScoreboard()
    {
        string path = Application.persistentDataPath + "/scoreboard.xssoftware";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            if(stream.Length != 0)
            {
                ScoreboardData data = formatter.Deserialize(stream) as ScoreboardData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.Log("Save file not found " + path);
                return null;
            }   
        }
        else
        {
            Debug.Log("Save file not found " + path);
            return null;
        }
    }
}
