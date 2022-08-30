using UnityEngine;

public class DestroyYourself : MonoBehaviour
{
    public static DestroyYourself Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
