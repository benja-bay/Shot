using System.Collections;
using UnityEngine;
using Gameplay.Minigames.ShotGrab.Data;

namespace Gameplay.Minigames.ShotGrab.Core
{
    public class ShotSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Shot shotPrefab;
        [SerializeField] private Transform[] laneSpawnPoints;

        [Header("Shot Types")]
        [SerializeField] private ShotData[] availableShots;

        [Header("Settings")]
        [SerializeField] private float shotSpeed = 5f;
        [SerializeField] private float spawnCooldown = 1.2f;
        [SerializeField] private bool spawnFromRight = true;

        private Coroutine spawnRoutine;

        public void StartSpawning()
        {
            spawnRoutine = StartCoroutine(SpawnLoop());
        }

        public void StopSpawning()
        {
            if (spawnRoutine != null)
                StopCoroutine(spawnRoutine);
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                SpawnShot();
                yield return new WaitForSeconds(spawnCooldown);
            }
        }

        private void SpawnShot()
        {
            if (availableShots.Length == 0)
                return;

            int lane = Random.Range(0, laneSpawnPoints.Length);
            int shotIndex = Random.Range(0, availableShots.Length);

            ShotData data = availableShots[shotIndex];

            Vector2 direction = spawnFromRight ? Vector2.left : Vector2.right;

            Shot shot = Instantiate(
                shotPrefab,
                laneSpawnPoints[lane].position,
                Quaternion.identity
            );

            shot.Init(data, shotSpeed, lane, direction);
        }
    }
}