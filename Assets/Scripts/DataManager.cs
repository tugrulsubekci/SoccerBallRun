using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public int playerIndex;
    public int coins;
    public int levelIndex;
    public int levelNumber;
    public int largerSkill_Level;
    public int smallerSkill_Level;
    public int ballIndex;
    public bool isMusicOn = true;
    public bool isVibrationOn = true;
    public bool[] purchaseData;
    public bool[] selectData;
    public bool isDisplayed; // for instructional object (don't save)
    public bool isInAppReviewShown;

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

    }
    private void Start()
    {
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
        public int levelNumber;
        public int largerSkill_Level;
        public int smallerSkill_Level;
        public int ballIndex;
        public bool isMusicOn;
        public bool isVibrationOn;
        public bool[] purchaseData;
        public bool[] selectData;
        public bool isInAppReviewShown;
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.playerIndex = playerIndex;
        data.coins = coins;
        data.levelIndex = levelIndex;
        data.levelNumber = levelNumber;
        data.largerSkill_Level = largerSkill_Level;
        data.smallerSkill_Level = smallerSkill_Level;
        data.ballIndex = ballIndex;
        data.isMusicOn = isMusicOn;
        data.isVibrationOn = isVibrationOn;
        data.purchaseData = purchaseData;
        data.selectData = selectData;
        data.isInAppReviewShown = isInAppReviewShown;
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
            levelNumber = data.levelNumber;
            levelIndex = data.levelIndex;
            largerSkill_Level = data.largerSkill_Level;
            smallerSkill_Level = data.smallerSkill_Level;
            ballIndex = data.ballIndex;
            isMusicOn = data.isMusicOn;
            isVibrationOn = data.isVibrationOn;
            purchaseData = data.purchaseData;
            selectData = data.selectData;
            isInAppReviewShown = data.isInAppReviewShown;
        }
    }
}
