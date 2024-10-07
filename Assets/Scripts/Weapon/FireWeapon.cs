using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    private float _fireRateCooldownTimer;
    private Weapon _weapon;
    private Player _player;
    private bool _shouldFire = true;

    private PoolManager _poolManager => PoolManager.Instance;
    private GameManager _gameManager => GameManager.Instance;

    private void Start()
    {
        _player = _gameManager.Player;
        _weapon = GetComponent<Weapon>();
    }

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
                _shouldFire = false;
                break;
            case GameState.Play:
                _shouldFire = true;
                break;
        }
    }

    private void Update()
    {
        if (!_shouldFire)
        {
            return;
        }

        _fireRateCooldownTimer -= Time.deltaTime;
        
        WeaponFire();
    }

    private void WeaponFire()
    {
        if (IsWeaponReadyToFire())
        {
            FireAmmo();
            ResetCooldownTimer();
        }
    }

    private bool IsWeaponReadyToFire()
    {
        if (_fireRateCooldownTimer > 0)
        {
            return false;
        }

        return true;
    }

    private void FireAmmo()
    {
        AmmoDetailsSO ammo = _weapon.WeaponDetails.WeaponAmmo;

        if (ammo != null)
        {
            StartCoroutine(FireAmmoRoutine(ammo));
        }
    }

    private IEnumerator FireAmmoRoutine(AmmoDetailsSO ammoDetails)
    {
        int ammoCounter = 0;

        int ammoPerShot = Random.Range(ammoDetails.SpawnAmountMin, ammoDetails.SpawnAmountMax + 1);

        float ammoSpawnInterval;

        if (ammoPerShot > 1)
        {
            ammoSpawnInterval = Random.Range(ammoDetails.SpawnIntervalMin, ammoDetails.SpawnIntervalMax + Mathf.Epsilon);
        }
        else
        {
            ammoSpawnInterval = 0;
        }

        while (ammoCounter < ammoPerShot)
        {
            ammoCounter++;

            GameObject ammoPrefab = ammoDetails.Prefab;

            int ammoDamage = Mathf.CeilToInt(Random.Range(ammoDetails.DamageMin, ammoDetails.DamageMax + 1) * _weapon.DamageModifier);
            float ammoSpeed = Random.Range(ammoDetails.SpeedMin, ammoDetails.SpeedMax + Mathf.Epsilon);
            float ammoRange = Random.Range(ammoDetails.RangeMin, ammoDetails.RangeMax + Mathf.Epsilon);

            IFireable fireableAmmo = 
                (IFireable)_poolManager.ReuseComponent(ammoPrefab, _weapon.ShootPosition.position, Quaternion.identity);

            Vector2 weaponAimDirectionVector = new Vector2();
            bool isAmmoSet;
            bool isFieldEffect;

            if (_weapon.WeaponDetails.IsAimable)
            {
                // Find closest enemy
                Enemy closestEnemy = null;
                EnemyFinder enemyFinder = _player.GetComponent<EnemyFinder>();
                closestEnemy = enemyFinder.FindClosestEnemy();
                isFieldEffect = false;
                
                if (closestEnemy != null)
                {
                    weaponAimDirectionVector = closestEnemy.gameObject.transform.position - transform.position;
                    weaponAimDirectionVector = weaponAimDirectionVector.normalized;
                    isAmmoSet = true;
                }
                else
                {
                    // no enemy around
                    weaponAimDirectionVector = Vector2.zero;
                    isAmmoSet = false;
                }
            }
            else if (_weapon.WeaponDetails.isFieldEffect)
            {
                // grow the ammo game object
                weaponAimDirectionVector = Vector2.down;
                isFieldEffect = true;
                isAmmoSet = true;

            }
            else
            {
                // choose random direction
                weaponAimDirectionVector = ChooseRandomDirection(weaponAimDirectionVector);
                isAmmoSet = true;
                isFieldEffect = false;
            }

            fireableAmmo.InitialiseAmmo(ammoDamage, ammoSpeed, ammoRange, weaponAimDirectionVector, isAmmoSet, isFieldEffect);

            yield return new WaitForSeconds(ammoSpawnInterval);     
        }
    }

    private Vector2 ChooseRandomDirection(Vector2 weaponAimDirectionVector)
    {
        int randomIndex = Random.Range(0, 4);

        switch (randomIndex)
        {
            case 0:
                weaponAimDirectionVector = Vector2.left;
                break;
            case 1:
                weaponAimDirectionVector = Vector2.up;
                break;
            case 2:
                weaponAimDirectionVector = Vector2.right;
                break;
            case 3:
                weaponAimDirectionVector = Vector2.down;
                break;
        }

        return weaponAimDirectionVector;
    }

    private void ResetCooldownTimer()
    {
        _fireRateCooldownTimer = _weapon.WeaponDetails.FireRate;
    }   
}