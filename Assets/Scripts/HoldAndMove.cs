using UnityEngine;
using UnityEngine.UI;

public class HoldAndMove : MonoBehaviour
{
    private MenuManager menuManager;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartGame);
    }
    public void StartNow()
    {
        menuManager.gameManager.isGameStarted = true;
        menuManager.player.GetComponent<Rolling>().Roll();
    }
    public void StartGame()
    {
        menuManager = transform.parent.parent.GetComponent<MenuManager>();
        menuManager.audioManager.Play("Start");
        menuManager.menuObjects.SetActive(false);
        Invoke(nameof(StartNow), 0.5f);
    }
}
