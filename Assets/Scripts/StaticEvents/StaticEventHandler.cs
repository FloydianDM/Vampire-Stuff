using System;

public static class StaticEventHandler
{
    public static event Action<CombatNotifierEventArgs> OnCombatNotified;
    public static event Action<GameStateChangedEventArgs> OnGameStateChanged;
    public static event Action<BombReadyEventArgs> OnBombReady;

    public static void CallCombatNotifiedEvent(string notificationText, float notificationTimer)
    {
        OnCombatNotified?.Invoke(new CombatNotifierEventArgs()
        {
            NotificationText = notificationText,
            NotificationTimer = notificationTimer
        });
    }

    public static void CallGameStateChangedEvent(GameState gameState)
    {
        OnGameStateChanged?.Invoke(new GameStateChangedEventArgs()
        {
            GameState = gameState
        });
    }

    public static void CallBombReadyEvent(bool isBombReady)
    {
        OnBombReady?.Invoke(new BombReadyEventArgs()
        {
            IsBombReady = isBombReady
        });
    }
}

public class CombatNotifierEventArgs : EventArgs
{
    public string NotificationText;
    public float NotificationTimer;
}

public class GameStateChangedEventArgs : EventArgs
{
    public GameState GameState;
}

public class BombReadyEventArgs : EventArgs
{
    public bool IsBombReady;
}

