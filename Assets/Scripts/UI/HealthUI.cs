using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{   
    [SerializeField] private Image _healthUIImage;

    private GameManager _gameManager => GameManager.Instance;

    private void Start()
    {
        _healthUIImage.fillAmount = 1;
    }

    private void OnEnable()
    {
        _gameManager.Player.HealthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }

    private void OnDisable()
    {
        _gameManager.Player.HealthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent @event, HealthEventArgs args)
    {
        float fillAmount = args.HealthPercent;
        _healthUIImage.fillAmount = fillAmount;
    }
}
