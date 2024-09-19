using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    public void ExitSettings()
    {
        SceneManager.UnloadSceneAsync(Settings.SETTINGS_SCENE_TAG);
    }
}
