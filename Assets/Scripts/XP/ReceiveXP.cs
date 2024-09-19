using UnityEngine;

[RequireComponent(typeof(ReceiveXPEvent))]
[DisallowMultipleComponent]
public class ReceiveXP : MonoBehaviour
{
    [SerializeField] private int _xpThreshold = 100;

    public int XPThreshold => _xpThreshold;
    public int XP { get; private set; }
    private int _startingXP = 0;
    private float _xpThresholdModifier = 1.2f;
    private ReceiveXPEvent _receiveXPEvent;
    private Player _player;

    private void Awake()
    {
        XP = _startingXP;
        _receiveXPEvent = GetComponent<ReceiveXPEvent>();
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _receiveXPEvent.OnReceiveXPEvent += ReceiveXPEvent_OnReceiveXPEvent;
        _player.LevelUpEvent.OnLevelUpEvent += LevelUpEvent_OnLevelUpEvent;
    }

    private void OnDisable()
    {
        _receiveXPEvent.OnReceiveXPEvent -= ReceiveXPEvent_OnReceiveXPEvent;
        _player.LevelUpEvent.OnLevelUpEvent -= LevelUpEvent_OnLevelUpEvent;
    }

    private void ReceiveXPEvent_OnReceiveXPEvent(ReceiveXPEvent @event, ReceiveXPEventArgs args)
    {
        XP += args.XPAmount;

        if (XP > _xpThreshold)
        {
            _player.PlayerLevelManager.GainLevelUp();
            XP = 0;
        }
    }

    private void LevelUpEvent_OnLevelUpEvent(LevelUpEvent @event, LevelUpEventArgs args)
    {
        _xpThreshold *= Mathf.FloorToInt(args.PlayerLevel * _xpThresholdModifier); 
    }
}
