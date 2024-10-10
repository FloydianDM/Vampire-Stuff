using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private ScoreManager _scoreManager => ScoreManager.Instance;
    
    private void OnEnable()
    {
        _scoreText.text = "SCORE: " + _scoreManager.Score;
        _scoreManager.ScoreChangedEvent.OnScoreChanged += ScoreChangedEvent_OnScoreChanged;
    }

    private void OnDisable()
    {
        _scoreManager.ScoreChangedEvent.OnScoreChanged -= ScoreChangedEvent_OnScoreChanged;
    }

    private void ScoreChangedEvent_OnScoreChanged(ScoreChangedEventArgs args)
    {
        _scoreText.text = "SCORE: " + _scoreManager.Score;
    }
}
