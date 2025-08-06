using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public string mainMenu;
    public string platformer;
    public GameObject pauseScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pressed");
            PauseUnpause();
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
    public void Platformer()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(platformer);

    }
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Quit");
    }
    public void PauseUnpause()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }

    }
}
