using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "ScriptableObjects/Player/PlayerDetails")]
public class PlayerDetailsSO : ScriptableObject
{
    public GameObject Prefab;
    public string Name;
    public float Speed;
    public int Health;
}
