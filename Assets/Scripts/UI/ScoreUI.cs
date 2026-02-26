using UnityEngine;
using TMPro;
using Core.Systems;

namespace UI
{
    public class ScoreUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScoreSystem scoreSystem;
        [SerializeField] private TextMeshProUGUI scoreText;

        private void OnEnable()
        {
            scoreSystem.OnScoreChanged += UpdateScore;
            UpdateScore(scoreSystem.CurrentScore);
        }

        private void OnDisable()
        {
            scoreSystem.OnScoreChanged -= UpdateScore;
        }

        private void UpdateScore(int newScore)
        {
            scoreText.text = newScore.ToString("0000");
        }
    }
}