using UnityEngine;

[RequireComponent(typeof(LevelUpEvent))]
[DisallowMultipleComponent]
public class PlayerLevelManager : MonoBehaviour
{
    public int PlayerLevel { get; private set; }
    private int _startingPlayerLevel = 1;
    private LevelUpEvent _levelUpEvent;

    private void Awake()
    {
        _levelUpEvent = GetComponent<LevelUpEvent>();

        PlayerLevel = _startingPlayerLevel;
    }

    public void GainLevelUp()
    {
        PlayerLevel++;

        _levelUpEvent.CallLevelUpEvent(PlayerLevel);
    }
}
