using UnityEngine;

[RequireComponent(typeof(BombDetonator))]
[RequireComponent(typeof(BombActivationEvent))]
[DisallowMultipleComponent]
public class BombUpgrade : Upgrade
{
    [HideInInspector] public BombUpgradeDetailsSO BombUpgradeDetails;
    [HideInInspector] public BombDetonator BombDetonator;
    [HideInInspector] public BombActivationEvent BombActivationEvent;

    protected override void Awake()
    {
        base.Awake();

        SpriteRenderer.sprite = BombUpgradeDetails.Sprite;
        BombDetonator = GetComponent<BombDetonator>();
        BombActivationEvent = GetComponent<BombActivationEvent>();
    }
}
