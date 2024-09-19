using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    private AmmoDetailsSO _ammoDetails;
    private float _ammoSpeed;
    private float _ammoRange;
    private Vector2 _fireDirectionVector;
    private bool _isColliding;
    private bool _isAmmoSet;
    private bool _shouldMove = true;

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
        if (args.GameState == GameState.PauseMenu)
        {
            _shouldMove = false;
        }
        else if (args.GameState == GameState.Play)
        {
            _shouldMove = true;
        }
    }

    private void Update()
    {
        if (!_shouldMove)
        {
            return;
        }

        Vector2 distanceVector = _fireDirectionVector * (_ammoSpeed * Time.deltaTime);

        transform.position += (Vector3)distanceVector;

        _ammoRange -= distanceVector.magnitude;

        if (_ammoRange < 0)
        {
            DisableAmmo();
        }
    }

    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float ammoSpeed, float ammoRange, Vector2 fireDirectionVector, bool isAmmoSet)
    {
        _ammoDetails = ammoDetails;
        _isColliding = false;

        _ammoSpeed = ammoSpeed;
        _ammoRange = ammoRange;
        _isAmmoSet = isAmmoSet;
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
        DisableAmmo();
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

            int ammoDamage = Random.Range(_ammoDetails.DamageMin, _ammoDetails.DamageMax + 1);

            health.TakeDamage(ammoDamage);
        }
    }
}
