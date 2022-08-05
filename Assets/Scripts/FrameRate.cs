using UnityEngine;

public class FrameRate : MonoBehaviour
{
    private int frameRate = 300;
    private void Awake()
    {
        Application.targetFrameRate = frameRate;
    }
}
