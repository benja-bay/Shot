using Gameplay.Minigames.ShotGrab.Core;
using UnityEngine;

namespace Gameplay.Minigames.ShotGrab.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class HandController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        [Header("Lanes")]
        [SerializeField] private Transform[] lanes;

        [Header("Game Flow")]
        [SerializeField] private ShotGrabGame shotGrabGame;

        private float targetY;

        private void Awake()
        {
            targetY = transform.position.y;
        }

        public void MoveVertical(float deltaY)
        {
            targetY = Mathf.Clamp(targetY + deltaY, minY, maxY);
            transform.position = new Vector3(
                transform.position.x,
                targetY,
                transform.position.z
            );
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Shot shot = other.GetComponent<Shot>();
            if (shot == null) return;

            int handLane = GetCurrentLaneIndex();

            if (shot.LaneIndex == handLane)
            {
                shot.Consume();
                shotGrabGame.NotifyShotGrabbed(shot.Data);
            }
        }

        private int GetCurrentLaneIndex()
        {
            float closest = float.MaxValue;
            int index = 0;

            for (int i = 0; i < lanes.Length; i++)
            {
                float dist = Mathf.Abs(transform.position.y - lanes[i].position.y);
                if (dist < closest)
                {
                    closest = dist;
                    index = i;
                }
            }

            return index;
        }
    }
}