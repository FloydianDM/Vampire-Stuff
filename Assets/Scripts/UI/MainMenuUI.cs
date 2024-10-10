using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _howToPlayButton;
    [SerializeField] private GameObject _exitButton;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    private void Start()
    {
        ShowHighScore();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(Settings.GAME_SCENE_TAG);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(Settings.SETTINGS_SCENE_TAG, LoadSceneMode.Additive);
    }

    public void OpenHowToPlay()
    {
        SceneManager.LoadScene(Settings.HOW_TO_PLAY_SCENE_TAG, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ShowHighScore()
    {
        _highScoreText.text = "High Score: " + PlayerPrefs.GetInt(Settings.HIGH_SCORE_KEY, 0);
    }
}
