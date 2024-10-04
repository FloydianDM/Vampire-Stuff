using UnityEngine;

[DisallowMultipleComponent]
public class BombOperator : MonoBehaviour
{
    private GameObject _bombPocket;
    public bool HasBombInPocket;

    public void AddBombToPocket(GameObject bombUpgrade)
    {
        if (_bombPocket != null)
        {
            Destroy(_bombPocket);
        }

        HasBombInPocket = true;
        _bombPocket = Instantiate(bombUpgrade, transform);
    }

    public void OperateBomb()
    {
        _bombPocket.GetComponent<BombUpgrade>().BombActivationEvent.CallBombActivationEvent();
    }
}
