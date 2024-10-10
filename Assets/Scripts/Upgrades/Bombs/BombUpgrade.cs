using UnityEngine;

[RequireComponent(typeof(BombDetonator))]
[RequireComponent(typeof(BombActivationEvent))]
[DisallowMultipleComponent]
public class BombUpgrade : Upgrade
{
    public BombUpgradeDetailsSO BombUpgradeDetails;
    
    public BombDetonator BombDetonator { get; private set; }
    public BombActivationEvent BombActivationEvent { get; private set; }
    public float ImpactArea { get; private set; }
    public float CooldownTime { get; private set; }
    public int Damage { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        BombDetonator = GetComponent<BombDetonator>();
        BombActivationEvent = GetComponent<BombActivationEvent>();
        SpriteRenderer.sprite = BombUpgradeDetails.Sprite;
        SpriteRenderer.color = BombUpgradeDetails.SpriteColor;
        ImpactArea = BombUpgradeDetails.ImpactArea;
        CooldownTime = BombUpgradeDetails.CooldownTime;
        Damage = BombUpgradeDetails.Damage;
    }
}
