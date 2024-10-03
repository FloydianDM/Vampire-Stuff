using UnityEngine;

[RequireComponent(typeof(BombDetonator))]
[RequireComponent(typeof(BombActivationEvent))]
[DisallowMultipleComponent]
public class BombUpgrade : Upgrade
{
    [SerializeField] private BombUpgradeDetailsSO _bombUpgradeDetails;
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
        SpriteRenderer.sprite = _bombUpgradeDetails.Sprite;
        SpriteRenderer.color = _bombUpgradeDetails.SpriteColor;
        ImpactArea = _bombUpgradeDetails.ImpactArea;
        CooldownTime = _bombUpgradeDetails.CooldownTime;
        Damage = _bombUpgradeDetails.Damage;

        SpriteRenderer.gameObject.SetActive(false);
    }
}
