using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyMovement : MonoBehaviour
{
    private Transform _playerTransform;
    private Enemy _enemy;
    private float _speed;
    private float _dodgeThrust;
    private float _followSensitivity;
    private float _dodgeTime = 1f;
    private bool _shouldFollow = true;
    [HideInInspector] public bool IsDodged;

    private GameManager _gameManager => GameManager.Instance;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _followSensitivity = 1 / _enemy.EnemyDetails.Awareness;
    }

    private void Start()
    {
        if (_gameManager.GameState == GameState.PauseMenu)
        {
            _shouldFollow = false;
        }

        FindPlayer();
        FollowPlayer();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;

        _speed = Random.Range(_enemy.EnemyDetails.SpeedMin, _enemy.EnemyDetails.SpeedMax + Mathf.Epsilon);
        _dodgeThrust = _enemy.EnemyDetails.DodgeThrust;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        if (args.GameState == GameState.PauseMenu)
        {
            _shouldFollow = false;
            StopAllCoroutines();
        }
        else if (args.GameState == GameState.Play)
        {
            _shouldFollow = true;
            FollowPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (!_shouldFollow)
        {
            _enemy.Rigidbody.velocity = Vector2.zero;

            return;
        }
    }

    private void FindPlayer()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag(Settings.PLAYER_TAG).transform;
        }
    }

    private void FollowPlayer()
    {
        StartCoroutine(FollowPlayerRoutine());
    }

    private IEnumerator FollowPlayerRoutine()
    {
        while (_shouldFollow)
        {
            if (_playerTransform != null)
            {
                Vector2 direction = (_playerTransform.position - transform.position).normalized;

                _enemy.Rigidbody.velocity = direction * _speed;
            }

            yield return new WaitForSeconds(_followSensitivity);
        }
    }

    public void DodgeAmmo(float hitChance)
    {
        StartCoroutine(DodgeAmmoRoutine(hitChance));
    }

    private IEnumerator DodgeAmmoRoutine(float hitChance)
    {
        if (hitChance < _enemy.EnemyDetails.Agility)
        {
            IsDodged = true;

            _enemy.Rigidbody.velocity = Vector2.zero;

            float dodgeVectorX = Random.Range(15f + Mathf.Epsilon, 10f);
            float dodgeVectorY = Random.Range(10f, 15f + Mathf.Epsilon);
            Vector2 dodgeVector = new Vector3(dodgeVectorX, dodgeVectorY);

            _enemy.Rigidbody.AddForce(dodgeVector * _dodgeThrust);

            yield return new WaitForSeconds(_dodgeTime);

            StaticEventHandler.CallCombatNotifiedEvent("ENEMY DODGED", 0.5f);
        }
        else
        {
            yield return null;
        }
    }
}
