using UnityEngine;

public class FrameRate : MonoBehaviour
{
    private int frameRate = 60;
    private void Awake()
    {
        Application.targetFrameRate = frameRate;
    }
}
