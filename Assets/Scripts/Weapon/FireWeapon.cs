using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    private float _fireRateCooldownTimer;
    private Weapon _weapon;
    private Player _player;
    private bool _shouldFire = true;
    private PoolManager _poolManager => PoolManager.Instance;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Settings.PLAYER_TAG).GetComponent<Player>();
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
        if (args.GameState == GameState.PauseMenu)
        {
            _shouldFire = false;
        }
        else if (args.GameState == GameState.Play)
        {
            _shouldFire = true;
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

    private IEnumerator FireAmmoRoutine(AmmoDetailsSO ammo)
    {
        int ammoCounter = 0;

        int ammoPerShot = Random.Range(ammo.SpawnAmountMin, ammo.SpawnAmountMax + 1);

        float ammoSpawnInterval;

        if (ammoPerShot > 1)
        {
            ammoSpawnInterval = Random.Range(ammo.SpawnIntervalMin, ammo.SpawnIntervalMax + Mathf.Epsilon);
        }
        else
        {
            ammoSpawnInterval = 0;
        }

        while (ammoCounter < ammoPerShot)
        {
            ammoCounter++;

            GameObject ammoPrefab = ammo.Prefab;

            float ammoSpeed = Random.Range(ammo.SpeedMin, ammo.SpeedMax + Mathf.Epsilon);
            float ammoRange = Random.Range(ammo.RangeMin, ammo.RangeMax + Mathf.Epsilon);

            IFireable fireableAmmo = 
                (IFireable)_poolManager.ReuseComponent(ammoPrefab, _weapon.ShootPosition.position, Quaternion.identity);

            Vector2 weaponAimDirectionVector = new Vector2();
            bool isAmmoSet;

            if (_weapon.WeaponDetails.IsAimable)
            {
                // Find closest enemy
                Enemy closestEnemy = null;
                EnemyFinder enemyFinder = _player.GetComponent<EnemyFinder>();
                closestEnemy = enemyFinder.FindClosestEnemy();
                
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
            else
            {
                // choose random direction
                weaponAimDirectionVector = ChooseRandomDirection(weaponAimDirectionVector);
                isAmmoSet = true;
            }

            fireableAmmo.InitialiseAmmo(ammo, ammoSpeed, ammoRange, weaponAimDirectionVector, isAmmoSet);

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