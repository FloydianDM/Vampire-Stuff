using UnityEngine;

[DisallowMultipleComponent]
public class EnemyDeathEffect : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void InitialiseEnemyDeathEffect(Vector2 enemyPosition)
    {
        transform.position = enemyPosition;

        gameObject.SetActive(true);
    }

    public void PlayEnemyDeathEffect()
    {
        _particleSystem.Play();
    }
}
