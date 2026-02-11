using System;
using UnityEngine;

namespace Core.Systems
{
    public class LifeSystem : MonoBehaviour
    {
        public event Action<int> OnLivesChanged;
        public event Action OnNoLivesLeft;

        [SerializeField] private int startingLives = 3;

        private int currentLives;

        public int CurrentLives => currentLives;

        private void Awake()
        {
            ResetLives();
        }

        private void LogLives()
        {
            Debug.Log($"[LifeSystem] Vidas actuales: {currentLives}", this);
        }

        public void ResetLives()
        {
            currentLives = startingLives;
            LogLives();
            OnLivesChanged?.Invoke(currentLives);
        }

        public void LoseLife(int amount = 1)
        {
            currentLives -= amount;
            currentLives = Mathf.Max(0, currentLives);

            LogLives();
            OnLivesChanged?.Invoke(currentLives);

            if (currentLives == 0)
            {
                Debug.Log("[LifeSystem] Sin vidas!", this);
                OnNoLivesLeft?.Invoke();
            }
        }

        public void GainLife(int amount = 1)
        {
            currentLives += amount;

            LogLives();
            OnLivesChanged?.Invoke(currentLives);
        }
    }
}