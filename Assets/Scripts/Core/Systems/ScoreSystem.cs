using System;
using UnityEngine;

namespace Core.Systems
{
    public class ScoreSystem : MonoBehaviour
    {
        public event Action<int> OnScoreChanged;

        public int CurrentScore { get; private set; }

        public void ResetScore()
        {
            CurrentScore = 0;
            OnScoreChanged?.Invoke(CurrentScore);
        }

        public void AddPoints(int amount)
        {
            CurrentScore += amount;
            OnScoreChanged?.Invoke(CurrentScore);
        }
    }
}