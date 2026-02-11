using UnityEngine;

namespace Gameplay.Minigames.Skillbar
{
    public class SkillbarSelector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform bar;
        [SerializeField] private RectTransform selector;

        [Header("Speed")]
        [SerializeField] private float baseSpeed = 200f;
        [SerializeField] private float maxSpeed = 400f;

        private float speed;
        private float minX;
        private float maxX;
        private float direction = 1f;
        private bool isMoving;

        public float CurrentX => selector.anchoredPosition.x;

        public void Setup(int playerScore)
        {
            float t = Mathf.Clamp01(playerScore / 100f);
            speed = Mathf.Lerp(baseSpeed, maxSpeed, t);

            Canvas.ForceUpdateCanvases();
            CalculateLimits();

            selector.anchoredPosition =
                new Vector2(minX, selector.anchoredPosition.y);
        }

        private void CalculateLimits()
        {
            float barHalf = bar.rect.width * 0.5f;
            float selectorHalf = selector.rect.width * 0.5f;

            minX = -barHalf + selectorHalf;
            maxX = barHalf - selectorHalf;
        }

        public void StartMoving()
        {
            direction = 1f;
            isMoving = true;
        }

        public void StopMoving()
        {
            isMoving = false;
        }

        private void Update()
        {
            if (!isMoving) return;

            Vector2 pos = selector.anchoredPosition;
            pos.x += direction * speed * Time.deltaTime;

            if (pos.x >= maxX)
            {
                pos.x = maxX;
                direction = -1f;
            }
            else if (pos.x <= minX)
            {
                pos.x = minX;
                direction = 1f;
            }

            selector.anchoredPosition = pos;
        }
    }
}