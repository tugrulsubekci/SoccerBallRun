using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2);
    }
}
