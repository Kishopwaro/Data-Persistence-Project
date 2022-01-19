using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataBetweenScenes : MonoBehaviour
{
    public static DataBetweenScenes Instance;

    public bool isInStartMenu;
    public string nameText = "Player";
    public string highScoreNameText;
    public int highScore;

    private void Awake()
    {        
        if (Instance != null) //singleton
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighscore();
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string highScoreNameText;
    }

    public void SaveHighscore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        data.highScoreNameText = highScoreNameText;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    private void LoadHighscore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            highScoreNameText = data.highScoreNameText;
        }
    }

}


