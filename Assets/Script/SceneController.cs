using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
    
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

    public void UpdateGlobalVolume(System.Single volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        AudioListener.volume = volume;
    }
}
