using System.Collections;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class CombatNotifierUI : MonoBehaviour
{
    [SerializeField] private GameObject _combatNotifier;
    [SerializeField] private TextMeshProUGUI _combatNotifierText;

    private bool _shouldShow = true;

    private void Awake()
    {
        _combatNotifier.SetActive(false);
    }

    private void OnEnable()
    {
        StaticEventHandler.OnCombatNotified += StaticEventHandler_OnCombatNotified;
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnCombatNotified -= StaticEventHandler_OnCombatNotified;
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void StaticEventHandler_OnCombatNotified(CombatNotifierEventArgs args)
    {
        ShowCombatNotifierText(args.NotificationText, args.NotificationTimer);
    }

    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        if (args.GameState == GameState.PauseMenu)
        {
            _shouldShow = false;
        }
        else if (args.GameState == GameState.Play)
        {
            _shouldShow = true;
        }
    }

    public void ShowCombatNotifierText(string text, float timer)
    {
        if (!_shouldShow)
        {
            return;
        }

        StartCoroutine(ShowCombatNotifierTextRoutine(text, timer));
    }

    private IEnumerator ShowCombatNotifierTextRoutine(string text, float timer)
    {
        _combatNotifierText.text = text;
        _combatNotifier.SetActive(true);

        yield return new WaitForSeconds(timer);

        _combatNotifier.SetActive(false);
    }
}
