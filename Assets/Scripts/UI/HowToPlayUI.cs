using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayUI : MonoBehaviour
{
    public void ExitHowToPlay()
    {
        SceneManager.UnloadSceneAsync(Settings.HOW_TO_PLAY_SCENE_TAG);
    }
}
