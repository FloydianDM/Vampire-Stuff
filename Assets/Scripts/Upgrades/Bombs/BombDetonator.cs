using System.Collections;
using UnityEngine;

public class BombDetonator : MonoBehaviour
{
    private float _bombCooldownTimer;
    private BombUpgrade _bombUpgrade;
    private bool _canDetonate = false;
    private bool _shouldStopTimer;

    private void Start()
    {
        _bombUpgrade = GetComponent<BombUpgrade>();
        _bombCooldownTimer = _bombUpgrade.BombUpgradeDetails.CooldownTime;
    }

    private void OnEnable()
    {
        _bombUpgrade.BombActivationEvent.OnBombActivated += BombActivationEvent_OnBombActivated;
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        _bombUpgrade.BombActivationEvent.OnBombActivated -= BombActivationEvent_OnBombActivated;
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void Update()
    {
        if (_shouldStopTimer)
        {
            return;
        }

        if (!_canDetonate)
        {
            _bombCooldownTimer -= Time.deltaTime;
        }
        
        if (_bombCooldownTimer <= 0)
        {
            _canDetonate = true;
            _bombCooldownTimer = _bombUpgrade.BombUpgradeDetails.CooldownTime;
        }
    }

    private void BombActivationEvent_OnBombActivated(BombActivationEvent @event, BombActivationEventArgs args)
    {
        DetonateBomb();
    }
    
    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        if (args.GameState == GameState.PauseMenu)
        {
            _shouldStopTimer = true;
        }
        else if (args.GameState == GameState.Play)
        {
            _shouldStopTimer = false;
        }
    }

    private void DetonateBomb()
    {
        StartCoroutine(DetonateBombRoutine());
    }

    private IEnumerator DetonateBombRoutine()
    {
        // TODO: write logic
        
        
        
        yield return null;
    }
}
