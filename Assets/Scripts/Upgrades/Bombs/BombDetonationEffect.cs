using UnityEngine;

public class BombDetonationEffect : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void PlayBombDetonationEffect()
    {
        _particleSystem.Play();
    }
}
