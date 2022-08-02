using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public int playerIndex;
    public int coins;
    public int levelIndex;
    public int largerSkill_Level;
    public int smallerSkill_Level;
    public int ballIndex;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        File.Delete(Application.persistentDataPath + "/datafile.json"); // This line can be activated, If you want to delete save file.
        Load();
        if (levelIndex != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(levelIndex);
        }
    }
    [System.Serializable]
    class SaveData
    {
        public int playerIndex;
        public int coins;
        public int levelIndex;
        public int largerSkill_Level;
        public int smallerSkill_Level;
        public int ballIndex;
    }
    public void Save()
    {
        SaveData data = new SaveData();
        data.playerIndex = playerIndex;
        data.coins = coins;
        data.levelIndex = levelIndex;
        data.largerSkill_Level = largerSkill_Level;
        data.smallerSkill_Level = smallerSkill_Level;
        data.ballIndex = ballIndex;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/datafile.json", json);
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/datafile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            coins = data.coins;
            playerIndex = data.playerIndex;
            levelIndex = data.levelIndex;
            largerSkill_Level = data.largerSkill_Level;
            smallerSkill_Level = data.smallerSkill_Level;
            ballIndex = data.ballIndex;
        }
    }
}