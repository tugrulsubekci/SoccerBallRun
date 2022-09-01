using UnityEngine;
using UnityEngine.UI;

public class CloseMe : MonoBehaviour
{
    private Button closeMeButton;
    private void Start()
    {
        closeMeButton = GetComponent<Button>();
        closeMeButton.onClick.AddListener(() => Close());
    }
    public void Close()
    {
        transform.parent.gameObject.SetActive(false);
    }
}