using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    private int _ammoDamage;
    private float _ammoSpeed;
    private float _ammoRange;
    private Vector2 _fireDirectionVector;
    private Vector3 _fieldEffectAmmoGrowMultiplier = new Vector3(0.01f, 0.01f, 0);
    private bool _isColliding;
    private bool _isAmmoSet;
    private bool _isFieldEffect;
    private bool _shouldMove = true;

    private GameManager _gameManager => GameManager.Instance;

    private void OnEnable()
    {
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }

    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        switch (args.GameState)
        {
            case GameState.Pause:
            case GameState.LevelUp:
                _shouldMove = false;
                break;
            case GameState.Play:
                _shouldMove = true;
                break;
        }
    }

    private void Update()
    {
        if (!_shouldMove)
        {
            return;
        }

        if (!_isFieldEffect)
        {
            Vector2 distanceVector = _fireDirectionVector * (_ammoSpeed * Time.deltaTime);

            transform.position += (Vector3)distanceVector;

            _ammoRange -= distanceVector.magnitude;
        }
        else
        {
            gameObject.transform.localScale += _ammoSpeed * _fieldEffectAmmoGrowMultiplier;

            _ammoRange -= Time.deltaTime;

            transform.position = _gameManager.Player.GetPlayerPosition();
        }

        if (_ammoRange < 0)
        {
            DisableAmmo();
        }
    }

    public void InitialiseAmmo(int ammoDamage, float ammoSpeed, float ammoRange, Vector2 fireDirectionVector, bool isAmmoSet, 
        bool isFieldEffect)
    {
        _isColliding = false;

        _ammoDamage = ammoDamage;
        _ammoSpeed = ammoSpeed;
        _ammoRange = ammoRange;
        _isAmmoSet = isAmmoSet;
        _isFieldEffect = isFieldEffect;
        _fireDirectionVector = fireDirectionVector;

        if (!_isAmmoSet)
        {
            DisableAmmo();

            return;
        }

        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Settings.PLAYER_TAG))
        {
            return;
        }

        if (_isColliding)
        {
            return;
        }

        DealDamage(other);

        if (!_isFieldEffect)
        {
            DisableAmmo();
        }
    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void DealDamage(Collider2D other)
    {
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            _isColliding = true;

            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                float hitChance = Random.Range(0, 20 + Mathf.Epsilon);

                enemy.EnemyMovement.DodgeAmmo(hitChance);

                if (enemy.EnemyMovement.IsDodged)
                {
                    enemy.EnemyMovement.IsDodged = false;
                    
                    return;
                }
            }
            
            health.TakeDamage(_ammoDamage);
        }
    }
}
