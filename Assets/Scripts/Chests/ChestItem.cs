using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class ChestItem : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private TextMeshPro _textTMP;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _textTMP = GetComponentInChildren<TextMeshPro>();
    }

    public void InitializeChestItem(Sprite sprite, string itemText, Vector2 spawnPosition)
    {
        _spriteRenderer.sprite = sprite;
        _textTMP.text = itemText;
        transform.position = spawnPosition;
    }
}
