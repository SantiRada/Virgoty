using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScene : MonoBehaviour {

    public static bool inPause = false;

    public void GoToGame(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void PauseGame()
    {
        inPause = !inPause;
        Time.timeScale = inPause ? 1f : 0f;
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
