using UnityEngine;

public class OldAdManager : MonoBehaviour
{
    public static OldAdManager Instance;
    public bool isRevived;
    public Vector3 playerPos;
    public int reviveCoins;
    public Vector3 playerScale;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}