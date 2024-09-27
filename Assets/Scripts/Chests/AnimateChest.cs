using UnityEngine;

public class AnimateChest : MonoBehaviour
{
    private Chest _chest;

    private void Awake()
    {
        _chest = GetComponent<Chest>();
    }

    public void AnimateOpenChest()
    {
        _chest.Animator.SetBool(Settings.IsUsing, true);
    }
}
