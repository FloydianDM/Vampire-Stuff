using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimateChest))]
[DisallowMultipleComponent]
public class Chest : MonoBehaviour, IUsable
{
    [SerializeField] private Transform _itemSpawnPoint;
    
    private AnimateChest _animateChest;
    private GameObject _chestItemGameObject;
    private ChestItem _chestItem;
    private ChestState _chestState = ChestState.Closed;
    private int _healthPercent;
    private WeaponDetailsSO _weaponDetails;
    private int _addedXP;
    private bool _isChestEnabled = false;
    
    public Animator Animator { get; private set; }
    private GameManager _gameManager => GameManager.Instance;
    private GameResources _gameResources => GameResources.Instance;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _animateChest = GetComponent<AnimateChest>();
    }

    public void InitializeUsable(ChestDetailsSO chestDetails)
    {
        _healthPercent = chestDetails.HealthPercent;
        _weaponDetails = chestDetails.WeaponDetails;
        _addedXP = chestDetails.AddedXP;

        EnableChest();
    }

    private void EnableChest()
    {
        gameObject.SetActive(true);
        _isChestEnabled = true;
    }

    public void UseItem()
    {
        if (!_isChestEnabled)
        {
            return;
        }

        switch (_chestState)
        {
            case ChestState.Closed:
                OpenChest();
                break;
            case ChestState.WeaponItem:
                CollectWeaponItem();
                break;
            case ChestState.HealthItem:
                CollectHealthItem();
                break;
            case ChestState.XP:
                CollectXP();
                break;
            default:
                return;
        }
    }

    private void OpenChest()
    {
        _animateChest.AnimateOpenChest();
        
        // add sound effects

        if (_weaponDetails != null)
        {
            // is weapon held by player, give health!
            if (!_gameManager.Player.IsWeaponHeldByPlayer(_weaponDetails))
            {
                _chestState = ChestState.WeaponItem;
                InstantiateWeaponItem();
            }
            else
            {
                // if the player health is full, add XP!
                if (_gameManager.Player.Health.CurrentHealth != _gameManager.Player.Health.MaxHealth)
                {
                    _chestState = ChestState.HealthItem;
                    InstantiateHealthItem();
                }
                else
                {
                    _chestState = ChestState.XP;
                    InstantiateXPItem();
                }
            }
        }
    }

    private void InstantiateItem()
    {
        _chestItemGameObject = Instantiate(_gameResources.ChestItemPrefab, transform);
        _chestItem = _chestItemGameObject.GetComponent<ChestItem>();
    }

    private void InstantiateWeaponItem()
    {
        InstantiateItem();

        _chestItem.InitializeChestItem(_weaponDetails.Sprite, _weaponDetails.Name, _itemSpawnPoint.position);
    }

    private void InstantiateHealthItem()
    {
        InstantiateItem();

        _chestItem.InitializeChestItem(_gameResources.HeartIconSprite, "%" + _healthPercent.ToString(), _itemSpawnPoint.position);
    }

    private void InstantiateXPItem()
    {
        InstantiateItem();
        _chestItem.InitializeChestItem(_gameResources.XPIconSprite, _addedXP.ToString() + " XP", _itemSpawnPoint.position);
    }

    private void CollectWeaponItem()
    {
        _gameManager.Player.AddWeaponToPlayerWeaponList(_weaponDetails);

        Destroy(gameObject);
    }

    private void CollectHealthItem()
    {
        _gameManager.Player.Health.AddHealth(_healthPercent);

        Destroy(gameObject);
    }

    private void CollectXP()
    {
        _gameManager.Player.ReceiveXPEvent.CallReceiveXPEvent(_addedXP);

        Destroy(gameObject);
    }
}
