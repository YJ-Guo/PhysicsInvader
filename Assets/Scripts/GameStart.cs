using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    public Button startGameButton;

    private void Start()
    {
        Screen.SetResolution(1000, 1920, true);
        this.startGameButton.onClick.AddListener(delegate { StartGame(); });
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("GamePlay");
        }    
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
