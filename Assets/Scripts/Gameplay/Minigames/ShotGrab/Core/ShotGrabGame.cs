using System;
using UnityEngine;

namespace Gameplay.Minigames.ShotGrab.Core
{
    public class ShotGrabGame : MonoBehaviour
    {
        public event Action<int> OnShotGrabbed;

        [SerializeField] private ShotSpawner spawner;

        private bool isRunning;

        public void StartGame()
        {
            isRunning = true;
            spawner.StartSpawning();
        }

        public void StopGame()
        {
            isRunning = false;
            spawner.StopSpawning();
        }

        public void NotifyShotGrabbed(int points)
        {
            if (!isRunning) return;

            OnShotGrabbed?.Invoke(points);
        }
    }
}