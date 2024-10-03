using UnityEngine;

[DisallowMultipleComponent]
public class BombOperator : MonoBehaviour
{
    [SerializeField] private GameObject _bombUpgradePrefab; // temp
    
    private BombUpgrade _bombPocket;
    public bool HasBombInPocket;

    private void Start()
    {
        AddBombToPocket(_bombUpgradePrefab); // temp
    }

    public void AddBombToPocket(GameObject bombUpgrade)
    {
        HasBombInPocket = true;
        // temp code
        GameObject prefab = Instantiate(_bombUpgradePrefab, transform);
        _bombPocket = prefab.GetComponent<BombUpgrade>();
        // temp code is finished
    }

    public void OperateBomb()
    {
        _bombPocket.BombActivationEvent.CallBombActivationEvent();
    }
}
