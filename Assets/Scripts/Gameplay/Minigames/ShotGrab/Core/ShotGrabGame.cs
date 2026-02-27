using System;
using UnityEngine;
using Gameplay.Minigames.ShotGrab.Data;

namespace Gameplay.Minigames.ShotGrab.Core
{
    public class ShotGrabGame : MonoBehaviour
    {
        public event Action<ShotData> OnShotGrabbed;

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

        public void NotifyShotGrabbed(ShotData data)
        {
            if (!isRunning) return;

            OnShotGrabbed?.Invoke(data);
        }
    }
}