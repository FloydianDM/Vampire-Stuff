using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _exitButton;

    public void PlayGame()
    {
        SceneManager.LoadScene(Settings.GAME_SCENE_TAG);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(Settings.SETTINGS_SCENE_TAG, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
