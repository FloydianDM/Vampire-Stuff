using UnityEngine;

[DisallowMultipleComponent]
public class BombOperator : MonoBehaviour
{
    private BombUpgrade _bombPocket;

    public void AddBombToPocket(BombUpgrade bombUpgrade)
    {
        _bombPocket = bombUpgrade;
    }

    public void OperateBomb()
    {
        _bombPocket.BombActivationEvent.CallBombActivationEvent();
    }
}
