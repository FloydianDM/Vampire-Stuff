using System;

public class ScoreChangedEvent
{
    public event Action<ScoreChangedEventArgs> OnScoreChanged;

    public void CallScoreChangedEvent(int addedScore)
    {
        OnScoreChanged?.Invoke(new ScoreChangedEventArgs()
        {
            AddedScore = addedScore
        });
    }
}

public class ScoreChangedEventArgs : EventArgs
{
    public int AddedScore;
}
