using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GotoMainGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GotoTitleScreen()
    {
        SceneManager.LoadScene("Title");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
