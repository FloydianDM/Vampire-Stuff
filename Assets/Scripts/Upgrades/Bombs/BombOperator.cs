using UnityEngine;

[DisallowMultipleComponent]
public class BombOperator : MonoBehaviour
{
    public GameObject BombPocket { get; private set; }
    public bool HasBombInPocket { get; private set; }

    public void AddBombToPocket(GameObject bombUpgrade)
    {
        if (BombPocket != null)
        {
            Destroy(BombPocket);
        }

        HasBombInPocket = true;
        BombPocket = Instantiate(bombUpgrade, transform);
    }

    public void OperateBomb()
    {
        BombPocket.GetComponent<BombUpgrade>().BombActivationEvent.CallBombActivationEvent();
    }
}
