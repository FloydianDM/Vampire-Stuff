using UnityEngine;

[DisallowMultipleComponent]
public class ScoreManager : SingletonMonobehaviour<ScoreManager>
{
    public int Score { get; private set; }
    private int _highScore;
    public readonly ScoreChangedEvent ScoreChangedEvent = new ScoreChangedEvent();

    private void Start()
    {
        if (PlayerPrefs.HasKey(Settings.HIGH_SCORE_KEY))
        {
            _highScore = PlayerPrefs.GetInt(Settings.HIGH_SCORE_KEY, _highScore);
        }
    }

    private void OnEnable()
    {
        ScoreChangedEvent.OnScoreChanged += ScoreChangedEvent_OnScoreChanged;
    }

    private void OnDisable()
    {
        ScoreChangedEvent.OnScoreChanged -= ScoreChangedEvent_OnScoreChanged;
    }

    private void ScoreChangedEvent_OnScoreChanged(ScoreChangedEventArgs args)
    {
        AddScore(args.AddedScore);
    }

    private void AddScore(int addedScore)
    {
        Score += addedScore;
    }

    public void SaveHighScore()
    {
        if (Score > _highScore)
        {
            PlayerPrefs.SetInt(Settings.HIGH_SCORE_KEY, Score);
        }
    }
}
