using System.Collections;
using UnityEngine;

public class BombDetonator : MonoBehaviour
{
    [SerializeField] private LayerMask _destroyableLayerMask;
    
    private float _bombCooldownTimer;
    private BombUpgrade _bombUpgrade;
    private bool _canDetonate;
    private bool _shouldStopTimer;
    private float _effectTimeAdjuster = 2.5f;
    
    private GameResources _gameResources => GameResources.Instance;

    private void Awake()
    {
        _bombUpgrade = GetComponent<BombUpgrade>();
        _bombCooldownTimer = _bombUpgrade.CooldownTime;
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

        BombTimer();
    }

    private void BombActivationEvent_OnBombActivated(BombActivationEvent @event)
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
    
    private void BombTimer()
    {
        if (!_canDetonate)
        {
            _bombCooldownTimer -= Time.deltaTime;
        }

        if (_bombCooldownTimer <= 0)
        {
            _canDetonate = true;
            _bombCooldownTimer = _bombUpgrade.CooldownTime;
        }
    }

    private void DetonateBomb()
    {
        if (!_canDetonate)
        {
            return;
        }

        StartCoroutine(DetonateBombRoutine());
    }

    private IEnumerator DetonateBombRoutine()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, _bombUpgrade.ImpactArea, _destroyableLayerMask);
        
        foreach (Collider2D col in colliderArray)
        {
           col.GetComponent<Health>()?.TakeDamage(_bombUpgrade.Damage);
        }

        _canDetonate = false;

        GameObject bombDetonationEffect = Instantiate(_gameResources.BombDetonationEffect, transform);
        bombDetonationEffect.GetComponent<BombDetonationEffect>().PlayBombDetonationEffect();
        
        yield return new WaitForSeconds(_effectTimeAdjuster);
        
        Destroy(bombDetonationEffect);
    }
}
