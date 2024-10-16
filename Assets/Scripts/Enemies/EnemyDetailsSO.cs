using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "ScriptableObjects/Enemies/EnemyDetails")]
public class EnemyDetailsSO : ScriptableObject
{
    public string Type;
    public Sprite Sprite;
    public Color SpriteColor;
    public float SpeedMin;
    public float SpeedMax;
    public float DodgeThrust;
    public int HealthMin;
    public int HealthMax;
    public float Damage;
    public int ContactDamageMin;
    public int ContactDamageMax;
    [Range(1, 20)] public int ExperienceDrop;
    [Range(0, 2)] public float Awareness;
    [Range(0, 20)] public float Agility;

    [Header("EFFECTS")]
    public EnemyDeathEffectSO EnemyDeathEffect;
}
