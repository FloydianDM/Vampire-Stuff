using UnityEngine;

public class Upgrade : MonoBehaviour
{
    protected SpriteRenderer SpriteRenderer;

    protected virtual void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
}
