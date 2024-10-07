using UnityEngine;

[RequireComponent(typeof(BombDetonator))]
[RequireComponent(typeof(BombActivationEvent))]
[DisallowMultipleComponent]
public class BombUpgrade : Upgrade
{
    public BombUpgradeDetailsSO BombUpgradeDetails;
    [HideInInspector] public BombDetonator BombDetonator;
    [HideInInspector] public BombActivationEvent BombActivationEvent;
    [HideInInspector] public float ImpactArea;
    [HideInInspector] public float CooldownTime;
    [HideInInspector] public int Damage;
    
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
