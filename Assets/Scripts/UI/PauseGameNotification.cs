using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameNotification : MonoBehaviour
{
    [SerializeField] private Image _notificationImage;
    [SerializeField] private float _blinkingInterval = 0.3f;

    private void OnEnable()
    {
        BlinkPauseGameNotification();
    }

    private void BlinkPauseGameNotification()
    {
        StartCoroutine(BlinkPauseGameNotificationRoutine());
    }

    private IEnumerator BlinkPauseGameNotificationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_blinkingInterval);

            _notificationImage.enabled = false;

            yield return new WaitForSeconds(_blinkingInterval);

            _notificationImage.enabled = true;
        }
    }
}
