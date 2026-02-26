using UnityEngine;
using Gameplay.Minigames.ShotGrab.Data;

namespace Gameplay.Minigames.ShotGrab.Core
{
    [RequireComponent(typeof(Collider2D))]
    public class Shot : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float maxLifetime = 6f;

        [Header("Visual Root")]
        [SerializeField] private Transform visualRoot;

        private float speed;
        private Vector2 direction;
        private float lifeTimer;

        private GameObject currentVisual;

        public int LaneIndex { get; private set; }
        public ShotData Data { get; private set; }

        public void Init(
            ShotData data,
            float speed,
            int laneIndex,
            Vector2 direction)
        {
            Data = data;
            this.speed = speed;
            LaneIndex = laneIndex;
            this.direction = direction.normalized;
            lifeTimer = 0f;

            SetupVisual();
        }

        private void SetupVisual()
        {
            if (Data.visualPrefab == null)
                return;

            if (currentVisual != null)
                Destroy(currentVisual);

            currentVisual = Instantiate(
                Data.visualPrefab,
                visualRoot
            );

            currentVisual.transform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);

            lifeTimer += Time.deltaTime;
            if (lifeTimer >= maxLifetime)
            {
                Destroy(gameObject);
            }
        }

        public void Consume()
        {
            Destroy(gameObject);
        }
    }
}